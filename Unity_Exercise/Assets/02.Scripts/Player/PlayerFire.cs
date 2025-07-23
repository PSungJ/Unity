using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    private readonly int hashFire = Animator.StringToHash("Fire");

    private Animator ani;
    private PlayerInputHandler input;
    [SerializeField] private Pistol gun;
    private float lastFire;
    private float fireRate = 0.1f;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        ani = GetComponent<Animator>();
        input = GetComponent<PlayerInputHandler>();
        lastFire = 0f;
    }

    void Update()
    {
        if (input.isFiring && Time.time >= lastFire + fireRate)
        {
            lastFire = Time.time;
            Fire();
        }
    }

    void Fire()
    {
        gun.FireBullet();
        ani.SetTrigger(hashFire);
        Debug.Log("น฿ป็");
    }
}
