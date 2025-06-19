using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChange : MonoBehaviour
{
    public SkinnedMeshRenderer Spas12;
    public MeshRenderer[] Ak47;
    public MeshRenderer[] M4A1;
    public Animation ani;
    private readonly string weaponAni = "draw";
    public bool isHaveAR = false;
    void Start()
    {
        ani = this.transform.GetChild(0).GetChild(0).GetComponent<Animation>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            WeaponAni();
            Weapon1();
            isHaveAR = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            WeaponAni();
            Weapon2();
            isHaveAR = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            WeaponAni();
            Weapon3();
            isHaveAR = false;
        }
    }

    private void Weapon3()
    {
        for (int i = 0; i < Ak47.Length; i++)
        {
            Ak47[i].enabled = false;
        }
        for (int i = 0; i < M4A1.Length; i++)
        {
            M4A1[i].enabled = false;
        }
        Spas12.enabled = true;
    }

    private void Weapon2()
    {
        for (int i = 0; i < Ak47.Length; i++)
        {
            Ak47[i].enabled = false;
        }
        for (int i = 0; i < M4A1.Length; i++)
        {
            M4A1[i].enabled = true;
        }
        Spas12.enabled = false;
    }

    private void Weapon1()
    {
        for (int i = 0; i < Ak47.Length; i++)
        {
            Ak47[i].enabled = true;
        }
        for (int i = 0; i < M4A1.Length; i++)
        {
            M4A1[i].enabled = false;
        }
        Spas12.enabled = false;
    }

    void WeaponAni()
    {
        ani.Play(weaponAni);
    }
}
