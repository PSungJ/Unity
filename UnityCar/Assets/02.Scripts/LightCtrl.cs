using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class LightCtrl : MonoBehaviour
{
    [SerializeField] private Light[] frontLight;
    [SerializeField] private Light[] backLight;
    private PlayerCar PlayerCar;
    private bool LightOn;
    void Start()
    {
        PlayerCar = GetComponent<PlayerCar>();
        frontLightOff();
        backLigthOff();
        LightOn = Input.GetKeyDown(KeyCode.F);
    }

    void FixedUpdate()
    {
        if (LightOn = !LightOn)
        {
            for (int i = 0; i < frontLight.Length; i++)
            {
                frontLight[i].enabled = true ? true : false;
            }
        }

        if (PlayerCar.curSpeed > 0f)
        {
            backLightOn();
            if (PlayerCar.MotorInput < 0f)
            {
                backLight[0].color = Color.white;
                backLight[1].color = Color.white;
            }
            else if (PlayerCar.MotorInput > 0f)
            {
                backLight[0].color = Color.green;
                backLight[1].color = Color.green;
            }
            else if (PlayerCar.isEmsBraking)
            {
                backLight[0].color = Color.red;
                backLight[1].color = Color.red;
            }
            else
                backLigthOff();
        }
        
    }
    void frontLightOff()
    {
        for (int i = 0; i < frontLight.Length; i++)
        {
            frontLight[i].enabled = false;
        }
    }
    void backLightOn()
    {
        for (int i = 0; i < backLight.Length; i++)
        {
            backLight[i].enabled = true;
        }
    }
    void backLigthOff()
    {
        for (int i = 0; i < backLight.Length; i++)
        {
            backLight[i].enabled = false;
        }
    }
}
