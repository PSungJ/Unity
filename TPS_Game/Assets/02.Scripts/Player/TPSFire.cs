using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DataInfo;
[System.Serializable]
public struct PlayerSfx
{
    public AudioClip[] fire;
    public AudioClip[] reload;
}
//Scriptable ��ũ���ͺ� 


public class TPSFire : MonoBehaviour
{
    public enum WeaponType
    {
      RIFLE=0,SHOTGUN=1
    }
    public WeaponType curweapon = WeaponType.RIFLE;
    public PlayerSfx playerSfx;
    [SerializeField] private Transform firePos;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip fireClip;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem cartrige;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] TPSPlayerInput input;
    [SerializeField] TPSMecanimCtrl tpsctrl;
    private float fireRate = 0.1f;
    private float timePrev;

    //źâ �̹��� UI
    public Image magazineImg;
    public Text magazinetxt;
    public int maxBullet = 10; //�ִ��Ѿ˼� 
    public int remainingBullet = 10;// ���� �Ѿ˼� 

    //public float reloadTime = 2.0f;
    private bool isReloading = false;
    private WaitForSeconds reloadWs;

    private readonly int hashReload = Animator.StringToHash("Reload");

    public Sprite[] weaponIcons;
    public Image weaponImg;

    public RifleGunData rifleGunData;
   

    // �� ĳ������ ���̾� ���� ���� �� ����
    private int enemyLayer;
    private int barrelLayer;
    private int obstacleLayer;
    private int layerMask;  
    private bool isFire = false;
    private float nextFie;
    public float autoFireRate = 0.15f;
    private readonly string enemyTag = "ENEMY"; 
    void Start()
    {
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        input = GetComponent<TPSPlayerInput>();
        tpsctrl = GetComponent<TPSMecanimCtrl>();
        muzzleFlash = firePos.GetComponentInChildren<ParticleSystem>();
        reloadWs = new WaitForSeconds(2f);
        enemyLayer = LayerMask.NameToLayer("ENEMY");
        barrelLayer = LayerMask.NameToLayer("BARREL");
        obstacleLayer = LayerMask.NameToLayer("OBSTACLE");
        layerMask = 1 << enemyLayer | 1 << barrelLayer | 1 << obstacleLayer; //���̾� ����ũ ����
    }
    void Update()
    {
        Debug.DrawRay(firePos.position, firePos.forward * 20f, Color.green);
        if (EventSystem.current.IsPointerOverGameObject()) return;
        // ��ư�� ��Ҵٸ�  ���� ��η� �������� �ʰ� �������� �̺�Ʈ �� 

        //RaycastHit hit;
        //if(Physics.Raycast(firePos.position, firePos.forward, out hit, 20f, layerMask))
        //{
        //    input.isFiring = (hit.collider.CompareTag(enemyTag)); // �� ĳ���Ͱ� �¾��� �� �߻� ���� 

        //}
        //else
        //{
        //    input.isFiring = false;
        //}
        if(!isReloading && input.isFiring && tpsctrl.isRun == false)
        {
            if(Time.time > nextFie)
            {
            --remainingBullet;
            Fire();
            if(remainingBullet == 0)
            {
                StartCoroutine(Reloading());
            }
            nextFie = Time.time + autoFireRate;
            }
        }
        //if (!isReloading && input.isFiring && tpsctrl.isRun == false)

        //{
        //    if (Time.time - timePrev >= fireRate)
        //    {
        //        timePrev = Time.time;
        //        --remainingBullet;
        //        Fire();
        //        if (remainingBullet == 0)
        //        {
        //            StartCoroutine(Reloading());
        //        }
        //    }
        //}
    }
    IEnumerator Reloading()
    {
        isReloading = true;
        source.PlayOneShot(rifleGunData.reloadClip,1.0f);
        animator.SetTrigger(hashReload);
        yield return reloadWs;
        isReloading = false;
        magazineImg.fillAmount = 1.0f;
        remainingBullet = maxBullet;
        UpdateBulletText();
    }
    void UpdateBulletText()
    {      
        //���� �Ѿ˼�  �ִ� �Ѿ˼� ǥ�� 
        magazinetxt.text = string.Format($"<color=ff0000>{remainingBullet}</color>/{maxBullet}");
    }
    void Fire()
    {   //������ ���� �Լ�( ������ , ��� ,��� ȸ�� �Ұ��ΰ�)
        //Instantiate(bulletPrefab,firePos.position,firePos.rotation);
        if (isReloading) return;
        var _bullet = PoolingManager.p_instance.GetBullet();
        if (_bullet != null)
        {
            _bullet.transform.position = firePos.position;
            _bullet.transform.rotation = firePos.rotation;
            _bullet.SetActive(true);
        }
        source.PlayOneShot(rifleGunData.shotClip, 1.0f);
        cartrige.Play();
        muzzleFlash.Play();
        magazineImg.fillAmount =(float)remainingBullet /(float)maxBullet;
        UpdateBulletText();
    }
    public void OnChageWeapon()
    {
        curweapon=(WeaponType)((int)++curweapon%2);
        weaponImg.sprite = weaponIcons[(int)curweapon%2];
        //source.clip = rifleGunData.shotClip;
    }
}
