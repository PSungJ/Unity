using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//1. �Ѿ� ,���� �÷��̾� �������� ��� ,��� �ִϸ��̼� , ��ݽ� ���ʰ������� �߻����� 
// ��� �Ķ���� ������ �ؽð����� ã�Ƽ� int������ ��ȯ // �÷��̾� �������� ȸ����  damping��
// �Һ��� isFire== ���ε�� �Ѿ˹߻� ���� 
public class EnemyFire : MonoBehaviour
{
    [SerializeField] private GameObject E_bullet;
    [SerializeField] private Transform E_firePos;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip fireSfx;
    [SerializeField] private Transform playerTr;
    [SerializeField] private Transform enemyTr;
    [SerializeField] private Animator animator;
    private readonly int hashFire = Animator.StringToHash("Fire");
    private readonly int hashReload = Animator.StringToHash("Reload");
    private float nextFire = 0f;  //�ð����� ���� 
    private float fireRate = 0.1f; //�߻� ����
    public bool isFire=false;
    private float damping = 10f;

    // ������ ���� ��� �ʵ� 
    private readonly float reloadTime = 2.0f; // ������ �ð�
    private readonly int maxBullet = 10; //źâ �ִ� �Ѿ� ��
    private int curBullet = 10;
    private bool isReload = false;
    private WaitForSeconds wsReload;
    public AudioClip reloadSfx;
    public MeshRenderer muzzleFlash;
    void Start()
    {
        //���۳�Ʈ �ʱ�ȭ 
        playerTr = GameObject.FindWithTag("Player").transform;
        enemyTr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        wsReload = new WaitForSeconds(reloadTime);
    }
    void Update()
    {
        if(!isReload && isFire)
        {
            if(Time.time >=nextFire)
            {
                Fire();
                nextFire = Time.time + fireRate + Random.Range(0.0f, 0.3f);
            }
            Quaternion rot = Quaternion.LookRotation(playerTr.position - enemyTr.position);
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime *damping);

        }

    }
    void Fire()
    {
        var E_bullet = PoolingManager.p_instance.GetE_Bullet();
        E_bullet.transform.position = E_firePos.position;
        E_bullet.transform.rotation = E_firePos.rotation;
        E_bullet.gameObject.SetActive(true);
        animator.SetTrigger(hashFire);
        source.PlayOneShot(fireSfx, 1.0f);
        isReload =(--curBullet % maxBullet==0);
        if (isReload)
            StartCoroutine(Reloading());

        StartCoroutine(ShowMuzzleFlash());
    }
    IEnumerator ShowMuzzleFlash()
    {
        muzzleFlash.enabled = true;
        muzzleFlash.transform.localScale = Vector3.one * Random.Range(1.0f, 2.0f);
        Quaternion rot = Quaternion.Euler(Vector3.forward * Random.Range(0, 360));
        muzzleFlash.transform.localRotation = rot;
        yield return new WaitForSeconds(Random.Range(0.05f,0.2f));

        muzzleFlash.enabled = false;

    }
    IEnumerator Reloading()
    {
        animator.SetTrigger(hashReload);
        source.PlayOneShot(reloadSfx, 1.0f);
        yield return wsReload; //2�ʰ� ��� �ϴٰ� 

        curBullet = maxBullet;
        isReload = false;
    }
}
