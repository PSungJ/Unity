using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
[RequireComponent(typeof(Animator))]
public class EnemyAI : MonoBehaviour
{
    public enum State //������ ���
    {
        PATROL = 0, TRACE, ATTACK, DIE
    }
    public State state = State.PATROL;
    private Transform playerTr;  //�÷��̾� ��ġ
    private Transform enemyTr;   //enemy��ġ  
    //���� �����Ÿ�
    private Animator animator;
    private EnemyFire enemyFire;
    public float attackDist = 5.0f; //���� ���� �Ѿ� �߻�  ���� �Ÿ� 
    public float traceDist = 10f; //���� ����
    public bool isDie = false;
    //�ڷ�ƾ���� ����� �����ð� ���� 
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
        //Cycle offset ���� �ұ�Ģ �ϰ� ����
        animator.SetFloat(hashOffset,Random.Range(0f, 1f));
        //speed���� �ұ�Ģ�ϰ� ���� 
        animator.SetFloat(hashWalkSpeed,Random.Range(1f, 1.2f));
        Damage.OnPlayerDie += this.OnPlayerDie;
        //�̺�Ʈ ��� 
        //BarrelCtrl.OnEnemyDie += this.Die;
        StartCoroutine(SetHpBar());
    }
    IEnumerator SetHpBar()
    {
        yield return null;


        hpBar = PoolingManager.p_instance.GetHpBar();//��Ȱ��ȭ�� hpbar������Ʈ ��ȯ
        hpBar.gameObject.SetActive(true); //������Ʈ Ȱ��ȭ
        hpBarImage = hpBar.GetComponentsInChildren<Image>()[1]; //fillamount�� �ִ� �ڽ� �̹����� ����

        var _hpBar = hpBar.GetComponent<EnemyHpBar>(); //EnemyHpBar��ũ��Ʈ ��ȯ
        _hpBar.targetTr = this.gameObject.transform; //EnemyHpBar Ŭ�����ȿ� targetTr �ʱ�ȭ
        _hpBar.offset = hpBarOffset; //  hpBar ��ġ��  ���� 



    }
    private void OnDisable()
    {
        Damage.OnPlayerDie -= this.OnPlayerDie;
        //�̺�Ʈ ���� 
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
        //�ڷ�ƾ �Լ� ����
        StopAllCoroutines();
        animator.SetTrigger(hashPlayerDie);
    }


    void Update()
    {
        animator.SetFloat(hashSpeed,moveAgent.speed);

    }
}
