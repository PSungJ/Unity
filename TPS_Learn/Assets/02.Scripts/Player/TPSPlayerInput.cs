using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSPlayerInput : MonoBehaviour
{
    private string moveZAxisName = "Vertical";
    private string moveXAxisName = "Horizontal";
    private string fireButtonName = "Fire1";
    //private string reloadButtonName = "Reload";
    private string mouseX = "Mouse X";
    // 값 할당은 내부에서 가능
    public float moveZ { get; private set; }     // 감지된 움직임의 입력값
    public float moveX { get; private set; }
    public float mouseRotate { get; private set; }
    public bool fire { get; private set; }
    public bool movefire { get; private set; }
    public bool reload { get; private set; }
    public bool sprint { get; private set; }
    void Start()
    {
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver)
        {
            moveZ = 0; moveX = 0; fire = false; reload = false; mouseRotate = 0; sprint = false;
            return;
        }
        moveZ = Input.GetAxis(moveZAxisName);
        moveX = Input.GetAxis(moveXAxisName);
        mouseRotate = Input.GetAxis(mouseX);
        fire = Input.GetButton(fireButtonName) && sprint == false;
        //reload = Input.GetButton(reloadButtonName);
        sprint = Input.GetKey(KeyCode.LeftShift) && moveZ >= 0.1f;
    }
}
