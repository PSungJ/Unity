using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 필요한것 : 1. 총알프리팹, 2. 발사위치, 3. 발사타이밍 시간조절, 4. 파티클이펙트, 5. 사운드
public class TPSFire : MonoBehaviour
{
    private readonly int hashReload = Animator.StringToHash("Reload");
    [SerializeField] private Transform firePos;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip fireSound;
    [SerializeField] private AudioClip reloadSfx;
    //[SerializeField] private ParticleSystem cartrige;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private Animator ani;
    private float fireRate = 0.1f;
    private float timePrev;
    private TPSPlayerInput input;

    private readonly float reloadTime = 2.0f;
    private readonly int maxBullet = 10;
    private int curBullet = 10;
    private bool isReload = false;
    private WaitForSeconds wsReload;
    void Start()
    {
        source = GetComponent<AudioSource>();
        input = GetComponent<TPSPlayerInput>();
        ani = GetComponent<Animator>();
        muzzleFlash = firePos.GetComponentInChildren<ParticleSystem>();
        timePrev = Time.time;
        wsReload = new WaitForSeconds(reloadTime);
    }

    void Update()
    {
        if (input.fire && !isReload)
        {
            if (Time.time - timePrev > fireRate)
            {
                timePrev = Time.time;
                Fire();
            }
        }
    }

    private void Fire()
    {
        //Instantiate(BulletPrefab, firePos.position, firePos.rotation);
        var _bullet = PoolingManager.p_Instance.GetBullet();
        if (_bullet != null)
        {
            _bullet.transform.position = firePos.position;
            _bullet.transform.rotation = firePos.rotation;
            _bullet.SetActive(true);
        }
        source.PlayOneShot(fireSound, 1f);
        //cartrige.Play();
        muzzleFlash.Play();
        isReload  = (--curBullet % maxBullet == 0);
        if (isReload)
            StartCoroutine(Reloading());
    }
    IEnumerator Reloading()
    {
        ani.SetTrigger(hashReload);
        source.PlayOneShot(reloadSfx, 1f);
        yield return wsReload;

        curBullet = maxBullet;
        isReload = false;
    }
}
