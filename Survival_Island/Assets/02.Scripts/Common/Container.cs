using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    private readonly string bulletTag = "BULLET";
    public AudioSource source;
    public AudioClip hitSound;
    public GameObject hitEffectPrefab;
    void Start()
    {
        source = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(bulletTag))
        {
            col.gameObject.SetActive(false);
            var hitEffect = Instantiate(hitEffectPrefab, col.contacts[0].point, Quaternion.identity);
            Destroy(hitEffect, 0.5f);
            source.PlayOneShot(hitSound, 1f);
        }
    }
}
