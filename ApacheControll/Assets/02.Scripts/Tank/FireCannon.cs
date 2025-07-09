using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class FireCannon : MonoBehaviour
{
    private TankInput input;
    private AudioSource source;
    [SerializeField] private Transform firePos;
    [SerializeField] private LaserBeam beam;
    [SerializeField] private GameObject expEffect;
    [SerializeField] private AudioClip fireClip;
    [SerializeField] private AudioClip expClip;
    [SerializeField] private int terrainLayer;
    public bool isHit = false;
    Ray ray;
    Vector3 hitPoint;
    Vector3 normal;
    Quaternion rot;
    GameObject eff;

    void Start()
    {
        input = GetComponent<TankInput>();
        source = GetComponent<AudioSource>();
        firePos = transform.GetChild(4).GetChild(1).GetChild(1).transform;
        beam = firePos.GetComponentInChildren<LaserBeam>();
        expEffect = Resources.Load<GameObject>("Effects/BigExplosionEffect");
        fireClip = Resources.Load<AudioClip>("Sounds/ShootMissile");
        expClip = Resources.Load<AudioClip>("Sounds/DestroyedExplosion");
        terrainLayer = LayerMask.GetMask("TERRAIN");
    }

    void Update()
    {
        if (input.isFire)
        {
            Fire();
        }
    }
    void Fire()
    {
        source.PlayOneShot(fireClip, 1f);
        RaycastHit hit;
        ray = new Ray (firePos.position, firePos.forward);
        if (Physics.Raycast(ray, out hit, 200f, terrainLayer))
            isHit = true;
        else
            isHit = false;

        beam.FireRay(); // LineRenderer ȣ��
        ShowEffect(hit);
    }
    void ShowEffect(RaycastHit hit)
    {
        if (isHit)
        {
            hitPoint = hit.point;   // Ÿ������
            normal = (firePos.position - hitPoint).normalized;
            rot = Quaternion.FromToRotation(-Vector3.forward, normal);   // ���� ����Ʈ ����

            eff = Instantiate(expEffect, hitPoint, rot);
            source.PlayOneShot(expClip);
            Destroy(eff, 2f);
        }
        else
        {
            hitPoint = ray.GetPoint(200f);  // ���� ������Ʈ�� ���ٸ� 200�������� �ڵ� ����
            normal = (firePos.position - hitPoint).normalized;
            rot = Quaternion.FromToRotation(-Vector3.forward, normal);   // ���� ����Ʈ ����

            eff = Instantiate(expEffect, hitPoint, rot);
            source.PlayOneShot(expClip, 1f);
            Destroy(eff, 2f);
        }
    }
}
