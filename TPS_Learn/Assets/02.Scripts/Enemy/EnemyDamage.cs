using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private ParticleSystem blood;
    [SerializeField] private EnemyAi EnemyAi;
    private float hp;
    private float maxHp = 100f;

    private readonly string bulletTag = "BULLET";
    void Start()
    {
        hp = maxHp;
        EnemyAi = GetComponent<EnemyAi>();
        blood.Stop();
    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(bulletTag))
        {
            col.gameObject.SetActive(false);
            blood.Play();
            hp -= col.gameObject.GetComponent<BulletCtrl>().damage;
            //Mathf.Clamp(maxHp, 0, 100);

            if (hp <= 0)
            {
                EnemyAi.Die();
            }
        }
    }
}
