using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
// 1. 컴퍼넌트선언 - 애니메이터, RigidBody, Input스크립트
public class WomanMovement : MonoBehaviourPun
{
    [SerializeField] private WomanInput input;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator ani;
    private readonly int hashMoveX = Animator.StringToHash("MoveX");
    private readonly int hashMoveY = Animator.StringToHash("MoveY");
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
        if (!photonView.IsMine) return;

        Move();
        Rotate();
    }
    private void Move()
    {
        // RigidBody를 이용한 움직임 제어 로직
        //Vector3 moveDistance = (transform.forward * input.moveZ) + (transform.right * input.moveX);
        //transform.Translate(moveDistance.normalized * moveSpeed * Time.fixedDeltaTime);
        //rb.MovePosition(rb.position + moveDistance * moveSpeed * Time.fixedDeltaTime);

        // input.moveZ와 input.moveX는 플레이어 입력에 따라 -1, 0, 1 값을 가진다.
        Vector3 localMoveDirection = new Vector3(input.moveX, 0f, input.moveZ).normalized;
        // 플레이어의 로컬 방향을 기준으로 이동 벡터를 월드 공간으로 변환
        // transform.TransformDirection은 로컬 벡터를 월드 벡터로 변환
        Vector3 worldMoveDirection = transform.TransformDirection(localMoveDirection);
        // Rigidbody를 사용하여 플레이어를 이동
        rb.MovePosition(rb.position + worldMoveDirection * moveSpeed * Time.fixedDeltaTime);
        ani.SetFloat(hashMoveX, input.moveX, 0.01f, Time.deltaTime);
        ani.SetFloat(hashMoveY, input.moveZ, 0.01f, Time.deltaTime);

        if (input.isRun)
        {
            moveSpeed = 8f;
            ani.SetBool("isRun", true);
        }
        else
        {
            moveSpeed = 5f;
            ani.SetBool("isRun", false);
        }
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
