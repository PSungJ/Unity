using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCtrl : MonoBehaviour
{
    public TPSPlayerInput input;
    private Transform tr;
    private Animation anim;
    public float moveSpeed = 10f;
    public float rotSpeed = 90f;
    void Start()
    {
        anim = GetComponent<Animation>();
        input = GetComponent<TPSPlayerInput>();
        tr = transform;
      
    }
    void Update()
    {
        //Vector3 moveDir =(Vector3.forward * input.moveZ) +(Vector3.right * input.moveX);
        //tr.Translate(moveDir.normalized * Time.deltaTime *moveSpeed);
        //tr.Rotate(Vector3.up * input.mouseRotate *Time.deltaTime *rotSpeed);
        ////애니메이션 구현 
        //if (input.moveX > 0.1f)
        //    anim.CrossFade("RunR", 0.2f);
        //else if (input.moveX < -0.1f)
        //    anim.CrossFade("RunL", 0.2f);
        //else if (input.moveZ > 0.1f)
        //    anim.CrossFade("RunF", 0.2f);
        //else if (input.moveZ < -0.1f)
        //    anim.CrossFade("RunB", 0.2f);
        //else
        //    anim.CrossFade("Idle", 0.2f);

    }
}
