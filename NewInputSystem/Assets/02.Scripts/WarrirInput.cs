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
        
        // Invoke Csharp Events ����� �ʱ�ȭ
        playerInput = GetComponent<PlayerInput>();              // ���۳�Ʈ �ʱ�ȭ
        mainActionMap = playerInput.actions.FindActionMap("PlayerAction");  // ActionMap ����
        moveAction = mainActionMap.FindAction("Move");
        attackAction = mainActionMap.FindAction("Attack");      // Move, Attack ����
        // Move Action performed �̺�Ʈ ����
        moveAction.performed += ctx =>
        {
            Vector2 dir = ctx.ReadValue<Vector2>();
            moveDir = new Vector3(dir.x, 0, dir.y);
            ani.SetFloat(hashMove, dir.magnitude);
        };
        // Move Action canceled �̺�Ʈ ����
        moveAction.canceled += ctx =>
        {
            moveDir = Vector3.zero;
            ani.SetFloat(hashMove, 0f);
        };
        // Attack Action performed �̺�Ʈ ����
        attackAction.performed += ctx =>
        {
            ani.SetTrigger(hashAttack);
        };
    }

    #region SendMessage ���
    void OnMove(InputValue value)
    {
        dir = value.Get<Vector2>();
        // 2���� ��ǥ�� 3���� ��ǥ�� ��ȯ
        moveDir = new Vector3(dir.x, 0, dir.y);
        ani.SetFloat(hashMove, dir.magnitude);
    }

    void OnAttack()
    {
        ani.SetTrigger(hashAttack);
    }
    #endregion

    #region Invoke Unity Events ���
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
