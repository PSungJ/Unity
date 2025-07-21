using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TPSPlayerInput : MonoBehaviour
{
    // �ٸ� ��ũ��Ʈ���� �÷��̾��� �Է� ���¸� ���� �� �ֵ��� ������Ƽ�� ����
    public float MoveRot { get; private set; } = 0f;
    public Vector3 moveDir { get; private set; } = Vector3.zero;
    public bool isFiring { get; private set; } = false;
    public bool isSprinting { get; private set; } = false;

    public event Action OnFireStarted;
    public event Action OnFireCanceled;
    #region Legacy Input
    //public string moveZAxisName = "Vertical";
    //public string moveXAxisName = "Horizontal";
    //public string fireButtonName = "Fire1";
    //public string reloadButtonName = "Reload";
    //public string mouseX = "Mouse X";

    //�� �Ҵ��� ���ο����� ����
    //public float moveZ { get; private set; } //������ �������� �Է°�
    //public float moveX { get; private set; }//�¿� �������� �Է°� �̸�
    //public float mouseRotate { get; private set; }
    //public bool fire { get; private set; }
    //public bool reload { get; private set; }
    //public bool sprint { get; private set; }
    //void Start()
    //{

    //}
    //void Update()
    //{
    //    if (GameManager.Instance != null && GameManager.Instance.isGameOver)
    //    {
    //        moveZ = 0; moveX = 0; fire = false; reload = false; mouseRotate = 0f; sprint = false;
    //        return;
    //    }
    //    moveZ = Input.GetAxis(moveZAxisName);
    //    moveX = Input.GetAxis(moveXAxisName);
    //    mouseRotate = Input.GetAxis(mouseX);
    //    fire = Input.GetButton(fireButtonName);
    //    reload = Input.GetButtonDown(reloadButtonName);
    //    sprint = Input.GetKey(KeyCode.LeftShift);
    //}
    #endregion

    // Behavior�� Invoke Unity Events�� �Ѵ�.
    public void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 dir = ctx.ReadValue<Vector2>();
        moveDir = new Vector3(dir.x, 0, dir.y);
    }

    public void OnLook(InputAction.CallbackContext ctx)
    {
        float rot = ctx.ReadValue<float>();
        MoveRot = rot;
    }

    public void OnFire(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            isFiring = true;
            OnFireStarted?.Invoke();
        }
        if (ctx.canceled)
        {
            isFiring = false;
            OnFireCanceled?.Invoke();
        }
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            isSprinting = true;
        }
        if (ctx.canceled)
        {
            isSprinting = false;
        }
    }
}
