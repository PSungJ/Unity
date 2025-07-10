using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonCtrl : MonoBehaviour
{
    private TankInput input;
    private Transform tr;
    public float rotSpeed = 150f;
    [Header("���� ��������")]
    public float upperAngle = -30f; // �ִ� ���� ����
    public float downAngle = 0;
    public float curRotate = 0f;    // ���� ȸ�� ����

    void Start()
    {
        input = GetComponentInParent<TankInput>();
        tr = GetComponent<Transform>();
    }

    void Update()
    {
        float wheel = -input.m_scrollWheel;
        float angle = Time.deltaTime * rotSpeed * wheel;
        if (wheel <= -0.01f)    // ������ �ø� ��
        {
            curRotate += angle;
            if (curRotate > upperAngle)
                tr.Rotate(angle, 0f, 0f);
            else
                curRotate = upperAngle;
        }
        else
        {
            curRotate += angle;
            if (curRotate < downAngle)
                tr.Rotate(angle, 0f, 0f);
            else
                curRotate = downAngle;
        }
       
    }
}
