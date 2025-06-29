using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
[RequireComponent(typeof(Animator))]
public class EnemyAI : MonoBehaviour
{
    public enum State //열거형 상수
    {
        PATROL = 0, TRACE, ATTACK, DIE
    }
    public State state = State.PATROL;
    private Transform playerTr;  //플레이어 위치
    private Transform enemyTr;   //enemy위치  
    //공격 사정거리
    private Animator animator;
    private EnemyFire enemyFire;
    public float attackDist = 5.0f; //공격 범위 총알 발사  사정 거리 
    public float traceDist = 10f; //추적 범위
    public bool isDie = false;
    //코루틴에서 사용할 지연시간 변수 
    private MoveAgent moveAgent;
    private WaitForSeconds ws;
    private WaitForSeconds pushWs;
    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashDie = Animator.StringToHash("Die");
    private readonly int hashDieIdx = Animator.StringToHash("DieIdx");
    private readonly int hashOffset = Animator.StringToHash("Offset");
    private readonly int hashWalkSpeed = Animator.StringToHash("WalkSpeed");
    private readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");
    public Image hpBarImage;
    public GameObject hpBar;
    [Header("HpBar")]
    [SerializeField] Vector3 hpBarOffset = new Vector3(0, 2.2f, 0);
    
    void Awake()
    {
        enemyFire = GetComponent<EnemyFire>();
        animator = GetComponent<Animator>();
        moveAgent = GetComponent<MoveAgent>();
        playerTr = GameObject.FindWithTag("Player").transform;
        enemyTr = GetComponent<Transform>();
        ws = new WaitForSeconds(0.3f);
        pushWs = new WaitForSeconds(3.0f);
    }
    private void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(CheckAction());
        //Cycle offset 값을 불규칙 하게 변경
        animator.SetFloat(hashOffset,Random.Range(0f, 1f));
        //speed값을 불규칙하게 변경 
        animator.SetFloat(hashWalkSpeed,Random.Range(1f, 1.2f));
        Damage.OnPlayerDie += this.OnPlayerDie;
        //이벤트 등록 
        //BarrelCtrl.OnEnemyDie += this.Die;
        StartCoroutine(SetHpBar());
    }
    IEnumerator SetHpBar()
    {
        yield return null;


        hpBar = PoolingManager.p_instance.GetHpBar();//비활성화된 hpbar오브젝트 반환
        hpBar.gameObject.SetActive(true); //오브젝트 활성화
        hpBarImage = hpBar.GetComponentsInChildren<Image>()[1]; //fillamount가 있는 자식 이미지에 접근

        var _hpBar = hpBar.GetComponent<EnemyHpBar>(); //EnemyHpBar스크립트 반환
        _hpBar.targetTr = this.gameObject.transform; //EnemyHpBar 클래스안에 targetTr 초기화
        _hpBar.offset = hpBarOffset; //  hpBar 위치값  지정 



    }
    private void OnDisable()
    {
        Damage.OnPlayerDie -= this.OnPlayerDie;
        //이벤트 해제 
        //BarrelCtrl.OnEnemyDie -= this.Die;
    }
    IEnumerator CheckState()
    {
        while(!isDie)
        {
            if(state == State.DIE) yield break;
            float dist = Vector3.Distance(enemyTr.position, playerTr.position);
            if(dist <= attackDist)
            {
                state = State.ATTACK;
            }
            else if(dist <= traceDist) 
            {
                 state =State.TRACE;
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

            switch(state)
            {

                case State.PATROL:
                    enemyFire.isFire = false;
                    moveAgent.patrolling = true;
                    animator.SetBool(hashMove,true);
                    break;
                case State.TRACE:
                    enemyFire.isFire = false;
                    moveAgent.traceTarget = playerTr.position;
                    animator.SetBool(hashMove, true);
                    break;
                case State.ATTACK:
                    moveAgent.Stop();
                    animator.SetBool(hashMove, false);
                    enemyFire.isFire = true;
                    break;
                case State.DIE:
                    Die();
                    break;
            }


        }

    }

    public void Die()
    {
        //hpBarImage.GetComponentsInChildren<Image>()[1].color = Color.clear;
        state = State.DIE;
        isDie = true;
        moveAgent.Stop();
        enemyFire.isFire = false;
        animator.SetInteger(hashDieIdx, Random.Range(0, 2));
        animator.SetTrigger(hashDie);
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        hpBarImage.color = Color.clear;
        hpBar.gameObject.SetActive(false);

        GameManager.Instance.IncrementKillCount();
        StartCoroutine(PoolPush());
    }
    IEnumerator PoolPush()
    {
        yield return pushWs;
        this.gameObject.SetActive(false);
        GetComponent<CapsuleCollider>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
        isDie = false;
        state = State.PATROL;
        hpBarImage.color = Color.red;
        hpBarImage.fillAmount = 1f;


    }
    public void OnPlayerDie()
    {
        moveAgent.Stop();
        enemyFire.isFire = false;
        //코루틴 함수 종료
        StopAllCoroutines();
        animator.SetTrigger(hashPlayerDie);
    }


    void Update()
    {
        animator.SetFloat(hashSpeed,moveAgent.speed);

    }
}
