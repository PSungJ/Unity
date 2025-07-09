using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// W,S로 앞뒤, A,D로 회전, 마우스 휠로 포신 상하조작
// Mouse X로 움직이면 삼각함수를 이용하여 Turret이 Y축으로 회전
// 역탄젠트 함수 Atan
public class TankInput : MonoBehaviour
{
    public string hori = "Horizontal";
    public string ver = "Vertical";
    public string mouseWheel = "Mouse ScrollWheel";
    public string fire = "Fire1";
    public float h = 0; // A,D 회전
    public float v = 0; // W,S 이동
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
