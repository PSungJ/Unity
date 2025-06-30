using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public float speed = 3000f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private SphereCollider s_col;
    [SerializeField] private Transform tr;
    [SerializeField] private TrailRenderer trail;
    public float damage = 10f;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        s_col = GetComponent<SphereCollider>();
        tr = GetComponent<Transform>();
        trail = GetComponent<TrailRenderer>();
        //Destroy(this.gameObject, 3f);
    }
    private void OnEnable() //총알오브젝트가 활성화 될때 마다 호출
    {
        rb.AddForce(tr.forward * speed);
        Invoke("BulletDisable", 3f);
        GameManager.OnItemChange += UpdateSetUp;
    }
    void UpdateSetUp()
    {
        damage = GameManager.Instance.gameData.damage;
    }
    private void Start()
    {
        damage = GameManager.Instance.gameData.damage;
    }
    void BulletDisable()
    {
        this.gameObject.SetActive(false);
    }
    private void OnDisable() // 비활성화 될때마다 호출
    {
        trail.Clear();
        rb.Sleep();
    }
}
