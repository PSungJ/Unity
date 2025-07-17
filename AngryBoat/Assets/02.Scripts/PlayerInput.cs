using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string Verti = "Vertical";
    public string Hori = "Horizontal";
    public float h = 0f;
    public float v = 0f;
    public bool isMouseClick = false;
    void Start()
    {
        
    }
    void Update()
    {
        h = Input.GetAxis(Hori);
        v = Input.GetAxis(Verti);
        isMouseClick = Input.GetMouseButtonDown(0);
    }
}
