using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���ӿ�����Ʈ�� ���������� ȸ���ϴ� ��ũ��Ʈ
public class Rotator : MonoBehaviour
{
    public float rotationSpeed = 60f;

    private void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
