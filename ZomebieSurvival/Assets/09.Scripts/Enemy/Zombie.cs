using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
// 1. 필요한 컴퍼넌트 : NavMeshAgent, Audio(source,clip), Collider, 
// BloodEffect, 랜더러
public class Zombie : LivingEntity
{
    public LayerMask targetLayer;       // 추적 대상 레이어
    private LivingEntity targetEntity;  // 추적할 대상 LivingEntity

    public ParticleSystem hitEffect;
    private NavMeshAgent agent;
    private AudioSource source;
    public AudioClip hitClip;
    public AudioClip deathClip;
    private MeshRenderer meshRenderer;
    private Animator ani;

    public float damage = 20f;
    public float timeBetAttack = 0.5f;
    private float lastAttackTime;
    private readonly int hashTarget = Animator.StringToHash("HasTarget");
    private readonly int hashDie = Animator.StringToHash("Die");
    private WaitForSeconds traceWS = new WaitForSeconds(0.25f);

    // 추적 대상이 있는지 알려주는 프로퍼티
    private bool hasTarget
    {
        get
        {
            if(targetEntity != null && !targetEntity.dead)  // 추적 대상이 존재하고 죽지 않은 경우
            {
                return true;
            }
            return false;
        }
    }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        source = GetComponent<AudioSource>();
        ani = GetComponent<Animator>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetUp(ZombieData zombieData) // 좀비 AI 초기 스펙 설정값 메서드
    {
        startingHealth = zombieData.health; // 초기 체력 설정
        health = startingHealth;            // 현재 체력 초기화

        damage = zombieData.damage;     // 공격력 설정
        agent.speed = zombieData.speed; // 이동속도 설정
        meshRenderer.material.color = zombieData.skinColor; // 색상 설정
    }

    private void Start()
    {
        StartCoroutine (UpdatePath());
    }

    void Update()
    {
        ani.SetBool(hashTarget, hasTarget);
    }

    IEnumerator UpdatePath()
    {
        while (!dead)
        {
            if (hasTarget)
            {
                agent.isStopped = false;
                agent.SetDestination(targetEntity.transform.position);
            }
            else
            {
                agent.isStopped = true;
                // 자기위치에서 20범위 내의 가상의 구를 그렸을 때 구와 겹치는 모든 콜라이더를 가져옴
                Collider[] colliders = Physics.OverlapSphere(transform.position, 20f, targetLayer);
                for(int i = 0; i < colliders.Length; i++)
                {
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();
                    if (livingEntity != null && !livingEntity.dead)     // LivingEntity가 있고 죽지 않은 경우
                    {
                        targetEntity = livingEntity;    // 추적 대상 설정
                        break;  // 첫번째 추적 대상만 설정하고 루프 종료
                    }
                }
            }
            yield return traceWS; // 0.25초마다 경로 업데이트
        }
    }
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!dead)
        {
            hitEffect.transform.position = hitPoint;    // 피격 이펙트 위치 설정
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);  // 피격 이펙트 회전 설정
            hitEffect.Play();
            source.PlayOneShot(hitClip);
        }
        base.OnDamage(damage, hitPoint, hitNormal);
    }
    public override void Die()
    {
        base.Die();
        // 다른 AI를 방해하지 않도록 자신의 모든 콜라이도 비활성화
        Collider[] zomebieColliders = GetComponents<Collider>();
        for (int i = 0; i <= zomebieColliders.Length; i++)
        {
            zomebieColliders[i].enabled = false;    // 좀비의 모든 콜라이더 비활성화
        }
        agent.isStopped = true;
        agent.enabled = false;
        ani.SetTrigger(hashDie);
        source.PlayOneShot(deathClip);
    }
    public void OnTriggerStay(Collider other)   // 트리거 안에 있을 때 특정 기능 유지
    {
        // 트리거가 충돌한 상대방 게임 오브젝트가 추적 대상이라면 공격 실행
        if (!dead && Time.time >= lastAttackTime + timeBetAttack)   // 사망하지 않았고 최근 공격시점에서 timeBetAttack 시간 이상이 지난경우 공격 가능
        {
            LivingEntity attackTarget = other.GetComponent<LivingEntity>(); // 상대방의 LivingEntity 타입 호출 시도
            if (attackTarget != null && attackTarget == targetEntity)   // 상대방의 LivingEntity가 자신의 추적 대상이라면 공격 실행
            {
                lastAttackTime = Time.time; // 마지막 공격시간 갱신
                Vector3 hitPoint = other.ClosestPoint(transform.position);
                Vector3 hitNormal = transform.position - other.transform.position; // 상대방의 피격위치와 피격 방향을 근사값으로 계산
                attackTarget.OnDamage(damage, hitPoint, hitNormal); // 공격 실행
            }
        }
    }
}
