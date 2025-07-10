using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//1.�Ѿ� �浹���� �� �Ѿ��� ����� ����Ʈ ȿ�� ����ȿ��
public class RemoveBullet : MonoBehaviour
{
    public GameObject Spark;
    //public AudioSource source;
    public AudioClip hitClip;
    private readonly string bulletTag = "BULLET";
    private readonly string E_bulletTag = "E_BULLET";
    void Start()
    {
        //source = GetComponent<AudioSource>();
        Spark = Resources.Load("Weapon/FlareMobile") as GameObject;
        hitClip = Resources.Load("Sounds/bullet_hit_metal_enemy_4") as AudioClip;
    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == bulletTag || col.collider.tag== E_bulletTag)
        {
           

            //Destroy(col.gameObject);
            col.gameObject.SetActive(false);
            ContactPoint contact = col.contacts[0];
            // ù��°�� �浹�� ������ ContactPoint ����ü�� ���� 

            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);
            //�������Ͱ� �̷�� ȸ�� ���� ���� 
            var spk =Instantiate(Spark,contact.point,rot);
            Destroy(spk,1f);
            //source.PlayOneShot(hitClip, 1.0f);
            SoundManager.S_instance.PlaySfx(contact.point, hitClip, false);
        }
    }

}
