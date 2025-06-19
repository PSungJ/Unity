using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [SerializeField] private Light flash;
    [SerializeField] private AudioSource source;
    public AudioClip flashClip;
    void Start()
    {
        flash = GetComponent<Light>();
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            flash.enabled = !flash.enabled;
            source.PlayOneShot(flashClip, 1.0f);
        }
    }
}
