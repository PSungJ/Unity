using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    private Animator ani;
    private bool isScope = false;

    void Start()
    {
        ani = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isScope = !isScope;
            ani.SetBool("isZoom", isScope);
        }
    }
}
