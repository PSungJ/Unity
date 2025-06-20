using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 상태에 따른 공격 추격 애니메이션 등을 구현(FSM)
[RequireComponent(typeof(Animator))]
public class EnemyAi : MonoBehaviour
{
    public enum State   // 열거형 상수
    {
        PATROL = 0, TRACE = 1, ATTACK = 2, DIE = 3
    }
    public State state = State.PATROL;

    private readonly int hashMove = Animator.StringToHash("isMove");
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashDie = Animator.StringToHash("Die");
    private readonly int hashDieIdx = Animator.StringToHash("DieIdx");
    private Transform playerTr;
    private Transform tr;
    private MoveAgent moveAgent;
    private Animator ani;
    public float attackDist = 5f;
    public float traceDist = 10f;
    public bool isDie = false;
    private EnemyFire enemyFire;

    // 코루틴에서 사용할 지연시간 변수
    private WaitForSeconds ws;

    void Awake()
    {
        ani = GetComponent<Animator>();
        moveAgent = GetComponent<MoveAgent>();
        playerTr = GameObject.FindWithTag("Player").transform;
        tr = GetComponent<Transform>();
        ws = new WaitForSeconds(0.3f);
        enemyFire = GetComponent<EnemyFire>();
    }
    private void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(CheckAction());
    }
    IEnumerator CheckState()
    {
        while (!isDie)
        {
            if (state == State.DIE) yield break;

            float dist = Vector3.Distance(tr.position, playerTr.position);
            if (dist <= attackDist)
            {
                state = State.ATTACK;
            }
            else if (dist <= traceDist)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.PATROL;
            }
            yield return ws;
        }
    }
    IEnumerator CheckAction()
    {
        while (!isDie)
        {
            yield return ws;
            switch (state)
            {
                case State.PATROL:
                    moveAgent.patrolling = true;
                    enemyFire.isFire = false;
                    ani.SetBool(hashMove, true);
                    break;
                case State.TRACE:
                    moveAgent.traceTarget = playerTr.position;
                    enemyFire.isFire = false;
                    ani.SetBool(hashMove, true);
                    break;
                case State.ATTACK:
                    moveAgent.Stop();
                    enemyFire.isFire = true;
                    ani.SetBool(hashMove, false);
                    break;
                case State.DIE:
                    Die();
                    break;
            }
        }
    }

    public void Die()
    {
        isDie = true;
        moveAgent.Stop();
        enemyFire.isFire = false;
        ani.SetInteger(hashDieIdx, Random.Range(0, 2));
        ani.SetTrigger(hashDie);
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        StopAllCoroutines();
    }

    void Update()
    {
        ani.SetFloat(hashSpeed, moveAgent.speed);
    }
}
