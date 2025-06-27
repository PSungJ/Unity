using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireBullet : MonoBehaviour
{
    public Transform FirePos;
    public AudioClip FireSound;
    private AudioSource Source;
    public Animation ani;
    public ParticleSystem muzzleFlash;
    public ParticleSystem cartridge;
    private readonly string enemyTag = "ENEMY";
    [Header("Reload")]
    public float reloadTime = 1.5f;
    public int maxBullet = 10;
    public int curBullet = 10;
    public bool isReloading = false;

    PlayerControll playerAni;

    private int enemyLayer;
    private int barrelLayer;
    private int layerMask;
    private bool isFire = false;
    private float nextFire;
    public float autoFireRate = 0.5f;
    void Start()
    {
        ani = this.transform.GetChild(0).GetChild(0).GetComponent<Animation>();
        Source = GetComponent<AudioSource>();
        muzzleFlash = GetComponentsInChildren<ParticleSystem>()[0];
        cartridge = GetComponentsInChildren<ParticleSystem>()[1];
        playerAni = GetComponent<PlayerControll>();

        enemyLayer = LayerMask.NameToLayer("Enemy");
        barrelLayer = LayerMask.NameToLayer("Barrel");
        layerMask = 1 << enemyLayer | 1 << barrelLayer;
    }
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        Debug.DrawRay(FirePos.position, FirePos.forward * 20f, Color.green);

        RaycastHit hit;
        if (Physics.Raycast(FirePos.position,FirePos.forward, out hit, 20f, layerMask))
        {
            isFire = (hit.collider.CompareTag(enemyTag));
        }
        else
        {
            isFire = false;
        }
        if (!isReloading && isFire)
        {
            if (Time.time > nextFire)
            {
                --curBullet;
                Fire();
                if (curBullet == 0)
                {
                    StartCoroutine(Reload());
                }
                nextFire = Time.time + autoFireRate;
            }
        }

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
