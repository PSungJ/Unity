using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSMecanimCtrl : MonoBehaviour
{
    public TPSPlayerInput input;
    private Transform tr;
    private Animator anim;
    private Rigidbody rb;
    private float movespeed = 5f;
    private float maxSpeed = 10f;
    private float rotseed = 500f;
    public bool isRun = false;
    private readonly int hashPosX = Animator.StringToHash("PosX");
    private readonly int hashPosY = Animator.StringToHash("PosY");
    private readonly int hashSprint = Animator.StringToHash("IsSprint");
    private void OnEnable()
    {
        GameManager.OnItemChange += UpdateSetUp;
    }
    void UpdateSetUp()
    {
        movespeed = GameManager.Instance.gameData.speed;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        input = GetComponent<TPSPlayerInput>();
        tr = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        movespeed = GameManager.Instance.gameData.speed;
    }

    void Update()
    {
        if (input.sprint)
        {
            movespeed = 10f;
            isRun = true;
        }
        else
        {
            movespeed = 5f;
            isRun = false;
        }
        

        anim.SetFloat(hashPosX, input.moveX, 0.01f, Time.deltaTime);
        anim.SetFloat(hashPosY, input.moveZ, 0.01f, Time.deltaTime);
        anim.SetBool(hashSprint, isRun);
    }
    private void FixedUpdate() //��Ȯ�� �������� ���� �����̳� ��Ȯ�� �����Ӵ�� ���� �ϰ� �ʹٸ�
    {
        Vector3 moveDir = (Vector3.forward * input.moveZ) + (Vector3.right * input.moveX);
         tr.Translate(moveDir.normalized * movespeed * Time.fixedDeltaTime);
         tr.Rotate(Vector3.up * input.mouseRotate * Time.fixedDeltaTime * rotseed);

        #region --- ������ٵ� �̵�  ������ٵ�  ȸ�� ���� (New Rotation Logic) ---
        // rb.MovePosition(rb.position +  moveDir * speed * Time.fixedDeltaTime);
        // 1. �̹� FixedUpdate���� ȸ���� ��(����)�� ����մϴ�.
        // ���� ������Ʈ�̹Ƿ� Time.fixedDeltaTime�� ����մϴ�.
        //float turn = input.mouseRotate * rotseed * Time.fixedDeltaTime;

        // 2. �� ȸ������ Quaternion ���·� ����ϴ�. (Y�� ���� ȸ��)
        //Quaternion deltaRotation = Quaternion.Euler(Vector3.up * turn);

        // 3. ���� ȸ����(rb.rotation)�� ȸ������ ���Ͽ� ���� ��ǥ ȸ������ ����մϴ�.
        //Quaternion targetRotation = rb.rotation * deltaRotation;

        // 4. Rigidbody�� ���� ��ǥ ȸ�������� ���������� �����ϰ� ȸ����ŵ�ϴ�.
        //rb.MoveRotation(targetRotation);

        //if (rb.velocity.magnitude > maxSpeed)
        //{
        //    // �ӵ��� �ִ� �ӵ��� ����
        //    rb.velocity = rb.velocity.normalized * maxSpeed;
        //}
        #endregion
    }
}
