using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightChange : MonoBehaviour
{
    public Light whiteLight;
    public Light yellowLight;
    public Light blueLight;
    public AudioSource Wsource;
    public AudioSource Ysource;
    public AudioSource Bsource;
    public AudioClip clip;

    void Start()
    {
        whiteLight = GetComponentsInChildren<Light>()[0];
        yellowLight = GetComponentsInChildren<Light>()[1];
        blueLight = GetComponentsInChildren<Light>()[2];
        Wsource = GetComponentsInChildren<AudioSource>()[0];
        Ysource = GetComponentsInChildren<AudioSource>()[1];
        Bsource = GetComponentsInChildren<AudioSource>()[2];
        TurnOnLight();
    }
    void TurnOnLight()
    {
        StartCoroutine(LightOnOff());
    }
    IEnumerator LightOnOff()
    {
        whiteLight.enabled = true;
        yellowLight.enabled = false;
        blueLight.enabled = false;
        Wsource.PlayOneShot(clip);
        yield return new WaitForSeconds(3f);

        whiteLight.enabled = false;
        yellowLight.enabled = true;
        blueLight.enabled = false;
        Ysource.PlayOneShot(clip);
        yield return new WaitForSeconds(3f);

        whiteLight.enabled = false;
        yellowLight.enabled = false;
        blueLight.enabled = true;
        Bsource.PlayOneShot(clip);
        yield return new WaitForSeconds(3f);

        TurnOnLight();
    }
}
