using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    public Animation ani;
    public Animator anim;
    private readonly string run = "running";
    private readonly string runStop = "runStop";
    private readonly string FireAni = "fire";
    private readonly string FireInput = "Fire1";

    public bool isScope = false;
    public bool isRunning;
    FireBullet firebullet;
    void Start()
    {
        ani = transform.GetChild(0).GetChild(0).GetComponent<Animation>();
        isRunning = false;
        firebullet = GetComponent<FireBullet>();
        isScope = false;
    }

    void Update()
    {
        PlayerRun();
        Fire();
        ZoomScope();
    }

    private void Fire()
    {
        if (firebullet.isReloading) return;

        if (Input.GetButtonDown(FireInput) && !isRunning)
        {
            ani.Play(FireAni);
        }
    }

    private void PlayerRun()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            ani.Play(run);
            isRunning = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            ani.Play(runStop);
            isRunning = false;
        }
    }

    private void ZoomScope()
    {
        if (Input.GetButtonDown("Fire2") && isScope == false)
        {
            ani.Play("ScopeZoom");
            isScope = true;
        }
        else if (Input.GetButtonDown("Fire2") && isScope == true)
        {
            ani.Play("ZoomDown");
            isScope = false;
        }    
    }
}
