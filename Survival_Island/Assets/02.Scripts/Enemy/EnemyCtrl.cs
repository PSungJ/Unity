using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCtrl : MonoBehaviour
{
    [SerializeField] private Animator ani;
    [SerializeField] private Transform tr;
    [SerializeField] private Transform playerTr;
    [SerializeField] private NavMeshAgent navi;
    private Rigidbody rb;
    private float traceDist = 20f;
    private float attackDist = 3f;
    private float rotSpeed = 30f;
    private readonly string bulletTag = "BULLET";
    private readonly string playerTag = "Player";
    private readonly int hashTrace = Animator.StringToHash("isTrace");
    private readonly int hashAttack = Animator.StringToHash("isAttack");
    private readonly int hashHit = Animator.StringToHash("isHit");
    private readonly int hashDie = Animator.StringToHash("isDie");
    private BoxCollider attackCol;
    private int Hp;
    private int maxHp = 100;
    private bool isDie = false;

    void Start()
    {
        attackCol = GameObject.Find("WeaponCollider").GetComponent<BoxCollider>();
        Hp = maxHp;
        ani = GetComponent<Animator>();
        tr = GetComponent<Transform>();
        navi = GetComponent<NavMeshAgent>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
    }
    public void EnableAttackCollider()
    {
        attackCol.enabled = true;
    }
    public void DisableAttackCollider()
    {
        attackCol.enabled = false;
    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(playerTag))
        {
            rb.isKinematic = true;
            rb.mass = 1000f;
        }
        else if (col.gameObject.CompareTag(bulletTag))
        {
            ani.SetTrigger(hashHit);
            Hp -= 20;
            Hp = Mathf.Clamp(Hp, 0, maxHp);
            Debug.Log("Hit");
        }
        if (Hp <= 0)
        {
            Die();
        }
    }
    void Update()
    {
        if (isDie) return;

        float dist = Vector3.Distance(tr.position, playerTr.position);
        
        if(dist <= attackDist)
        {
            Attack();
        }
        else if (dist <= traceDist)
        {
            Trace();
        }
        else
        {
            Idle();
        }
    }

    private void Trace()
    {
        ani.SetBool(hashTrace, true);
        ani.SetBool(hashAttack, false);
        navi.isStopped = false;
        navi.destination = playerTr.position;
    }
    private void Attack()
    {
        ani.SetBool(hashAttack, true);
        ani.SetBool(hashTrace, false);
        navi.isStopped = true;
        Vector3 attackTarget = (playerTr.position - tr.position).normalized;
        Quaternion rot = Quaternion.LookRotation(attackTarget);
        tr.rotation = Quaternion.Slerp(tr.rotation, rot, Time.deltaTime * rotSpeed);
    }
    private void Idle()
    {
        ani.SetBool(hashTrace, false);
        navi.isStopped = true;
    }
    private void Die()
    {
        isDie = true;
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        ani.SetTrigger(hashDie);
        Destroy(gameObject, 5f);
    }
}
