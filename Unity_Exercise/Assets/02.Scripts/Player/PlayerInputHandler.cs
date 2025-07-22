using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public float MoveRot { get; private set; } = 0f;
    public Vector3 moveDir { get; private set; } = Vector3.zero;
    public bool isJump { get; private set; } = false;

    public void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 dir = ctx.ReadValue<Vector2>();
        moveDir = new Vector3(dir.x, 0, dir.y);
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            isJump = true;
        }
        if (ctx.canceled)
        {
            isJump = false;
        }
    }

    public void OnLook(InputAction.CallbackContext ctx)
    {
        float rot = ctx.ReadValue<float>();
        MoveRot = rot;
    }
}
