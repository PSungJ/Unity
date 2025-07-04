using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class WomanInput : MonoBehaviour
{
    public string moveXAxisName = "Horizontal";    // �¿� �̵�
    public string moveZAxisName = "Vertical";        // �յ� �̵�
    public string rotate = "Mouse X";
    public string fireButton = "Fire1";
    public string reloadButton = "Reload";
    
    // ������Ƽ �Է°� ����
    public float moveZ { get; private set; }
    public float moveX { get; private set; }
    public float mouseRotate {  get; private set; }
    public bool fire { get; private set; }
    public bool reload { get; private set; }
 
    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver)
        {
            moveZ = 0f;
            moveX = 0f;
            mouseRotate = 0f;
            fire = false;
            reload = false;
            return;
        }
        moveZ = Input.GetAxis(moveZAxisName);
        moveX = Input.GetAxis(moveXAxisName);
        mouseRotate = Input.GetAxis(rotate);
        fire = Input.GetButton(fireButton);
        reload = Input.GetButton(reloadButton);
    }
}
