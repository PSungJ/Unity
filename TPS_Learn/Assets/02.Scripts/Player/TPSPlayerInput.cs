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
    
    //값 할당은 내부에서나 가능
    public float moveZ { get ; private set; } //감지된 움직임의 입력값
    public float moveX { get; private set; }//좌우 움직임의 입력값 이름
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
