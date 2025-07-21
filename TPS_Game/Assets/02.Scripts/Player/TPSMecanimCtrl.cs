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
    //    GameManager.OnItemChange += UpdateSetUp; // ������ ���� �̺�Ʈ ����
    //}
    //void UpdateSetUp()
    //{
    //    movespeed = GameManager.Instance.gameData.speed; // ���� �Ŵ������� �ӵ� ��������
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
    //private void FixedUpdate() //��Ȯ�� �������� ���� �����̳� ��Ȯ�� �����Ӵ�� ���� �ϰ� �ʹٸ�
    //{
    //    Vector3 moveDir = (Vector3.forward * input.moveZ) + (Vector3.right * input.moveX);
    //    tr.Translate(moveDir.normalized * movespeed * Time.fixedDeltaTime);
    //    tr.Rotate(Vector3.up * input.mouseRotate * Time.fixedDeltaTime * rotspeed);
    //    #region --- ������ٵ� �̵�  ������ٵ�  ȸ�� ���� (New Rotation Logic) ---
    //    // rb.MovePosition(rb.position +  moveDir * movespeed * Time.fixedDeltaTime);
    //    // 1. �̹� FixedUpdate���� ȸ���� ��(����)�� ����մϴ�.
    //    // ���� ������Ʈ�̹Ƿ� Time.fixedDeltaTime�� ����մϴ�.
    //    //float turn = input.mouseRotate * rotspeed * Time.fixedDeltaTime;

    //    // 2. �� ȸ������ Quaternion ���·� ����ϴ�. (Y�� ���� ȸ��)
    //    //Quaternion deltaRotation = Quaternion.Euler(Vector3.up * turn);

    //    // 3. ���� ȸ����(rb.rotation)�� ȸ������ ���Ͽ� ���� ��ǥ ȸ������ ����մϴ�.
    //    //Quaternion targetRotation = rb.rotation * deltaRotation;

    //    // 4. Rigidbody�� ���� ��ǥ ȸ�������� ���������� �����ϰ� ȸ����ŵ�ϴ�.
    //    //rb.MoveRotation(targetRotation);

    //    //if (rb.velocity.magnitude > maxSpeed)
    //    //{
    //    //    // �ӵ��� �ִ� �ӵ��� ����
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
