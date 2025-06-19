using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//자기자신 스스로 z축으로 빨리 이동 한다. 
public class BulletCtrl : MonoBehaviour
{
    public float Speed = 1500f;
    private Rigidbody rb;
    public float zomBieDamage = 20f;
    public float skeletonDamage = 10f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * Speed);
        Destroy(this.gameObject, 3f); //3초 후에 총알 오브젝트를 파괴
    }
    
}
