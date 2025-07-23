using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    [SerializeField] private Transform firePos;
    public ParticleSystem muzzleFlash;
    private LineRenderer line;
    private float fireDistance = 100f;
    public float damage = 10f;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.enabled = false;
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 20f, Color.green);
    }

    public void FireBullet()
    {
        RaycastHit hit;
        Vector3 hitPos = Vector3.zero;
        if (Physics.Raycast(firePos.position, firePos.forward, out hit, fireDistance))
        {
            Debug.Log($"{hit.collider.name}");
            IDamage target = hit.collider.GetComponent<IDamage>();
            if (target != null)
            {
                target.OnDamage(damage, hit.point, hit.normal);
            }
            hitPos = hit.point;
        }
        else
        {
            hitPos = firePos.position + firePos.forward * fireDistance;
        }
        StartCoroutine(FireEffect(hitPos));
    }

    IEnumerator FireEffect(Vector3 hitPosition)
    {
        muzzleFlash.Play();
        line.SetPosition(0, firePos.position);
        line.SetPosition(1, hitPosition);
        line.enabled = true;
        yield return new WaitForSeconds(0.2f);
        line.enabled = false;
    }
}
