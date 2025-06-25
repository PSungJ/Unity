using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour
{
    [SerializeField]
    [Tooltip("터치패드")] private RectTransform touchPad;
    [SerializeField] private Vector3 StartPos;
    [SerializeField] private float dragRadius = 80f;
    [SerializeField] private Rocket rocket;
    private bool isPressed = false;
    private int touchId = -1;
    void Start()
    {
        touchPad = GetComponent<RectTransform>();
        rocket = GameObject.FindGameObjectWithTag("Player").GetComponent<Rocket>();
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
    void HandleInput(Vector3 input)
    {
        if (isPressed)
        {
            Vector3 differVector = (input - StartPos);
            if (differVector.sqrMagnitude > (dragRadius * dragRadius))
            {
                differVector.Normalize();
                touchPad.position = StartPos + differVector * dragRadius;
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
        Vector3 differ = touchPad.position - StartPos;
        Vector3 normalDiffer = new Vector3(differ.x / dragRadius, differ.y / dragRadius);

        if (rocket != null)
        {
            rocket.OnStickPos(normalDiffer);
        }
    }
    void FixedUpdate()
    {
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
    void HandleTouchInput()
    {
        int i = 0;
        if (Input.touchCount > 0)
        {
            foreach(Touch touch in Input.touches)
            {
                i++;
                Vector2 touchPos = new Vector2(touch.position.x, touch.position.y);
                if (touch.phase == TouchPhase.Began)
                {
                    if (touch.position.x <= (StartPos.x + dragRadius))
                        touchId = i;
                }
                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    if (touchId == i)
                        HandleInput(touchPos);
                }
                if(touch.phase == TouchPhase.Ended)
                {
                    if (touchId == i)
                        touchId = -1;
                }
            }
        }
    }
}
