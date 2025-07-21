using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class WarrirInput : MonoBehaviour
{
    private readonly int hashAttack = Animator.StringToHash("Attack");
    private readonly int hashMove = Animator.StringToHash("Movement");
    public Vector3 moveDir;
    public Vector2 dir;
    private Animator ani;

    // Invoke Csharp Events
    private PlayerInput playerInput;
    private InputActionMap mainActionMap;
    private InputAction moveAction;
    private InputAction attackAction;

    private void Start()
    {
        ani = GetComponent<Animator>();
        
        // Invoke Csharp Events 방식의 초기화
        playerInput = GetComponent<PlayerInput>();              // 컴퍼넌트 초기화
        mainActionMap = playerInput.actions.FindActionMap("PlayerAction");  // ActionMap 추출
        moveAction = mainActionMap.FindAction("Move");
        attackAction = mainActionMap.FindAction("Attack");      // Move, Attack 추출
        // Move Action performed 이벤트 연결
        moveAction.performed += ctx =>
        {
            Vector2 dir = ctx.ReadValue<Vector2>();
            moveDir = new Vector3(dir.x, 0, dir.y);
            ani.SetFloat(hashMove, dir.magnitude);
        };
        // Move Action canceled 이벤트 연결
        moveAction.canceled += ctx =>
        {
            moveDir = Vector3.zero;
            ani.SetFloat(hashMove, 0f);
        };
        // Attack Action performed 이벤트 연결
        attackAction.performed += ctx =>
        {
            ani.SetTrigger(hashAttack);
        };
    }

    #region SendMessage 방식
    void OnMove(InputValue value)
    {
        dir = value.Get<Vector2>();
        // 2차원 좌표를 3차원 좌표로 변환
        moveDir = new Vector3(dir.x, 0, dir.y);
        ani.SetFloat(hashMove, dir.magnitude);
    }

    void OnAttack()
    {
        ani.SetTrigger(hashAttack);
    }
    #endregion

    #region Invoke Unity Events 방식
    public void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 dir = ctx.ReadValue<Vector2>();
        moveDir = new Vector3(dir.x, 0, dir.y);
        ani.SetFloat(hashMove, dir.magnitude);
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Debug.Log("Attack");
            ani.SetTrigger(hashAttack);
        }
    }
    #endregion
}
