using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private readonly int hashJump = Animator.StringToHash("isJumping");
    private readonly int hashPosX = Animator.StringToHash("PosX");
    private readonly int hashPosY = Animator.StringToHash("PosY");
    private Animator ani;
    private PlayerInputHandler input;
    private Transform tr;
    private float moveSpeed = 5f;
    private float rotSpeed = 20f;

    void Start()
    {
        tr = transform;
        input = GetComponent<PlayerInputHandler>();
        ani = GetComponent<Animator>();
    }

    void Update()
    {
        MoveAndRotate();
    }

    void MoveAndRotate()
    {
        tr.Rotate(Vector3.up * input.MoveRot * Time.deltaTime * rotSpeed);
        tr.Translate(input.moveDir.normalized * moveSpeed * Time.deltaTime);
        ani.SetFloat(hashPosX, input.moveDir.x, 0.01f, Time.deltaTime);
        ani.SetFloat(hashPosY, input.moveDir.z, 0.01f, Time.deltaTime);
        if (input.isJump)
            ani.SetBool(hashJump, true);
        else
            ani.SetBool(hashJump, false);
    }
}
