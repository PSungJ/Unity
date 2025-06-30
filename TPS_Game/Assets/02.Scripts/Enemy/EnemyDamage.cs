using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent (typeof(CapsuleCollider))]
public class EnemyDamage : MonoBehaviour
{
    [SerializeField] ParticleSystem blood;
    private readonly string bulletTag = "BULLET";
    private float InitHp = 100f;
    private float hp = 100f;
    private EnemyAI enemyAI;
    private void OnEnable()
    {
        hp = 100f;
    }
    void Start()
    {
        enemyAI = GetComponent<EnemyAI>();
        blood.Stop();
    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(bulletTag))
        {
            col.gameObject.SetActive(false);
            blood.Play();
            hp -= col.gameObject.GetComponent<BulletCtrl>().damage;
            enemyAI.hpBarImage.fillAmount = hp / InitHp;
            if (hp <= 0f)
            {
                GetComponent<EnemyAI>().Die();
                
            }
                
        }
    }
    
}
