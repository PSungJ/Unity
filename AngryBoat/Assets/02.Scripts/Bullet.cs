using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject hitEffect;
    public int actorNumber;

    void Start()
    {
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 3000f);
        // AddRelativeForce�� Unity ���ӿ������� ������Ʈ�� ������� ���� �������� ���� ���ϴ� ���̴�.
        // ���� ��ǥ�� ���� (������Ʈ ��ü�� ��, ��, �� ����)
        Destroy(this.gameObject, 1.0f);
    }

    private void OnCollisionEnter(Collision col)
    {
        var contact = col.GetContact(0);
        var obj = Instantiate(hitEffect, contact.point, Quaternion.LookRotation(contact.normal));
        Destroy(obj, 2.0f);
        Destroy(this.gameObject, 0.25f);
    }
}
