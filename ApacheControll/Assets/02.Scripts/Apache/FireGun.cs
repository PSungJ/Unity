using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGun : MonoBehaviour
{
    //private AudioSource source;
    public AudioClip fireClip;
    public AudioClip expClip;
    [SerializeField] ApacheAi apacheAi;
    [SerializeField] LineRenderer left_firePos = null;
    [SerializeField] LineRenderer right_firePos = null;
    [SerializeField] private GameObject expEffect;
    public int tankLayer;
    private float maxDelay = 0.3f;
    private float curDelay = 0f;

    void Start()
    {
        //source = GetComponent<AudioSource>();
        fireClip = Resources.Load<AudioClip>("Sounds/ShootMissile");
        expClip = Resources.Load<AudioClip>("Sounds/DestroyedExplosion");
        expEffect = Resources.Load<GameObject>("Effects/BigExplosionEffect");
        apacheAi = GetComponent<ApacheAi>();
        left_firePos = GetComponentsInChildren<LineRenderer>()[0];
        right_firePos = GetComponentsInChildren<LineRenderer>()[1];
        tankLayer = LayerMask.NameToLayer("TANK");
        curDelay = maxDelay;
    }

    void Update()
    {
        switch (apacheAi.state)
        {
            case ApacheAi.ApacheState.ATTACK:
                Fire();
                break;
        }
    }
    void Fire()
    {
        Ray ray1 = new Ray(left_firePos.transform.position, left_firePos.transform.forward);
        Ray ray2 = new Ray(right_firePos.transform.position, right_firePos.transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(ray1, out hit, Mathf.Infinity, 1 << tankLayer) || Physics.Raycast(ray2, out hit, Mathf.Infinity, 1 << tankLayer))
        {
            curDelay -= 0.01f;
            if (curDelay <= 0)
            {
                curDelay = maxDelay;
                left_firePos.GetComponent<LaserBeam>().FireRay();
                right_firePos.GetComponent<LaserBeam>().FireRay();

                SoundManager.S_instance.PlaySfx(transform.position, fireClip, false);
                StartCoroutine(Exposion(hit));
            }
        }
    }

    IEnumerator Exposion(RaycastHit hit)
    {
        Vector3 hitPos = hit.point;
        Vector3 hitNormal = (left_firePos.transform.position - hitPos).normalized;
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, hitNormal);
        var _exp = PoolingManager.p_instance.GetExp();
        _exp.SetActive(true);
        _exp.transform.position = hitPos;
        _exp.transform.rotation = rot;
        yield return new WaitForSeconds(0.3f);
        _exp.SetActive(false);
        
        SoundManager.S_instance.PlaySfx(hitPos, expClip, false);
    }

    //void ShowEffect(RaycastHit hit)
    //{
    //    Vector3 hitPos = hit.point;
    //    Vector3 hitNormal = (left_firePos.transform.position - hitPos).normalized;
    //    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, hitNormal);
        
    //    GameObject eff = Instantiate(expEffect, hitPos, rot);
    //    source.PlayOneShot(expClip);
    //    Destroy(eff, 2f);
    //}

}
