using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 1. 컴퍼넌트선언 - 애니메이터, RigidBody, Input스크립트
public class WomanMovement : MonoBehaviour
{
    [SerializeField] private WomanInput input;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator ani;
    private int groundLayer;

    [Header("움직임 값")]
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

    void FixedUpdate()  // 물리적인 갱신 주기마다 애니메이션 호출
    {
        Move();
        Rotate();
    }
    private void Move()
    {
        // RigidBody를 이용한 움직임 제어 로직
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
        // RigidBody를 이용한 회전 제어 로직
        //float turn = input.mouseRotate * rotSpeed * Time.fixedDeltaTime;
        //rb.rotation = rb.rotation * Quaternion.Euler(0f, turn, 0f);
        /*Quaternion.LookRotation(바라볼 방향) : 원하는 "방향"으로 회전
          Quaternion.Euler(x, y, z) : X, Y, Z 축을 기준으로 회전*/
    }
}
