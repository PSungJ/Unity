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

    void FixedUpdate()  // ��Ȯ�� �������� ���� �����̳� ��Ȯ�� �����Ӵ�� �����ϰ� �ʹٸ�
    {
        Vector3 moveDir = (Vector3.forward * input.moveZ) + (Vector3.right * input.moveX);
        tr.Translate(moveDir.normalized * Time.fixedDeltaTime * moveSpeed);
        tr.Rotate(Vector3.up * input.mouseRotate * Time.fixedDeltaTime * rotSpeed);
        //rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);

        //// �ӵ��� �ִ� �ӵ��� ����
        //if (rb.velocity.magnitude > maxSpeed)
        //    rb.velocity = rb.velocity.normalized * maxSpeed;
    }
}
