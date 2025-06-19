using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//마우스 왼쪽    클릭으로 총알을 발사하는 스크립트
//뭐가 필요? 1.FirePos 발사위치  2.총알프리팹 3. 오디오소스 오디오클립
public class FireBullet : MonoBehaviour
{
    public Transform FirePos; //총알 발사 위치
    public AudioClip FireSound; //총알 발사 사운드 클립
    private AudioSource Source; //오디오 소스 컴포넌트
    public Animation ani;
    public ParticleSystem muzzleFlash;
    public ParticleSystem cartridge;
    [Header("Reload")]
    public float reloadTime = 1.5f;
    public int maxBullet = 10;
    public int curBullet = 10;
    public bool isReloading = false;

    PlayerControll playerAni;
    void Start()
    {
        ani = this.transform.GetChild(0).GetChild(0).GetComponent<Animation>();
        Source = GetComponent<AudioSource>();
        muzzleFlash = GetComponentsInChildren<ParticleSystem>()[0];
        cartridge = GetComponentsInChildren<ParticleSystem>()[1];
        playerAni = GetComponent<PlayerControll>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isReloading && !playerAni.isRunning)
        {
            muzzleFlash.Play();
            cartridge.Play();
            Fire();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            muzzleFlash.Stop();
            cartridge.Stop();
        }

    }
    void Fire()
    {
        //Instantiate(BulletPrefab, FirePos.position, FirePos.rotation);
        var _bullet = PoolingManager.p_Instance.GetBullet();
        if (_bullet != null)
        {
            _bullet.transform.position = FirePos.position;
            _bullet.transform.rotation = FirePos.rotation;
            _bullet.SetActive(true);
        }
        Source.PlayOneShot(FireSound,1.0f);

        isReloading = (--curBullet % maxBullet == 0);
        if (isReloading)
        {
            StartCoroutine(Reload());
            Source.Stop();
        }
    }
    IEnumerator Reload()
    {
        ani.Play("pump1");
        yield return new WaitForSeconds(reloadTime);
        curBullet = maxBullet;
        isReloading = false;
    }
}
