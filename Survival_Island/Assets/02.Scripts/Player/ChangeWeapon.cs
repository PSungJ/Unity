using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWeapon : MonoBehaviour
{
    private SkinnedMeshRenderer Spas12;
    private MeshRenderer[] Ak47;
    private MeshRenderer[] M4A1;
    private Animation ani;
    private readonly string weaponAni = "draw";
    public bool isRifle = false;
    void Start()
    {
        Spas12 = this.transform.GetChild(0).GetChild(0).GetChild(3).GetComponent<SkinnedMeshRenderer>();
        Ak47 = GameObject.Find("Ak-47").GetComponentsInChildren<MeshRenderer>();
        M4A1 = GameObject.Find("M4A1 Sopmod").GetComponentsInChildren<MeshRenderer>();
        ani = this.transform.GetChild(0).GetChild(0).GetComponent<Animation>();
    }
    void WeaponChangeAni()
    {
        ani.Play(weaponAni);
    }
    void ShotGun()
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
    void Ak47Rifle()
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
    void M4Rifle()
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
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            WeaponChangeAni();
            ShotGun();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            WeaponChangeAni();
            Ak47Rifle();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            WeaponChangeAni();
            M4Rifle();
        }
    }
}
