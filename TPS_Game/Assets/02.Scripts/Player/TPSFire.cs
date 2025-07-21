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
//Scriptable 스크립터블 


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

    //탄창 이미지 UI
    public Image magazineImg;
    public Text magazinetxt;
    public int maxBullet = 10; //최대총알수 
    public int remainingBullet = 10;// 남은 총알수 

    //public float reloadTime = 2.0f;
    private bool isReloading = false;
    private WaitForSeconds reloadWs;

    private readonly int hashReload = Animator.StringToHash("Reload");

    public Sprite[] weaponIcons;
    public Image weaponImg;

    public RifleGunData rifleGunData;
   

    // 적 캐릭터의 레이어 값을 저장 할 변수
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
        layerMask = 1 << enemyLayer | 1 << barrelLayer | 1 << obstacleLayer; //레이어 마스크 설정
    }
    void Update()
    {
        Debug.DrawRay(firePos.position, firePos.forward * 20f, Color.green);
        if (EventSystem.current.IsPointerOverGameObject()) return;
        // 버튼에 닿았다면  하위 경로로 내려가지 않고 빠져나감 이벤트 훅 

        //RaycastHit hit;
        //if(Physics.Raycast(firePos.position, firePos.forward, out hit, 20f, layerMask))
        //{
        //    input.isFiring = (hit.collider.CompareTag(enemyTag)); // 적 캐릭터가 맞았을 때 발사 가능 

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
        //남은 총알수  최대 총알수 표시 
        magazinetxt.text = string.Format($"<color=ff0000>{remainingBullet}</color>/{maxBullet}");
    }
    void Fire()
    {   //프리팹 생성 함수( 무엇을 , 어디서 ,어떻게 회전 할것인가)
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
