using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 1. ���۳�Ʈ���� - �ִϸ�����, RigidBody, Input��ũ��Ʈ
public class WomanMovement : MonoBehaviour
{
    [SerializeField] private WomanInput input;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator ani;
    private int groundLayer;

    [Header("������ ��")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotSpeed = 300f;
    private float currentRotationAmount;
    void Start()
    {
        input = GetComponent<WomanInput>();
        rb = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        groundLayer = LayerMask.GetMask("Ground");
    }

    void FixedUpdate()  // �������� ���� �ֱ⸶�� �ִϸ��̼� ȣ��
    {
        Move();
        Rotate();
    }
    private void Move()
    {
        // RigidBody�� �̿��� ������ ���� ����
        Vector3 moveDistance = (transform.forward * input.moveZ) + (transform.right * input.moveX);
        transform.Translate(moveDistance.normalized * moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(rb.position + moveDistance * moveSpeed * Time.fixedDeltaTime);
    }
    private void Rotate()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            Vector3 directionToLook = hit.point - rb.position;
            directionToLook.y = 0;
            if (directionToLook != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToLook, transform.up);
                rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotSpeed * Time.fixedDeltaTime);
            }
        }
        // RigidBody�� �̿��� ȸ�� ���� ����
        //float turn = input.mouseRotate * rotSpeed * Time.fixedDeltaTime;
        //rb.rotation = rb.rotation * Quaternion.Euler(0f, turn, 0f);
        /*Quaternion.LookRotation(�ٶ� ����) : ���ϴ� "����"���� ȸ��
          Quaternion.Euler(x, y, z) : X, Y, Z ���� �������� ȸ��*/
    }
}
