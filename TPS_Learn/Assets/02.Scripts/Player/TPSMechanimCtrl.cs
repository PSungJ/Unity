using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSMechanimCtrl : MonoBehaviour
{
    [SerializeField] private Animator ani;
    private Transform tr;
    private TPSPlayerInput input;
    private Rigidbody rb;
    private float moveSpeed = 5f;
    //private float maxSpeed = 10f;
    private float rotSpeed = 500f;
    public bool isRun = false;
    private readonly int hashPosX = Animator.StringToHash("PosX");
    private readonly int hashPosY = Animator.StringToHash("PosY");
    private readonly int hashsprint = Animator.StringToHash("isSprint");

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        input = GetComponent<TPSPlayerInput>();
        ani = GetComponent<Animator>();
        tr = transform;
    }

    void Update()
    {
        if (input.sprint)
        {
            isRun = true;
            moveSpeed = 10f;
        }
        else
        {
            isRun = false;
            moveSpeed = 5f;
        }
        ani.SetFloat(hashPosX, input.moveX, 0.01f, Time.deltaTime);
        ani.SetFloat(hashPosY, input.moveZ, 0.01f, Time.deltaTime);
        ani.SetBool(hashsprint, isRun);
    }

    void FixedUpdate()  // 정확한 물리량에 따른 구현이나 정확한 프레임대로 구현하고 싶다면
    {
        Vector3 moveDir = (Vector3.forward * input.moveZ) + (Vector3.right * input.moveX);
        tr.Translate(moveDir.normalized * Time.fixedDeltaTime * moveSpeed);
        tr.Rotate(Vector3.up * input.mouseRotate * Time.fixedDeltaTime * rotSpeed);
        //rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);

        //// 속도를 최대 속도로 제한
        //if (rb.velocity.magnitude > maxSpeed)
        //    rb.velocity = rb.velocity.normalized * maxSpeed;
    }
}
