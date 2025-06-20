using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// �Ѿ�, ����, �÷��̾� �������� ��ݷ���, ��ݾִϸ��̼�, �߻� ����
// ��� �Ķ���� ������ �ؽð����� ã�Ƽ� int������ ��ȯ
// �÷��̾� �������� ȸ���� damping��
// bool ���� isFire == ���ε�� �Ѿ� �߻� ����
public class EnemyFire : MonoBehaviour
{
    [SerializeField] private GameObject E_Bullet;
    [SerializeField] private Transform firePos;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip fireSound;
    [SerializeField] private Animator ani;
    [SerializeField] private MeshRenderer muzzleFlash;
    private Transform tr;
    private Transform playerTr;
    public bool isFire = false;

    private float nextFire = 0f;    // �ð�����
    private float fireRate = 0.1f;  // �߻� ����
    private float damping = 10f;

    //������ ����
    private readonly float reloadTime = 2.0f;
    private readonly int maxBullet = 30;
    private int curBullet = 30;
    private bool isReload = false;
    private WaitForSeconds wsReload;
    public AudioClip reloadSfx;

    private readonly int hashFire = Animator.StringToHash("Fire");
    private readonly int hashReload = Animator.StringToHash("Reload");
    void Start()
    {
        muzzleFlash.enabled = false;
        source = GetComponent<AudioSource>();
        ani = GetComponent<Animator>();
        tr = GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").transform;
        wsReload = new WaitForSeconds(reloadTime);
    }

    void Update()
    {
        if(!isReload && isFire)
        {
            if (Time.time >= nextFire)
            {
                Fire();
                nextFire = Time.time + fireRate + Random.Range(0.0f, 0.3f);
            }
            Quaternion rot = Quaternion.LookRotation(playerTr.position - tr.position);
            tr.rotation = Quaternion.Slerp(tr.rotation, rot, Time.deltaTime * damping);
        }
    }

    void Fire()
    {
        var E_bullet = PoolingManager.p_Instance.GetE_Bullet();
        if (E_bullet != null)
        {
            E_bullet.transform.position = firePos.position;
            E_bullet.transform.rotation = firePos.rotation;
            E_bullet.SetActive(true);
        }
        ani.SetTrigger(hashFire);
        source.PlayOneShot(fireSound, 1.0f);

        isReload = (--curBullet % maxBullet == 0);

        if (isReload)
            StartCoroutine(Reloading());
        StartCoroutine(ShowMuzzleFlash());
    }
    IEnumerator ShowMuzzleFlash()
    {
        muzzleFlash.enabled = true;
        yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
        muzzleFlash.enabled = false;
    }
    IEnumerator Reloading()
    {
        ani.SetTrigger(hashReload);
        source.PlayOneShot(reloadSfx, 1.0f);
        yield return wsReload;

        curBullet = maxBullet;
        isReload = false;
    }
}
