using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip expSound;
    [SerializeField] private GameObject expEffect;
    [SerializeField] private Texture[] textures;
    [SerializeField] private Mesh[] meshes;
    [SerializeField] private MeshFilter filter;

    private readonly string bulletTag = "BULLET";
    private int hitcount = 0;
    private float expRadius = 20f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();
        meshRenderer = GetComponent<MeshRenderer>();
        textures = Resources.LoadAll<Texture>("Textures");
        meshRenderer.material.mainTexture = textures[Random.Range(0, textures.Length)];
        filter = GetComponent<MeshFilter>();
    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(bulletTag))
        {
            if (++hitcount == 3)
            {
                Explosion();
            }
        }
    }

    void Explosion()
    {
        var exp = Instantiate(expEffect, transform.position, Quaternion.identity);
        Destroy(exp, 1.5f);
        source.PlayOneShot(expSound, 1.0f);
        int index = Random.Range(0, meshes.Length);
        filter.sharedMesh = meshes[index];

        Collider[] colls = Physics.OverlapSphere(transform.position, expRadius, 1 << 6);
        foreach (Collider coll in colls)
        {
            var _rb = coll.GetComponent<Rigidbody>();
            _rb.mass = 1.0f;
            _rb.AddExplosionForce(120f, transform.position, expRadius, 50f);
        }
    }
}
