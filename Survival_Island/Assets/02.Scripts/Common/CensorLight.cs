using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CensorLight : MonoBehaviour
{
    public Light censorLight;
    public AudioSource source;
    public AudioClip clip;
    void Start()
    {
        censorLight = GetComponent<Light>();
        source = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            censorLight.enabled = true;
            source.PlayOneShot(clip);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            censorLight.enabled = false;
        }
    }
    void Update()
    {
        
    }
}
