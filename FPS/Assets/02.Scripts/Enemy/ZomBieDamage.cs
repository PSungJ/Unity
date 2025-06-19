using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class ZomBieDamage : MonoBehaviour //상속X  컴퍼넌트X
{
    private Animator animator;
    private Rigidbody rb; //why?
    private NavMeshAgent agent;
    private readonly string playerTag = "Player"; 
    private readonly string jumpTag = "JUMPSURPORT"; 
    private readonly string bulletTag = "BULLET"; 
    private readonly int hashJump = Animator.StringToHash("IsJump"); 
    private readonly int hashHit = Animator.StringToHash("IsHit"); //맞았을 때 애니메이션 해시값
    private readonly int hashDie = Animator.StringToHash("IsDie"); //죽었을 때 애니메이션 해시값
    private int hp;
    private int maxHp = 100; //최대 체력
    //점프 애니메이션 해시값
    bool isJumping = false; //점프 여부를 나타내는 변수
    public bool IsDie = false;
    [Header("UI")]
    public Image hpBar; //체력바 UI 이미지
    public Text hpText; //체력 텍스트 UI
    public Canvas canvas; //캔버스 UI
    public BoxCollider attackCol;
    void Start()
    {
        attackCol = transform.GetChild(19).GetComponent<BoxCollider>();
        //attackCol = GameObject.Find("AttackPoint").GetComponent<BoxCollider>(); //공격 콜라이더를 찾는다.
        //공격 콜라이더 컴포넌트를 가져온다.
        canvas = GetComponentInChildren<Canvas>(); //자식 오브젝트에서 캔버스 컴포넌트를 가져온다.
        hpBar.color = Color.green; 
        hp = maxHp; //초기 체력을 최대 체력으로 설정
        agent = GetComponent<NavMeshAgent>(); //네비게이션 에이전트 컴포넌트를 가져온다.
        animator = GetComponent<Animator>(); //애니메이터 컴포넌트를 가져온다.
        rb = GetComponent<Rigidbody>(); //Rigidbody 컴포넌트를 가져온다.
    }
    private void OnCollisionEnter(Collision col) //콜백 함수
    {
        if(col.gameObject.CompareTag(playerTag))
        {
            rb.isKinematic = true; //충돌시 Rigidbody의 물리적 효과를 해제
            rb.mass = 1000f;
        }
        else if (col.gameObject.CompareTag(bulletTag)) 
        {
            animator.SetTrigger(hashHit); //맞았을 때 애니메이션 트리거 설정
            Destroy(col.gameObject); //총알 오브젝트를 파괴
            hp -= (int)col.gameObject.GetComponent<BulletCtrl>().zomBieDamage;
            hp = Mathf.Clamp(hp, 0, maxHp); //체력을 0과 최대 체력 사이로 제한

            hpBar.fillAmount = (float)hp / maxHp; //체력바 UI 업데이트
            if(hpBar.fillAmount<= 0.3f)
                hpBar.color = Color.red; //체력바가 30% 이하일 때 빨간색으로 변경
            else if(hpBar.fillAmount <= 0.5f)
                hpBar.color = Color.yellow; //체력바가 50%일 때 노란색으로 변경

            hpText.text =$"HP :<color=#ff0000>{hp}</color>"; //체력 텍스트 UI 업데이트

        }
        if(hp <= 0) //체력이 0 이하가 되면
        {
            Die();
        }
    }

    private void Die()
    {
        IsDie = true;
        GetComponent<CapsuleCollider>().enabled = false; //콜라이더 비활성화
        GetComponent<Rigidbody>().isKinematic = true; //Rigidbody의 물리적 효과를 해제
        animator.SetTrigger(hashDie); //죽었을 때 애니메이션 트리거 설정
        Destroy(gameObject, 10f);
        canvas.enabled = false; //캔버스 UI 비활성화
        GameManager.Instance.UpdateKillCount(1);
    }

    private void OnCollisionExit(Collision col) //콜백 함수
    {
        if (col.gameObject.CompareTag(playerTag))
        {
            rb.isKinematic = false; //충돌시 Rigidbody의 물리적 효과를 준다.
            rb.mass = 70f; //원래 질량으로 되돌린다.
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(jumpTag)&& isJumping==false) //플레이어와 충돌했을 때
        {
            isJumping = true;
            animator.SetTrigger(hashJump); //점프 애니메이션 트리거 설정
            agent.speed = 0.1f;
        }
    }
    void Update()
    {
        if(isJumping && agent.isOnOffMeshLink) //점프 애니메이션이 실행 중이고 네비게이션 에이전트가 오프메쉬 링크에 있을 때
        {
           StartCoroutine(EnemyJump()); //점프 코루틴 시작
        }


    }
    IEnumerator EnemyJump()
    {
        AnimatorClipInfo[] clipInfos = animator.GetCurrentAnimatorClipInfo(0); //현재 애니메이션 클립 정보를 가져온다.
                                                                            //0 기본인덱스 레이어 
        yield return new WaitForSeconds(clipInfos.Length); // 애니메이션 클립의 길이만큼 대기한다.
        isJumping = false; //점프 상태를 해제
        agent.speed = 3.5f; //원래 속도로 되돌린다.
    }
    public void EnableAttackCollider() //공격 콜라이더 활성화/비활성화
    {
        attackCol.enabled = true;
    }
    public void DisableAttackCollider()
    {
        attackCol.enabled = false;
    }
}
