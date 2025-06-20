using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 3번 맞으면 폭파
// 1. 폭파이펙트, 사운드, 물리적인 표현
public class BarrelCtrl : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private Texture[] textures;
    [SerializeField] private Mesh[] meshes;
    private MeshFilter meshFilter;
    int count = 0;
    private float radius = 20f;     // 폭파 반경
    private readonly string bulletTag = "BULLET";
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();
        _renderer = GetComponent<MeshRenderer>();
        textures = Resources.LoadAll<Texture>("Textures");
        _renderer.material.mainTexture = textures[Random.Range(0, textures.Length)];
        meshFilter = GetComponent<MeshFilter>();
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.CompareTag(bulletTag))
        {
            if (++count == 3)
            {
                ExplosionBarrel();
            }
        }
    }
    void ExplosionBarrel()
    {
        var exp = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(exp, 1.5f);
        source.PlayOneShot(explosionSound, 1f);
        int idx = Random.Range(0, meshes.Length);
        meshFilter.sharedMesh = meshes[idx];

        Collider[] colls = Physics.OverlapSphere(transform.position, radius, 1 << 13);
        // Barrel 위치에서 20 반경에 있는 Barrel 충돌체를 cols 배열에 하나씩 넣는다.
        foreach (Collider coll in colls)
        {
            var _rb = coll.GetComponent<Rigidbody>();
            _rb.mass = 1f;
            _rb.AddExplosionForce(120f, transform.position, radius, 50f);
                                    //폭파력, 위치, 반경, 위로 솟는힘
        }
    }
}
