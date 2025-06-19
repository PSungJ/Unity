using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    public Animation ani;
    private readonly string run = "running";
    private readonly string runStop = "runStop";
    private readonly string FireAni = "fire";
    private readonly string FireInput = "Fire1";

    public bool isRunning;
    FireBullet firebullet;
    void Start()
    {
        ani = transform.GetChild(0).GetChild(0).GetComponent<Animation>();
        isRunning = false;
        firebullet = GetComponent<FireBullet>();
    }

    void Update()
    {
        PlayerRun();
        Fire();
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
}
