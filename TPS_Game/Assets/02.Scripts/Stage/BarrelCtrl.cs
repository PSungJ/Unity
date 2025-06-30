using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//총알에 세번 맞으면 배럴 폭파 효과 구현
//1.폭파이펙트 2. 폭파 사운드 구현 3.물리적인 폭파를 구현 리디지바디를 이용해서
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

    private float radiuse = 20f; //폭파반경 
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
        //찌그러진 메쉬를 적용 
        Collider[] colls = Physics.OverlapSphere(transform.position, radiuse,1<<barrelLayer|1<<enemyLayer);
        //배럴위치에서 20반경에 있는 배럴 충돌체를 colls 배열에 하나씩 넣는다. 
        foreach(Collider coll in colls)
        {
            var _rb = coll.GetComponent<Rigidbody>();
            _rb.mass = 1.0f;//무게를 1로 변경 
            _rb.AddExplosionForce(1000f, transform.position, radiuse, 800f);
            //폭파력  , 위치       ,반경          ,위로솟구치는 힘 
            coll.gameObject.SendMessage("Die",SendMessageOptions.DontRequireReceiver);
            
        }
        //shake.shakeRotate = true;
        //StartCoroutine(shake.ShakeCamera(0.25f,0.1f ,0.003f));
       // cameraCtrl.ShakeCamera();
       OnShake();
       //OnEnemyDie();
    }
}
