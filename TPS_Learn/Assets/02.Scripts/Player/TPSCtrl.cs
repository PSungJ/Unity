using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCtrl : MonoBehaviour
{
    public TPSPlayerInput input;
    private Transform tr;
    private float moveSpeed = 5f;
    public float rotSpeed = 500f;
    [SerializeField] private Animation ani;

    void Start()
    {
        ani = GetComponent<Animation>();
        input = GetComponent<TPSPlayerInput>();
        tr = transform;

    }
    void Update()
    {
        Vector3 moveDir = (Vector3.forward * input.moveZ) + (Vector3.right * input.moveX);
        tr.Translate(moveDir.normalized * Time.deltaTime * moveSpeed);
        tr.Rotate(Vector3.up * input.mouseRotate * Time.deltaTime * rotSpeed);

        // ÀÌµ¿(Legacy, Animation)
        if (input.moveZ >= 0.1f)
        {
            ani.CrossFade("RunF", 0.3f);
            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveSpeed = 10f;
                ani.CrossFade("SprintF", 0.3f);
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
                moveSpeed = 5f;
        }
        else if (input.moveZ <= -0.1f)
            ani.CrossFade("RunB", 0.3f);
        else if (input.moveX >= 0.1f)
            ani.CrossFade("RunR", 0.3f);
        else if (input.moveX <= -0.1f)
            ani.CrossFade("RunL", 0.3f);
        else
            ani.CrossFade("Idle", 0.3f);
    }
}
