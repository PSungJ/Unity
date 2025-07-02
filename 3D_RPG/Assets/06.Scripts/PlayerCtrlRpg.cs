using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrlRpg : MonoBehaviour
{
    public enum PlayerState // 플레이어 상태 열거형 상수
    {
        IDLE, ATTACK, UNDER_ATTACK, HIT, DEAD
    }
    public PlayerState state = PlayerState.IDLE;   // 플레이어 상태 변수 선언 및 초기화
    [Header("속도")]
    [Tooltip("Walking")] public float walkSpeed = 5.0f;
    [Tooltip("Running")] public float runSpeed = 10.0f;

    [Header("카메라 관련 필드")]   // 카메라 설정
    [SerializeField] private Transform cameraTr;
    [SerializeField] private Transform cameraPivotTr;
    [SerializeField] private float cameraDistance = 5.0f;   // 카메라와 플레이어 사이 거리

    [Header("마우스 관련 필드")]
    public Vector3 mouseMove = Vector3.zero;    // 마우스 이동 벡터
    [SerializeField] private Vector3 moveVelocity = Vector3.zero;   // 이동속도 벡터

    [Header("Player 움직임 관련 필드")]
    [SerializeField] private Transform modelTr;
    [SerializeField] private Animator ani;
    [SerializeField] private CharacterController charCon;
    [SerializeField] private int playerLayer;

    private bool IsGrounded = false;    // 플레이어가 땅에 닿아있는지 확인

    private bool isRun;
    public bool IsRun
    {
        get { return isRun; }
        set
        {
            isRun = value;
            ani.SetBool("isRun", isRun /*value*/);    // 애니메이터 달리기 상태 프로퍼티 설정
        }
    }
    private float nextTime = 0f;    // 다음 공격시간
    void Start()
    {
        cameraTr = Camera.main.transform;   // 메인카메라
        cameraPivotTr = cameraTr.parent;    // 메인카메라 부모오브젝트
        playerLayer = LayerMask.NameToLayer("PLAYER");
        modelTr = GetComponentInChildren<Transform>();
        ani = GetComponentInChildren<Animator>();
        charCon = GetComponent<CharacterController>();
        cameraDistance = 5f;
    }
    void Update()
    {
        FreezeXZ(); // 플레이어의 X, Z축 회전 고정

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
    private void LateUpdate()   // 메인카메라 컨트롤
    {
        float cameraHeight = 1.3f;  // 카메라 높이, 플레이어의 가슴높이
        cameraPivotTr.localPosition = transform.position + (Vector3.up * cameraHeight); // 카메라 부모 위치 설정

        mouseMove += new Vector3(-Input.GetAxisRaw("Mouse Y") * 100f * 0.1f,
                                Input.GetAxisRaw("Mouse X") * 100f * 0.1f , 0f);    // 마우스 이동 벡터 업데이트
        if (mouseMove.x < -40f) // 마우스 이동 제한
            mouseMove.x = -40f;
        else if (mouseMove.x > 40f)
            mouseMove.x = 40f;

        cameraPivotTr.localEulerAngles = mouseMove; // 카메라 부모 회전 설정

        // 카메라가 장애물에 가려지지 않도록 위치조정
        RaycastHit hit;
        Vector3 dir = (cameraTr.position - cameraPivotTr.localPosition).normalized;
        Debug.DrawRay(cameraPivotTr.position, dir * 100f, Color.red);   // 씬화면에서 Ray 경로 확인
        if (Physics.Raycast(cameraPivotTr.position, dir, out hit, cameraDistance, ~(1 << playerLayer))) // ~(1 << playerLayer) : playerLayer를 제외한 모든 것
            cameraTr.localPosition = Vector3.back * hit.distance;   // 장애물에 가려지면 카메라 위치를 장애물 뒤로 이동
        else
            cameraTr.localPosition = Vector3.back * cameraDistance;
    }
    void PlayerIdleAndMove()
    {
        RunCheck(); // 달리기 체크

        // 캐릭터 컨트롤러 체크
        if (charCon.isGrounded)
        {
            if (IsGrounded == false) IsGrounded = true;
            ani.SetBool("isGrounded", true);

            CalcInputMove();
            RaycastHit groundHit;   // 땅 체크를 위한 레이캐스트 변수
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
            
            // 땅에 닿지 않았을 때 y축 속도 설정
            moveVelocity += Physics.gravity * Time.deltaTime;
        }
        // 캐릭터 컨트롤러의 이동속도
        charCon.Move(moveVelocity * Time.deltaTime);    // 캐릭터 컨트롤러 이동
        
    }
    void CalcInputMove()
    {
        // 입력에 따른 이동벡터 계산
        moveVelocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized * (IsRun ? runSpeed : walkSpeed);
        ani.SetFloat("speedX", Input.GetAxis("Horizontal"));    // 애니메이터 X축 속도
        ani.SetFloat("speedY", Input.GetAxis("Vertical"));      // 애니메이터 Y축 속도

        // 이동방향을 로컬방향으로 하지 않고 월드방향으로 설정
        moveVelocity = transform.TransformDirection(moveVelocity);  // 플레이어 방향으로 이동벡터 변환
                     //transform.TransformPoint(moveVelocity);      // 플레이어 위치에 이동벡터 적용
        if (moveVelocity.sqrMagnitude > 0.01f)  // 이동속도가 0.01 보다 클때
        {
            Quaternion cameraRot = cameraPivotTr.rotation;  // 카메라 회전 가져오기
            cameraRot.x = cameraRot.z = 0f; // 카메라 회전의 x, z축을 0으로 설정
            cameraPivotTr.rotation = cameraRot; // 플레이어 회전 설정
            if (IsRun)  // 달리기 상태일 때
            {
                Quaternion charRot = Quaternion.LookRotation(moveVelocity); // 이동방향으로 플레이어 모델 회전
                charRot.x = charRot.z = 0f; // 플레이어 모델 회전의 x,z축을 0으로 설정
                modelTr.rotation = Quaternion.Slerp(modelTr.rotation, charRot, Time.deltaTime * 10f);   // 플레이어 모델 회전 적용
            }
            else
            {
                // 걷기 상태일 때 카메라 회전으로 플레이어 모델 회전
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
            nextTime = 0f;  // 다음 공격 시간 초기화
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
            IsRun = true;   // 달리기 상태
        else if (IsRun == true && Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
            IsRun = false;  // 걷기로 변경
    }
    bool GroundCheck(out RaycastHit hit)  // 땅에 닿았는지 확인
    {
        return Physics.Raycast(transform.position, Vector3.down, out hit, 0.2f);
    }
    void CameraDistanceCtrl()
    {
        cameraDistance -= Input.GetAxis("Mouse ScrollWheel") * 2f;   // 마우스 휠로 카메라 거리 조정
        cameraDistance = Mathf.Clamp(cameraDistance, 2f, 7f);
    }
    void FreezeXZ()
    {
        transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
    }
}
