using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//�Ѿ˿� ���� ������ �跲 ���� ȿ�� ����
//1.��������Ʈ 2. ���� ���� ���� 3.�������� ���ĸ� ���� �������ٵ� �̿��ؼ�
public class BarrelCtrl : MonoBehaviour
{
    [SerializeField] GameObject expEffect;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip expClip;
    int hitcount = 0;
    [SerializeField] Rigidbody rb;
    [SerializeField] MeshRenderer _renderer;
    [SerializeField] Texture[] textures;
    [SerializeField] Mesh[] meshes;
    //[SerializeField] Shake shake;
    //[SerializeField] CameraControl cameraCtrl;
     private MeshFilter meshFilter;

    private float radiuse = 20f; //���Ĺݰ� 
    private readonly string bulletTag = "BULLET";
    //public delegate void EnemyDie();
   //public static event EnemyDie OnEnemyDie;
    public delegate void ShakeHandler();
    public static event ShakeHandler OnShake;
    private int barrelLayer;
    private int enemyLayer;
    void Start()
    {
        barrelLayer = LayerMask.NameToLayer("BARREL");
        enemyLayer = LayerMask.NameToLayer("ENEMY");
        rb = GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();
        _renderer = GetComponent<MeshRenderer>();
         meshFilter = GetComponent<MeshFilter>();
        textures = Resources.LoadAll<Texture>("Textures");
        _renderer.material.mainTexture = textures[Random.Range(0, textures.Length)];
        //cameraCtrl = Camera.main.GetComponent<CameraControl>();
        //cameraCtrl.StopCameraShake();
    }
    private void OnCollisionEnter(Collision col)
    {
        if(col.collider.CompareTag(bulletTag))
        {
            if(++hitcount ==3)
            {
                ExpolosionBarrel();
            }

        }
    }
    void ExpolosionBarrel()
    {
        var exp = Instantiate(expEffect,transform.position,Quaternion.identity);
        Destroy(exp,1.5f);
        source.PlayOneShot(expClip, 1.0f);
        int idx = Random.Range(0, meshes.Length);
        meshFilter.sharedMesh = meshes[idx];
        //��׷��� �޽��� ���� 
        Collider[] colls = Physics.OverlapSphere(transform.position, radiuse,1<<barrelLayer|1<<enemyLayer);
        //�跲��ġ���� 20�ݰ濡 �ִ� �跲 �浹ü�� colls �迭�� �ϳ��� �ִ´�. 
        foreach(Collider coll in colls)
        {
            var _rb = coll.GetComponent<Rigidbody>();
            _rb.mass = 1.0f;//���Ը� 1�� ���� 
            _rb.AddExplosionForce(1000f, transform.position, radiuse, 800f);
            //���ķ�  , ��ġ       ,�ݰ�          ,���μڱ�ġ�� �� 
            coll.gameObject.SendMessage("Die",SendMessageOptions.DontRequireReceiver);
            
        }
        //shake.shakeRotate = true;
        //StartCoroutine(shake.ShakeCamera(0.25f,0.1f ,0.003f));
       // cameraCtrl.ShakeCamera();
       OnShake();
       //OnEnemyDie();
    }
}
