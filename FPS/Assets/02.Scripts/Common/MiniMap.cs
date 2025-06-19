using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    public Image Img;
    private float timePrev;
    void Start()
    {
        Img = GetComponent<Image>();
        timePrev = Time.time;
    }

    void Update()
    {
        if ((Time.time - timePrev) >= 0.3f)
        {
            Img.enabled = !Img.enabled;
            timePrev = Time.time;
        }
    }
}
