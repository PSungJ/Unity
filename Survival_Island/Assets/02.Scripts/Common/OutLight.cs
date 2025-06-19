using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutLight : MonoBehaviour
{
    private Light wLight;
    private Light bLight;
    private Light yLight;
    void Start()
    {
        wLight = GetComponentsInChildren<Light>()[0];
        bLight = GetComponentsInChildren<Light>()[1];
        yLight = GetComponentsInChildren<Light>()[2];

        Lighting();
    }
    void Lighting()
    {
        StartCoroutine(LightOnOff());
    }
    IEnumerator LightOnOff()
    {
        wLight.enabled = true;
        bLight.enabled = false;
        yLight.enabled = false;
        yield return new WaitForSeconds(3f);

        wLight.enabled = false;
        bLight.enabled = true;
        yLight.enabled = false;
        yield return new WaitForSeconds(3f);

        wLight.enabled = false;
        bLight.enabled = false;
        yLight.enabled = true;
        yield return new WaitForSeconds(3f);

        Lighting();
    }
    
}
