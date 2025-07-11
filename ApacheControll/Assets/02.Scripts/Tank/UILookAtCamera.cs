using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILookAtCamera : MonoBehaviour
{
    public Transform mainCameraTr;
    private Transform thisTr;

    void Start()
    {
        thisTr = transform;
        mainCameraTr = Camera.main.transform;
    }

    void Update()
    {
        thisTr.LookAt(mainCameraTr);
    }
}
