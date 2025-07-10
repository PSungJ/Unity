using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//1.총알 충돌감지 후 총알은 사라짐 이펙트 효과 사운드효과
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
            // 첫번째로 충돌한 지점을 ContactPoint 구조체에 전달 

            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);
            //법선벡터가 이루는 회전 각도 추출 
            var spk =Instantiate(Spark,contact.point,rot);
            Destroy(spk,1f);
            //source.PlayOneShot(hitClip, 1.0f);
            SoundManager.S_instance.PlaySfx(contact.point, hitClip, false);
        }
    }

}
