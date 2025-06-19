using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossHair : MonoBehaviour
{
    private Transform tr;
    public Image Aim;
    private float startTime = 0f;
    private float duration = 0.3f;
    private float minAimsize = 0.7f;
    private float maxAimsize = 1.2f;
    private Color originColor = Color.white;
    private Color changeColor = Color.red;

    public bool isAiming = false;

    void Start()
    {
        Aim = GetComponent<Image>();
        Aim.color = originColor;
        tr = GetComponent<Transform>();
        startTime = Time.time;
        tr.localScale = Vector3.one * minAimsize;
    }

    void Update()
    {
        if (isAiming)
        {
            float time = (Time.time - startTime) / duration;
            tr.localScale = Vector3.one * Mathf.Lerp(minAimsize, maxAimsize, time);
            Aim.color = changeColor;
        }
        else
        {
            tr.localScale = Vector3.one * minAimsize;
            Aim.color = originColor;
            startTime = Time.time;
        }
    }
}
