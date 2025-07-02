using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrlRpg : MonoBehaviour
{
    public enum PlayerState // �÷��̾� ���� ������ ���
    {
        IDLE, ATTACK, UNDER_ATTACK, HIT, DEAD
    }
    public PlayerState state = PlayerState.IDLE;   // �÷��̾� ���� ���� ���� �� �ʱ�ȭ
    [Header("�ӵ�")]
    [Tooltip("Walking")] public float walkSpeed = 5.0f;
    [Tooltip("Running")] public float runSpeed = 10.0f;

    [Header("ī�޶� ���� �ʵ�")]   // ī�޶� ����
    [SerializeField] private Transform cameraTr;
    [SerializeField] private Transform cameraPivotTr;
    [SerializeField] private float cameraDistance = 5.0f;   // ī�޶�� �÷��̾� ���� �Ÿ�

    [Header("���콺 ���� �ʵ�")]
    public Vector3 mouseMove = Vector3.zero;    // ���콺 �̵� ����
    [SerializeField] private Vector3 moveVelocity = Vector3.zero;   // �̵��ӵ� ����

    [Header("Player ������ ���� �ʵ�")]
    [SerializeField] private Transform modelTr;
    [SerializeField] private Animator ani;
    [SerializeField] private CharacterController charCon;
    [SerializeField] private int playerLayer;

    private bool IsGrounded = false;    // �÷��̾ ���� ����ִ��� Ȯ��

    private bool isRun;
    public bool IsRun
    {
        get { return isRun; }
        set
        {
            isRun = value;
            ani.SetBool("isRun", isRun /*value*/);    // �ִϸ����� �޸��� ���� ������Ƽ ����
        }
    }
    private float nextTime = 0f;    // ���� ���ݽð�
    void Start()
    {
        cameraTr = Camera.main.transform;   // ����ī�޶�
        cameraPivotTr = cameraTr.parent;    // ����ī�޶� �θ������Ʈ
        playerLayer = LayerMask.NameToLayer("PLAYER");
        modelTr = GetComponentInChildren<Transform>();
        ani = GetComponentInChildren<Animator>();
        charCon = GetComponent<CharacterController>();
        cameraDistance = 5f;
    }
    void Update()
    {
        FreezeXZ(); // �÷��̾��� X, Z�� ȸ�� ����

        switch (state)
        {
            case PlayerState.IDLE:
                PlayerIdleAndMove();
                break;
            case PlayerState.ATTACK:
                AttackTimeState();
                break;
            case PlayerState.UNDER_ATTACK:
                AttackTimeState();
                break;
            case PlayerState.HIT:

                break;
            case PlayerState.DEAD:

                break;
        }
        CameraDistanceCtrl();
    }
    private void LateUpdate()   // ����ī�޶� ��Ʈ��
    {
        float cameraHeight = 1.3f;  // ī�޶� ����, �÷��̾��� ��������
        cameraPivotTr.localPosition = transform.position + (Vector3.up * cameraHeight); // ī�޶� �θ� ��ġ ����

        mouseMove += new Vector3(-Input.GetAxisRaw("Mouse Y") * 100f * 0.1f,
                                Input.GetAxisRaw("Mouse X") * 100f * 0.1f , 0f);    // ���콺 �̵� ���� ������Ʈ
        if (mouseMove.x < -40f) // ���콺 �̵� ����
            mouseMove.x = -40f;
        else if (mouseMove.x > 40f)
            mouseMove.x = 40f;

        cameraPivotTr.localEulerAngles = mouseMove; // ī�޶� �θ� ȸ�� ����

        // ī�޶� ��ֹ��� �������� �ʵ��� ��ġ����
        RaycastHit hit;
        Vector3 dir = (cameraTr.position - cameraPivotTr.localPosition).normalized;
        Debug.DrawRay(cameraPivotTr.position, dir * 100f, Color.red);   // ��ȭ�鿡�� Ray ��� Ȯ��
        if (Physics.Raycast(cameraPivotTr.position, dir, out hit, cameraDistance, ~(1 << playerLayer))) // ~(1 << playerLayer) : playerLayer�� ������ ��� ��
            cameraTr.localPosition = Vector3.back * hit.distance;   // ��ֹ��� �������� ī�޶� ��ġ�� ��ֹ� �ڷ� �̵�
        else
            cameraTr.localPosition = Vector3.back * cameraDistance;
    }
    void PlayerIdleAndMove()
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
    void CalcInputMove()
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
            Quaternion cameraRot = cameraPivotTr.rotation;  // ī�޶� ȸ�� ��������
            cameraRot.x = cameraRot.z = 0f; // ī�޶� ȸ���� x, z���� 0���� ����
            cameraPivotTr.rotation = cameraRot; // �÷��̾� ȸ�� ����
            if (IsRun)  // �޸��� ������ ��
            {
                Quaternion charRot = Quaternion.LookRotation(moveVelocity); // �̵��������� �÷��̾� �� ȸ��
                charRot.x = charRot.z = 0f; // �÷��̾� �� ȸ���� x,z���� 0���� ����
                modelTr.rotation = Quaternion.Slerp(modelTr.rotation, charRot, Time.deltaTime * 10f);   // �÷��̾� �� ȸ�� ����
            }
            else
            {
                // �ȱ� ������ �� ī�޶� ȸ������ �÷��̾� �� ȸ��
                modelTr.rotation = Quaternion.Slerp(modelTr.rotation, cameraRot, Time.deltaTime * 10f);
            }
        }
    }
    void PlayerAttack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            state = PlayerState.ATTACK;
            ani.SetTrigger("swordAttack");
            ani.SetFloat("speedX", 0f);
            ani.SetFloat("speedY", 0f);
            nextTime = 0f;  // ���� ���� �ð� �ʱ�ȭ
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            state = PlayerState.UNDER_ATTACK;
            ani.SetTrigger("shieldAttack");
            ani.SetFloat("speedX", 0f);
            ani.SetFloat("speedY", 0f);
            nextTime = 0f;
        }
    }
    void AttackTimeState()
    {
        nextTime += Time.deltaTime;
        if (nextTime >= 1f)
            state = PlayerState.IDLE;
    }
    void RunCheck()
    {
        if (IsRun == false && Input.GetKeyDown(KeyCode.LeftShift))
            IsRun = true;   // �޸��� ����
        else if (IsRun == true && Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
            IsRun = false;  // �ȱ�� ����
    }
    bool GroundCheck(out RaycastHit hit)  // ���� ��Ҵ��� Ȯ��
    {
        return Physics.Raycast(transform.position, Vector3.down, out hit, 0.2f);
    }
    void CameraDistanceCtrl()
    {
        cameraDistance -= Input.GetAxis("Mouse ScrollWheel") * 2f;   // ���콺 �ٷ� ī�޶� �Ÿ� ����
        cameraDistance = Mathf.Clamp(cameraDistance, 2f, 7f);
    }
    void FreezeXZ()
    {
        transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
    }
}
