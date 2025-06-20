using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private GameObject hitEffect;
    private readonly string bulletTag = "BULLET";
    private readonly string E_bulletTag = "E_BULLET";
    void Start()
    {
        source = GetComponent<AudioSource>();
        hitEffect = Resources.Load("Weapon/FlareMobile") as GameObject;
        hitSound = Resources.Load("Sounds/bullet_hit_metal_enemy_4") as AudioClip;
    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == bulletTag || col.gameObject.tag == E_bulletTag)
        {
            col.gameObject.SetActive(false);
            //Destroy(col.gameObject);
            ContactPoint contact = col.contacts[0]; // �Ѿ��� ó�� ���� ���� ContactPoint�� ����
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);   // ���� ���Ͱ� �̷�� ȸ������ ����
            var spk = Instantiate(hitEffect, contact.point, rot);
            Destroy(spk, 0.5f);
            source.PlayOneShot(hitSound, 1f);
        }
    }
}
