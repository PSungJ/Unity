#pragma warning disable IDE0051 // 사용되지 않은 멤버(변수,메서드,속성)에 대한 경고를 무시하는 전처리매크로
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WarriorCtrl : MonoBehaviour
{
    private Animator ani;
    private new Transform transform;
    private WarrirInput input;

    private void Start()
    {
        transform = GetComponent<Transform>();
        input = GetComponent<WarrirInput>();
    }

    private void Update()
    {
        if (input.moveDir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(input.moveDir);
            transform.Translate(Vector3.forward * Time.deltaTime * 4.0f);
        }
    }
}
