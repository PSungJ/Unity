using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public float Speed = 1500f;
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private Rigidbody rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        trail = GetComponent<TrailRenderer>();
    }
    void BulletDisable()
    {
        this.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        rb.AddForce(transform.forward * Speed);
        Invoke("BulletDisable", 2f);
    }
    private void OnDisable()
    {
        trail.Clear();
        rb.Sleep();
    }
}
