using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : LifeEntity
{
    private readonly int hashHit = Animator.StringToHash("Hit");
    private readonly int hashTrace = Animator.StringToHash("isTrace");
    private readonly int hashDie = Animator.StringToHash("Die");
    public LayerMask targetLayer;
    private LifeEntity targetEntity;
    public ParticleSystem hitEffect;
    private NavMeshAgent navi;
    private Animator ani;

    [Header("공격관련")]
    public float damage = 15f;
    public float timeBetAttack = 0.5f;
    private float lastAttackTime;

    private bool hasTarget
    {
        get
        {
            if (targetEntity != null && !targetEntity.dead)  // 추적 대상이 존재하고 죽지 않은 경우
            {
                return true;
            }
            return false;
        }
    }

    void Start()
    {
        navi = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();

        StartCoroutine(TraceTarget());
    }

    private void Update()
    {
        ani.SetBool(hashTrace, hasTarget);
    }

    IEnumerator TraceTarget()
    {
        while (!dead)
        {
            if (hasTarget)
            {
                navi.isStopped = false;
                navi.SetDestination(targetEntity.transform.position);
            }
            else
            {
                navi.isStopped = true;
                Collider[] colliders = Physics.OverlapSphere(transform.position, 15f, targetLayer);
                for (int i = 0; i < colliders.Length; i++)
                {
                    LifeEntity lifeEntity = colliders[i].GetComponent<LifeEntity>();
                    if (lifeEntity != null && !lifeEntity.dead)
                    {
                        targetEntity = lifeEntity;
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!dead)
        {
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
            hitEffect.Play();
            ani.SetTrigger(hashHit);
        }
        base.OnDamage(damage, hitPoint, hitNormal);
    }

    public override void Die()
    {
        Collider[] slimeColliders = GetComponents<Collider>();
        for (int i = 0; i < slimeColliders.Length; i++)
        {
            slimeColliders[i].enabled = false;    // 좀비의 모든 콜라이더 비활성화
        }
        navi.isStopped = true;
        navi.enabled = false;
        ani.SetTrigger(hashDie);
        base.Die();
    }
}
