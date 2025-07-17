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
        // AddRelativeForce는 Unity 게임엔진에서 오브젝트에 상대적인 전방 방향으로 힘을 가하는 것이다.
        // 로컬 좌표계 기준 (오브젝트 자체의 앞, 위, 옆 기준)
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
