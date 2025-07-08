using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class SniperRifleZoom : MonoBehaviour
{
    public Animator sniperAni;
    public Animator gunAni;
    public FirstPersonController firstPersonController;
    float T = 0f;
    float reloadTime = 1f;

    void Start()
    {
        
    }

    void Update()
    {
        T += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Mouse0) && T >= reloadTime)
        {
            T = 0f;
            Shoot();
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            sniperAni.SetBool("isZoomed", true);
            firstPersonController.m_MouseLook.XSensitivity = 0.3f;  // 마우스 x 이동시 감도
            firstPersonController.m_MouseLook.YSensitivity = 0.3f;  // 마우스 y 이동시 감도
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            sniperAni.speed = 1.0f;
            sniperAni.SetBool("isZoomed", false);
            firstPersonController.m_MouseLook.XSensitivity = 2f;  // 마우스 x 이동시 감도
            firstPersonController.m_MouseLook.YSensitivity = 2f;  // 마우스 y 이동시 감도
        }
    }

    private void Shoot()
    {
        gunAni.SetTrigger("Shoot");
    }
}
