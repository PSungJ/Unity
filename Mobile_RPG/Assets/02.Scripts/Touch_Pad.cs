using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class Touch_Pad : MonoBehaviour
{
    [SerializeField]
    [Tooltip("��ġ�е�")] private RectTransform touchPad;   // UI�� ��ǥ
    [SerializeField] private Vector3 StartPos;  // �������� ��ġ
    [SerializeField] private float dragRadius = 80f;    // ���̽�ƽ UI�� ������, ���� ����
    [SerializeField] private PlayerCtrl playerCtrl; // �е��� x,y ���Ⱚ�� �÷��̾�� �����ϱ� ���� �ʿ�
    private bool isPressed = false; // ��ư�� �������� ����Ȯ��
    private int touchId = -1;   // ���콺 �����ͳ� �հ����� ���̽�ƽ ���ȿ� �ִ��� üũ, ������ ������ -1
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
    // ���⼭ ���̽�ƽ �е� ������ �÷��̾��� FixedUpdate�� ���߾ ����
    void FixedUpdate()  // ���� ������, ��Ȯ�� �������� ���� ���� �����Ѵٸ� FixedUpdate�� ����Ѵ�
    {                   // ��Ȯ�� �ð��� ������ ���� �����Ѵٸ� FixedUpdate�� �Ѵ�.
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
    void HandleTouchInput() // ����� ȯ��
    {
        int i = 0;
        if (Input.touchCount > 0)
        {
            // ��ġ���� ���� ��ġ�� ������ touches��� �迭 ������Ƽ�� ����
            foreach (Touch touch in Input.touches)
            {
                i++;
                Vector2 touchPos = new Vector2(touch.position.x, touch.position.y);
                if (touch.phase == TouchPhase.Began) // ��ġ�� ���� �Ǿ��� ��
                {
                    if (touch.position.x <= (StartPos.x + dragRadius))
                        touchId = i;
                }
                // ��ġ�е尡 ���̽�ƽ �����ȿ��� �����̰ų� �����ִٸ�
                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    if (touchId == i)
                        HandleInput(touchPos);  // ������ �е带 �����̴� �޼���
                }
                // ��ġ�� �����ٸ�
                if (touch.phase == TouchPhase.Ended)
                {
                    if (touchId == i)
                        touchId = -1;
                }
            }
        }
    }
    void HandleInput(Vector3 input) // windowEditor ȯ��
    {
        if (isPressed)
        {
            //float differVector = (input - StartPos).magnitude;        // magnitude�� �̿��� �Ÿ��� ũ�⸦ float������ ������ �� �ִ�.
            Vector3 differVector = (input - StartPos);
            // ��ü �� �Ÿ��� ũ��, �� ������ ���� ���ٸ�
            if(differVector.sqrMagnitude > (dragRadius * dragRadius))   // �ٻ簪 ��ü ũ��
            {
                differVector.Normalize();   // ������ ����
                touchPad.position = StartPos + differVector * dragRadius;   // ������ �������
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
        // isPressed ������ ���� ������ �����ش�
        Vector3 differ = touchPad.position - StartPos;  // �Ÿ�
        Vector3 normalDiffer = new Vector3(differ.x / dragRadius, differ.y / dragRadius);   // �Ÿ� / ������ = ����

        if(playerCtrl != null)  // ��ȿ�� �˻�
        {
            //�÷��̾�� ������ ����
            playerCtrl.OnStickPos(normalDiffer);
        }
    }
}
