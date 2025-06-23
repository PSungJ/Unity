using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 3�� ������ ����
// 1. ��������Ʈ, ����, �������� ǥ��
public class BarrelCtrl : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private Texture[] textures;
    [SerializeField] private Mesh[] meshes;
    [SerializeField] Shake shake;
    private MeshFilter meshFilter;
    int count = 0;
    private float radius = 20f;     // ���� �ݰ�
    private readonly string bulletTag = "BULLET";
    void Start()
    {
        shake = Camera.main.GetComponent<Shake>();
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

        Collider[] colls = Physics.OverlapSphere(transform.position, radius, 1 << 10);
        // Barrel ��ġ���� 20 �ݰ濡 �ִ� Barrel �浹ü�� cols �迭�� �ϳ��� �ִ´�.
        foreach (Collider coll in colls)
        {
            var _rb = coll.GetComponent<Rigidbody>();
            _rb.mass = 1f;
            _rb.AddExplosionForce(120f, transform.position, radius, 50f);
                                    //���ķ�, ��ġ, �ݰ�, ���� �ڴ���
        }
        shake.shakeRotate = true;
        StartCoroutine(shake.ShakeCamera(0.3f, 0.25f, 0.03f));
    }
}
