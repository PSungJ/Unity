using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 왼쪽 쉬프트 키와 w키를 동시에 눌렀을 때  총을 접는 애니메이션을 재생하는 스크립트 구현
//왼쪽 쉬프트 키와 w키 둘중에 하나라도 떼면 애니메이션이 멈추고 총을 겨누는 애니메이션이 재생되도록 구현
public class PlayerHandAnimation : MonoBehaviour
{
    public Animation anim;
    private readonly string runAni = "running";
    private readonly string runStopAni = "runStop";
    private readonly string FireAni = "fire";
    private readonly string FireInput = "Fire1";
    public bool isRunning;
    FireBullet fireBullet;
    void Start()
    {    
        //자기자신의 첫번째 자식 오브젝트를 찾고 그 자식의  오브젝트의 첫번째 자식 오브젝트를 찾는다
        anim = transform.GetChild(0).GetChild(0).GetComponent<Animation>();
        isRunning = false;
        fireBullet = GetComponent<FireBullet>();
    }


    void Update()
    {
        PlayerRunAni();
        PlayerFire();

    }

    public void PlayerFire()
    {
        if (fireBullet.isReloading) return;

        if (Input.GetButtonDown(FireInput) && !isRunning)
        {
            anim.Play(FireAni);
        }
    }

    private void PlayerRunAni()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            anim.Play(runAni);
            isRunning = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            anim.Play(runStopAni);
            isRunning = false;
        }
    }
}
