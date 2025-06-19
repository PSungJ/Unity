using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class Touch_Pad : MonoBehaviour
{
    [SerializeField]
    [Tooltip("터치패드")] private RectTransform touchPad;   // UI의 좌표
    [SerializeField] private Vector3 StartPos;  // 시작지점 위치
    [SerializeField] private float dragRadius = 80f;    // 조이스틱 UI의 반지름, 추후 수정
    [SerializeField] private PlayerCtrl playerCtrl; // 패드의 x,y 방향값을 플레이어에게 전달하기 위해 필요
    private bool isPressed = false; // 버튼을 눌렀는지 여부확인
    private int touchId = -1;   // 마우스 포인터나 손가락이 조이스틱 원안에 있는지 체크, 밖으로 나가면 -1
     void Start()
    {
        touchPad = GetComponent<RectTransform>();
        playerCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();
        StartPos = touchPad.position;
    }
    public void ButtonDown()
    {
        isPressed = true;
    }
    public void ButtonUp()
    {
        isPressed = false;
        HandleInput(StartPos);
    }
    // 여기서 조이스틱 패드 방향을 플레이어의 FixedUpdate에 맞추어서 전달
    void FixedUpdate()  // 고정 프레임, 정확한 물리량에 따른 것을 구현한다면 FixedUpdate를 사용한다
    {                   // 정확한 시간에 따르는 것을 구현한다면 FixedUpdate로 한다.
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                HandleTouchInput();
                break;
            case RuntimePlatform.IPhonePlayer:
                HandleTouchInput();
                break;
            case RuntimePlatform.WindowsEditor:
                HandleInput(Input.mousePosition);
                break;
        }
    }
    void HandleTouchInput() // 모바일 환경
    {
        int i = 0;
        if (Input.touchCount > 0)
        {
            // 터치했을 때의 위치와 방향을 touches라는 배열 프로퍼티가 저장
            foreach (Touch touch in Input.touches)
            {
                i++;
                Vector2 touchPos = new Vector2(touch.position.x, touch.position.y);
                if (touch.phase == TouchPhase.Began) // 터치가 시작 되었을 때
                {
                    if (touch.position.x <= (StartPos.x + dragRadius))
                        touchId = i;
                }
                // 터치패드가 조이스틱 범위안에서 움직이거나 멈춰있다면
                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    if (touchId == i)
                        HandleInput(touchPos);  // 실제로 패드를 움직이는 메서드
                }
                // 터치가 끝났다면
                if (touch.phase == TouchPhase.Ended)
                {
                    if (touchId == i)
                        touchId = -1;
                }
            }
        }
    }
    void HandleInput(Vector3 input) // windowEditor 환경
    {
        if (isPressed)
        {
            //float differVector = (input - StartPos).magnitude;        // magnitude를 이용해 거리의 크기를 float값으로 가져올 수 있다.
            Vector3 differVector = (input - StartPos);
            // 전체 원 거리의 크기, 원 밖으로 벗어 났다면
            if(differVector.sqrMagnitude > (dragRadius * dragRadius))   // 근사값 전체 크기
            {
                differVector.Normalize();   // 방향을 유지
                touchPad.position = StartPos + differVector * dragRadius;   // 원밖을 못벗어나게
            }
            else
            {
                touchPad.position = input;
            }
        }
        else
        {
            touchPad.position = StartPos;
        }
        // isPressed 로직에 따른 방향을 구해준다
        Vector3 differ = touchPad.position - StartPos;  // 거리
        Vector3 normalDiffer = new Vector3(differ.x / dragRadius, differ.y / dragRadius);   // 거리 / 반지름 = 방향

        if(playerCtrl != null)  // 유효성 검사
        {
            //플레이어에게 방향을 전달
            playerCtrl.OnStickPos(normalDiffer);
        }
    }
}
