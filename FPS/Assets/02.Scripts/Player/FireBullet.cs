using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
//마우스 왼쪽    클릭으로 총알을 발사하는 스크립트
//뭐가 필요? 1.FirePos 발사위치  2.총알프리팹 3. 오디오소스 오디오클립
public class FireBullet : MonoBehaviour
{
    public Transform FirePos; //총알 발사 위치
    public GameObject BulletPrefab; //총알 프리팹
    public AudioClip FireSound; //총알 발사 사운드 클립
    private AudioSource Source; //오디오 소스 컴포넌트
    public ParticleSystem muzzleFlash;  // 총구 이펙트
    public ParticleSystem cartridge;

    [Header ("Reload")]
    public readonly float reloadTime = 1.4f;    // 재장전 시간
    public readonly int maxBullet = 10;         // 최대 총알 수
    public int currentBullet = 10;              // 현재 총알 수
    public bool isReloading = false;            // 재장전 여부
    public Animation ani;
    WeaponChange weaponChange;
    PlayerHandAnimation playerHandAnimation;
    private float timePrev;
    void Start()
    {
        playerHandAnimation = GetComponent<PlayerHandAnimation>();
        weaponChange = GetComponent<WeaponChange>();
        Source = GetComponent<AudioSource>(); //오디오 소스 컴포넌트를 가져온다.
        ani = this.transform.GetChild(0).GetChild(0).GetComponent<Animation>();
        muzzleFlash = GetComponentsInChildren<ParticleSystem>()[0];
        cartridge = GetComponentsInChildren<ParticleSystem>()[1];
        timePrev = Time.time;
    }
    void Update()
    {
        if (Input.GetMouseButton(0) && weaponChange.isHaveAR && playerHandAnimation.isRunning == false && !isReloading)
        {
            if (Time.time - timePrev > 0.1f)
            {
                Fire();
                timePrev = Time.time;
            }
        }
        if (Input.GetMouseButtonDown(0) && !isReloading && playerHandAnimation.isRunning == false) //마우스 왼쪽 버튼 클릭 시
        {
            muzzleFlash.Play();
            cartridge.Play();
            Fire(); //총알 발사 함수 호출
        }
        else if (Input.GetMouseButtonUp(0))
        {
            muzzleFlash.Stop();
            cartridge.Stop();
        }
    }
    void Fire()
    {              //what    ,   where            , how rotaion
        Instantiate(BulletPrefab, FirePos.position, FirePos.rotation);
        //총알 프리팹을 발사 위치와 회전으로 인스턴스화
        Source.PlayOneShot(FireSound,1.0f); //총알 발사 사운드 재생
        isReloading = (--currentBullet % maxBullet == 0);
        if (isReloading)
        {
            StartCoroutine(Reload());
            Source.Stop();
        }
    }
    IEnumerator Reload()
    {
        ani.Play("pump2");
        yield return new WaitForSeconds(reloadTime);
        currentBullet = maxBullet;
        isReloading = false;
    }
}
