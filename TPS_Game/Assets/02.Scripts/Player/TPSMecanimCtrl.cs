using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSMecanimCtrl : MonoBehaviour
{
    public TPSPlayerInput input;
    private Transform tr;
    private Animator anim;
    private Rigidbody rb;
    public float movespeed = 10f;
    private float maxSpeed = 10f;
    private float rotspeed = 20f;
    public bool isRun = false;
    private readonly int hashPosX = Animator.StringToHash("PosX");
    private readonly int hashPosY = Animator.StringToHash("PosY");
    private readonly int hashSprint = Animator.StringToHash("IsSprint");

    //private void OnEnable()
    //{
    //    GameManager.OnItemChange += UpdateSetUp; // 아이템 변경 이벤트 구독
    //}
    //void UpdateSetUp()
    //{
    //    movespeed = GameManager.Instance.gameData.speed; // 게임 매니저에서 속도 가져오기
    //}
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        input = GetComponent<TPSPlayerInput>();
        tr = GetComponent<Transform>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        MoveAndRotate();
        if (input.isSprinting)
        {
            movespeed = 10f;
            isRun = true;
        }
        else
        {
            movespeed = 5f;
            isRun = false;
        }
        anim.SetBool(hashSprint, isRun);
    }
    //private void FixedUpdate() //정확한 물리량에 따른 구현이나 정확한 프레임대로 구현 하고 싶다면
    //{
    //    Vector3 moveDir = (Vector3.forward * input.moveZ) + (Vector3.right * input.moveX);
    //    tr.Translate(moveDir.normalized * movespeed * Time.fixedDeltaTime);
    //    tr.Rotate(Vector3.up * input.mouseRotate * Time.fixedDeltaTime * rotspeed);
    //    #region --- 리지디바디 이동  리지디바디  회전 로직 (New Rotation Logic) ---
    //    // rb.MovePosition(rb.position +  moveDir * movespeed * Time.fixedDeltaTime);
    //    // 1. 이번 FixedUpdate에서 회전할 양(각도)을 계산합니다.
    //    // 물리 업데이트이므로 Time.fixedDeltaTime을 사용합니다.
    //    //float turn = input.mouseRotate * rotspeed * Time.fixedDeltaTime;

    //    // 2. 이 회전량을 Quaternion 형태로 만듭니다. (Y축 기준 회전)
    //    //Quaternion deltaRotation = Quaternion.Euler(Vector3.up * turn);

    //    // 3. 현재 회전값(rb.rotation)에 회전량을 곱하여 최종 목표 회전값을 계산합니다.
    //    //Quaternion targetRotation = rb.rotation * deltaRotation;

    //    // 4. Rigidbody를 계산된 목표 회전값으로 물리적으로 안전하게 회전시킵니다.
    //    //rb.MoveRotation(targetRotation);

    //    //if (rb.velocity.magnitude > maxSpeed)
    //    //{
    //    //    // 속도를 최대 속도로 제한
    //    //    rb.velocity = rb.velocity.normalized * maxSpeed;
    //    //}
    //    #endregion
    //}

    void MoveAndRotate()
    {
        tr.Translate(input.moveDir.normalized * movespeed * Time.deltaTime);
        tr.Rotate(Vector3.up * input.MoveRot * Time.deltaTime * rotspeed);
        anim.SetFloat(hashPosX, input.moveDir.x, 0.01f, Time.deltaTime);
        anim.SetFloat(hashPosY, input.moveDir.z, 0.01f, Time.deltaTime);
    }
}
