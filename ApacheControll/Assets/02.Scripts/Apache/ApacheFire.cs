using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApacheFire : MonoBehaviour
{
    private AudioSource source;
    [SerializeField] private AudioClip fireClip;
    [SerializeField] private AudioClip expClip;
    [SerializeField] private Transform leftFirePos;
    [SerializeField] private Transform rigthFirePos;
    [SerializeField] private LaserBeam leftBeam;
    [SerializeField] private LaserBeam rightBeam;
    [SerializeField] private GameObject expEffect;
    [SerializeField] private int terrainLayer;
    private bool isHit = false;
    private float fireRate = 0.1f;
    private float nextFireTime = 0f;
    Ray ray;
    Vector3 hitPoint;
    Vector3 normal;
    Quaternion rot;
    GameObject eff;
    void Start()
    {
        source = GetComponent<AudioSource>();
        fireClip = Resources.Load<AudioClip>("Sounds/ShootMissile");
        expClip = Resources.Load<AudioClip>("Sounds/DestroyedExplosion");
        leftFirePos = transform.GetChild(3).transform;
        rigthFirePos = transform.GetChild(4).transform;
        leftBeam = leftFirePos.GetComponentInChildren<LaserBeam>();
        rightBeam = rigthFirePos.GetComponentInChildren<LaserBeam>();
        expEffect = Resources.Load<GameObject>("Effects/BigExplosionEffect");
        terrainLayer = LayerMask.GetMask("TERRAIN");
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Fire();
        }
    }

    void Fire()
    {
        source.PlayOneShot(fireClip, 1f);
        RaycastHit hit;
        ray = new Ray(leftFirePos.position, leftFirePos.forward);
        if (Physics.Raycast(ray, out hit, 200f, terrainLayer))
            isHit = true;
        else
            isHit = false;

        leftBeam.FireRay();
        rightBeam.FireRay();
        ShowEffect(hit);
    }
    void ShowEffect(RaycastHit hit)
    {
        if (isHit)
        {
            hitPoint = hit.point;
            normal = (leftFirePos.position - hitPoint).normalized;
            rot = Quaternion.FromToRotation(-Vector3.forward, normal);

            eff = Instantiate(expEffect, hitPoint, rot);
            source.PlayOneShot(expClip);
            Destroy(eff, 2f);
        }
        else
        {
            hitPoint = ray.GetPoint(200f);
            normal = (leftFirePos.position - hitPoint).normalized;
            rot = Quaternion.FromToRotation(-Vector3.forward, normal);

            eff = Instantiate(expEffect, hitPoint, rot);
            source.PlayOneShot(expClip, 1f);
            Destroy(eff, 2f);
        }
    }
}
