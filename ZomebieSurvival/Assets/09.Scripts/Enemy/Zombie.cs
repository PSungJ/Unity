using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
// 1. �ʿ��� ���۳�Ʈ : NavMeshAgent, Audio(source,clip), Collider, 
// BloodEffect, ������
public class Zombie : LivingEntity
{
    public LayerMask targetLayer;       // ���� ��� ���̾�
    private LivingEntity targetEntity;  // ������ ��� LivingEntity

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

    // ���� ����� �ִ��� �˷��ִ� ������Ƽ
    private bool hasTarget
    {
        get
        {
            if(targetEntity != null && !targetEntity.dead)  // ���� ����� �����ϰ� ���� ���� ���
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

    public void SetUp(ZombieData zombieData) // ���� AI �ʱ� ���� ������ �޼���
    {
        startingHealth = zombieData.health; // �ʱ� ü�� ����
        health = startingHealth;            // ���� ü�� �ʱ�ȭ

        damage = zombieData.damage;     // ���ݷ� ����
        agent.speed = zombieData.speed; // �̵��ӵ� ����
        meshRenderer.material.color = zombieData.skinColor; // ���� ����
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
                // �ڱ���ġ���� 20���� ���� ������ ���� �׷��� �� ���� ��ġ�� ��� �ݶ��̴��� ������
                Collider[] colliders = Physics.OverlapSphere(transform.position, 20f, targetLayer);
                for(int i = 0; i < colliders.Length; i++)
                {
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();
                    if (livingEntity != null && !livingEntity.dead)     // LivingEntity�� �ְ� ���� ���� ���
                    {
                        targetEntity = livingEntity;    // ���� ��� ����
                        break;  // ù��° ���� ��� �����ϰ� ���� ����
                    }
                }
            }
            yield return traceWS; // 0.25�ʸ��� ��� ������Ʈ
        }
    }
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!dead)
        {
            hitEffect.transform.position = hitPoint;    // �ǰ� ����Ʈ ��ġ ����
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);  // �ǰ� ����Ʈ ȸ�� ����
            hitEffect.Play();
            source.PlayOneShot(hitClip);
        }
        base.OnDamage(damage, hitPoint, hitNormal);
    }
    public override void Die()
    {
        base.Die();
        // �ٸ� AI�� �������� �ʵ��� �ڽ��� ��� �ݶ��̵� ��Ȱ��ȭ
        Collider[] zomebieColliders = GetComponents<Collider>();
        for (int i = 0; i <= zomebieColliders.Length; i++)
        {
            zomebieColliders[i].enabled = false;    // ������ ��� �ݶ��̴� ��Ȱ��ȭ
        }
        agent.isStopped = true;
        agent.enabled = false;
        ani.SetTrigger(hashDie);
        source.PlayOneShot(deathClip);
    }
    public void OnTriggerStay(Collider other)   // Ʈ���� �ȿ� ���� �� Ư�� ��� ����
    {
        // Ʈ���Ű� �浹�� ���� ���� ������Ʈ�� ���� ����̶�� ���� ����
        if (!dead && Time.time >= lastAttackTime + timeBetAttack)   // ������� �ʾҰ� �ֱ� ���ݽ������� timeBetAttack �ð� �̻��� ������� ���� ����
        {
            LivingEntity attackTarget = other.GetComponent<LivingEntity>(); // ������ LivingEntity Ÿ�� ȣ�� �õ�
            if (attackTarget != null && attackTarget == targetEntity)   // ������ LivingEntity�� �ڽ��� ���� ����̶�� ���� ����
            {
                lastAttackTime = Time.time; // ������ ���ݽð� ����
                Vector3 hitPoint = other.ClosestPoint(transform.position);
                Vector3 hitNormal = transform.position - other.transform.position; // ������ �ǰ���ġ�� �ǰ� ������ �ٻ簪���� ���
                attackTarget.OnDamage(damage, hitPoint, hitNormal); // ���� ����
            }
        }
    }
}
