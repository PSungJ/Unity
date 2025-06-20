using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    [SerializeField] private SphereCollider spCol;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform tr;
    [SerializeField] private TrailRenderer trail;
    private float speed = 2000f;
    public float damage = 10f;
    void Awake()
    {
        spCol = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        trail = GetComponent<TrailRenderer>();
        //Destroy(this.gameObject, 3f);
    }
    void BulletDisable()
    {
        this.gameObject.SetActive(false);
    }
    private void OnEnable() // 총알 오브젝트가 활성화 될때 호출
    {
        rb.AddForce(tr.forward * speed);
        Invoke("BulletDisable", 3f);
    }
    private void OnDisable()
    {
        trail.Clear();
        rb.Sleep();
    }
}
