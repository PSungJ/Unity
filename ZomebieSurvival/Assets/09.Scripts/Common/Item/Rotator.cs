using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 게임오브젝트를 지속적으로 회전하는 스크립트
public class Rotator : MonoBehaviour
{
    public float rotationSpeed = 60f;

    private void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
