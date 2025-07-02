using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Animator ani;
    [SerializeField] private Transform mariaTr;
    [SerializeField] private CharacterController charCon;
    public int playerLayer;

    [SerializeField] private CameraCtrl cameraCtrl; // ī�޶� ��ũ��Ʈ
    [SerializeField] private PlayerState p_State;

    [Header("�ӵ�")]
    [Tooltip("Walking")] public float walkSpeed = 5.0f;
    [Tooltip("Running")] public float runSpeed = 10.0f;

    [Header("���콺 ���� �ʵ�")]
    public Vector3 mouseMove = Vector3.zero;    // ���콺 �̵� ����
    [SerializeField] private Vector3 moveVelocity = Vector3.zero;   // �̵��ӵ� ����

    private bool IsGrounded = false;

    private bool isRun;
    public bool IsRun
    {
        get { return isRun; }
        set
        {
            isRun = value;
            ani.SetBool("isRun", isRun);
        }
    }
    private float nextTime = 0f;

    void Start()
    {
        ani = GetComponentInChildren<Animator>();
        mariaTr = GetComponentInChildren<Transform>();
        charCon = GetComponent<CharacterController>();
        cameraCtrl = GetComponent<CameraCtrl>();
        p_State = GetComponent<PlayerState>();
        playerLayer = LayerMask.NameToLayer("PLAYER");
    }
    public void PlayerIdleAndMove()
    {
        RunCheck(); // �޸��� üũ

        // ĳ���� ��Ʈ�ѷ� üũ
        if (charCon.isGrounded)
        {
            if (IsGrounded == false) IsGrounded = true;
            ani.SetBool("isGrounded", true);

            CalcInputMove();
            RaycastHit groundHit;   // �� üũ�� ���� ����ĳ��Ʈ ����
            if (GroundCheck(out groundHit))
                moveVelocity.y = IsRun ? -runSpeed : -walkSpeed;
            else
                moveVelocity.y = -1f;

            PlayerAttack();
        }
        else
        {
            if (IsGrounded == false) IsGrounded = true;
            else
                ani.SetBool("isGrounded", false);

            // ���� ���� �ʾ��� �� y�� �ӵ� ����
            moveVelocity += Physics.gravity * Time.deltaTime;
        }
        // ĳ���� ��Ʈ�ѷ��� �̵��ӵ�
        charCon.Move(moveVelocity * Time.deltaTime);    // ĳ���� ��Ʈ�ѷ� �̵�

    }
    public void CalcInputMove()
    {
        // �Է¿� ���� �̵����� ���
        moveVelocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized * (IsRun ? runSpeed : walkSpeed);
        ani.SetFloat("speedX", Input.GetAxis("Horizontal"));    // �ִϸ����� X�� �ӵ�
        ani.SetFloat("speedY", Input.GetAxis("Vertical"));      // �ִϸ����� Y�� �ӵ�

        // �̵������� ���ù������� ���� �ʰ� ����������� ����
        moveVelocity = transform.TransformDirection(moveVelocity);  // �÷��̾� �������� �̵����� ��ȯ
                                                                    //transform.TransformPoint(moveVelocity);      // �÷��̾� ��ġ�� �̵����� ����
        if (moveVelocity.sqrMagnitude > 0.01f)  // �̵��ӵ��� 0.01 ���� Ŭ��
        {
            Quaternion cameraRot = cameraCtrl.cameraPivotTr.rotation;  // ī�޶� ȸ�� ��������
            cameraRot.x = cameraRot.z = 0f; // ī�޶� ȸ���� x, z���� 0���� ����
            cameraCtrl.cameraPivotTr.rotation = cameraRot; // �÷��̾� ȸ�� ����
            if (IsRun)  // �޸��� ������ ��
            {
                Quaternion charRot = Quaternion.LookRotation(moveVelocity); // �̵��������� �÷��̾� �� ȸ��
                charRot.x = charRot.z = 0f; // �÷��̾� �� ȸ���� x,z���� 0���� ����
                mariaTr.rotation = Quaternion.Slerp(mariaTr.rotation, charRot, Time.deltaTime * 10f);   // �÷��̾� �� ȸ�� ����
            }
            else
            {
                // �ȱ� ������ �� ī�޶� ȸ������ �÷��̾� �� ȸ��
                mariaTr.rotation = Quaternion.Slerp(mariaTr.rotation, cameraRot, Time.deltaTime * 10f);
            }
        }
    }
    public void PlayerAttack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            p_State.state = PlayerState.State.ATTACK;
            ani.SetTrigger("swordAttack");
            ani.SetFloat("speedX", 0f);
            ani.SetFloat("speedY", 0f);
            nextTime = 0f;  // ���� ���� �ð� �ʱ�ȭ
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            p_State.state = PlayerState.State.SHIELD_ATTACK;
            ani.SetTrigger("shieldAttack");
            ani.SetFloat("speedX", 0f);
            ani.SetFloat("speedY", 0f);
            nextTime = 0f;
        }
    }
    public void AttackTimeState()
    {
        nextTime += Time.deltaTime;
        if (nextTime >= 1f)
            p_State.state = PlayerState.State.IDLE;
    }
    public void RunCheck()
    {
        if (IsRun == false && Input.GetKeyDown(KeyCode.LeftShift))
            IsRun = true;   // �޸��� ����
        else if (IsRun == true && Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
            IsRun = false;  // �ȱ�� ����
    }
    public bool GroundCheck(out RaycastHit hit)  // ���� ��Ҵ��� Ȯ��
    {
        return Physics.Raycast(transform.position, Vector3.down, out hit, 0.2f);
    }
    void Update()
    {
        float cameraHeight = 1f;  // ī�޶� ����, �÷��̾��� ��������
        cameraCtrl.cameraPivotTr.localPosition = transform.position + (Vector3.up * cameraHeight); // ī�޶� �θ� ��ġ ����

        mouseMove += new Vector3(-Input.GetAxisRaw("Mouse Y") * 100f * 0.1f,
                                Input.GetAxisRaw("Mouse X") * 100f * 0.1f, 0f);    // ���콺 �̵� ���� ������Ʈ
        if (mouseMove.x < -40f) // ���콺 �̵� ����
            mouseMove.x = -40f;
        else if (mouseMove.x > 40f)
            mouseMove.x = 40f;

        cameraCtrl.cameraPivotTr.localEulerAngles = mouseMove; // ī�޶� �θ� ȸ�� ����

        // ī�޶� ��ֹ��� �������� �ʵ��� ��ġ����
        RaycastHit hit;
        Vector3 dir = (cameraCtrl.cameraTr.position - cameraCtrl.cameraPivotTr.localPosition).normalized;
        Debug.DrawRay(cameraCtrl.cameraPivotTr.position, dir * 100f, Color.red);   // ��ȭ�鿡�� Ray ��� Ȯ��
        if (Physics.Raycast(cameraCtrl.cameraPivotTr.position, dir, out hit, cameraCtrl.cameraDistance, ~(1 << playerLayer))) // ~(1 << playerLayer) : playerLayer�� ������ ��� ��
            cameraCtrl.cameraTr.localPosition = Vector3.back * hit.distance;   // ��ֹ��� �������� ī�޶� ��ġ�� ��ֹ� �ڷ� �̵�
        else
            cameraCtrl.cameraTr.localPosition = Vector3.back * cameraCtrl.cameraDistance;
    }
}
