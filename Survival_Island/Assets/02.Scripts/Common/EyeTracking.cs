using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeTracking : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    private Transform tr;
    CrossHair crosshair;
    void Start()
    {
        crosshair = GameObject.Find("CrossHair-img").GetComponent<CrossHair>();
        tr = GetComponent<Transform>();
    }

    void Update()
    {
        ray = new Ray (tr.position, tr.forward);
        if (Physics.Raycast(ray, out hit, 20f, 1<<7))
        {
            crosshair.isAiming = true;
        }
        else
        {
            crosshair.isAiming = false;
        }
    }
}
