using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMoveAndRotate : MonoBehaviour
{
    private Rigidbody rb;
    private Transform tr;
    private TankInput input;
    private float moveSpeed = 0f;
    private float rotSpeed = 0f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0f, -0.5f, 0f);
        tr = GetComponent<Transform>();
        input = GetComponent<TankInput>();
        moveSpeed = 10f;
        rotSpeed = 10f;
    }

    void FixedUpdate()
    {
        // 이동
        Vector3 move = (Vector3.forward * input.v);
        tr.Translate(Vector3.forward * input.v * moveSpeed * Time.deltaTime);
        rb.MovePosition(rb.position + move * moveSpeed  * Time.deltaTime);

        // 회전
        float turn = input.h * rotSpeed * Time.deltaTime;
        rb.rotation = rb.rotation * Quaternion.Euler(0f, turn, 0f);
    }
}
