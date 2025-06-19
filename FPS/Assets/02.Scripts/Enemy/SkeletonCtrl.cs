using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonCtrl : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform skeletonTr;
    [SerializeField] private NavMeshAgent navi;
    [SerializeField] private SkeletonDamage s_damage;
    public float traceDist = 20.0f; //추적 범위
    public float attackDist = 3.0f; //공격 범위
    public Transform playerTr;
    public float rotSpeed = 30f;
    private readonly int hashAttack = Animator.StringToHash("IsAttack");
    private readonly int hashTrace = Animator.StringToHash("IsTrace");
    //동적할당과 동시에 문자열을 읽어서 탐색을 해서 찾은 문자열을  정수로 변환
    void Start()
    {
        s_damage = GetComponent<SkeletonDamage>();
        skeletonTr = transform;
        animator = GetComponent<Animator>();
        navi = GetComponent<NavMeshAgent>(); //네비게이션 컴포넌트 가져옴
        playerTr = GameObject.FindWithTag("Player").transform;
        //하이라키에서 Player라는 태그를 가진오브젝트의 트랜스폼을 가져옴

    }
    void Update()
    {
        if (s_damage.IsDie) return;
       

        float dist = Vector3.Distance(skeletonTr.position, playerTr.position);
        //float dist =(playerTr.position - zomBieTr.position).magnitude; //벡터의 크기(거리)를 구함
        if (dist <= attackDist) //공격중일때
        {
            PlayerAttack();

        }
        else if (dist <= traceDist) //추적중일때
        {
            PlayerTrace();

        }
        else //공격도 아니고 추적도 아닐때
        {
            PlayerIdle();
        }



    }

    private void PlayerIdle()
    {
        animator.SetBool(hashTrace, false);
        navi.isStopped = true; //추적 범위 밖일 때 정지
    }

    private void PlayerTrace()
    {
        animator.SetBool(hashAttack, false);
        animator.SetBool(hashTrace, true);
        navi.isStopped = false; //추적 범위 안에 들어오면 네비 추적 시작
        navi.destination = playerTr.position; //플레이어의 위치를 목적지로 설정
    }

    private void PlayerAttack()
    {
        animator.SetBool(hashAttack, true);

        navi.isStopped = true; //공격중일 때 네비 추적정지

        Vector3 attackTarget = (playerTr.position - skeletonTr.position).normalized; //공격 대상과의 거리
                                                                                   //타겟위치 - 자기자신 좀비 위치 = 방향
        Quaternion rot = Quaternion.LookRotation(attackTarget); //좀비가 플레이어를 바라보게 회전
        skeletonTr.rotation = Quaternion.Slerp(skeletonTr.rotation, rot, Time.deltaTime * rotSpeed);
        //부드럽게 회전
        //곡면보간 함수  //자기자신회전값 ,타겟회전 , 시간 만큼 회전 
    }
}
