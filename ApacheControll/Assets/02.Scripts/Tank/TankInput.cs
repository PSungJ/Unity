using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// W,S�� �յ�, A,D�� ȸ��, ���콺 �ٷ� ���� ��������
// Mouse X�� �����̸� �ﰢ�Լ��� �̿��Ͽ� Turret�� Y������ ȸ��
// ��ź��Ʈ �Լ� Atan
public class TankInput : MonoBehaviour
{
    public string hori = "Horizontal";
    public string ver = "Vertical";
    public string mouseWheel = "Mouse ScrollWheel";
    public string fire = "Fire1";
    public float h = 0; // A,D ȸ��
    public float v = 0; // W,S �̵�
    public float m_scrollWheel = 0f;
    public float axisRaw = 0f;
    public bool isFire = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver)
        {
            h = 0f; v = 0f; m_scrollWheel = 0f; isFire = false;
            return;
        }
        h = Input.GetAxis(hori);
        v = Input.GetAxis(ver);
        m_scrollWheel = Input.GetAxisRaw(mouseWheel);
        isFire = Input.GetButtonDown(fire);
        axisRaw = Input.GetAxisRaw(ver);
    }
}
