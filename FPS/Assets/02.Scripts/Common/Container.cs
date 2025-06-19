using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    private readonly string bulletTag = "BULLET";
    public GameObject hitEffectPrefab;
    public AudioSource source;
    public AudioClip hitSound;
    void Start()
    {
        source = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(bulletTag))
        {
            var hitEffect = Instantiate(hitEffectPrefab, col.contacts[0].point, Quaternion.identity);
            //col.tarnsform.position
            Destroy(hitEffect, 1.5f);
            source.PlayOneShot(hitSound, 1f);
        }
    }
}
