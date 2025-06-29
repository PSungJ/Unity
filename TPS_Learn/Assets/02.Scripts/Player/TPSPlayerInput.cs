using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSPlayerInput : MonoBehaviour
{
    public string moveZAxisName = "Vertical";
    public string moveXAxisName = "Horizontal";
    public string fireButtonName = "Fire1";
    public string reloadButtonName = "Reload";
    public string mouseX = "Mouse X";
    
    //�� �Ҵ��� ���ο����� ����
    public float moveZ { get ; private set; } //������ �������� �Է°�
    public float moveX { get; private set; }//�¿� �������� �Է°� �̸�
    public float mouseRotate { get; private set; }
    public bool fire { get; private set; }  
    public bool reload { get; private set; }
    public bool sprint { get; private set; }
    void Start()
    {
        
    }
    void Update()
    {
        if (GameManager.Instance !=null&&GameManager.Instance.isGameOver)
        {
            moveZ = 0; moveX = 0; fire = false; mouseRotate = 0f; sprint =false; reload = false;
            return;
        }
        moveZ = Input.GetAxis(moveZAxisName);
        moveX = Input.GetAxis(moveXAxisName);
        mouseRotate = Input.GetAxis(mouseX);
        fire = Input.GetButton(fireButtonName);
        reload = Input.GetButtonDown(reloadButtonName);
        sprint = Input.GetKey(KeyCode.LeftShift);
    }
}
