using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// 플레이어 탱크가 여러개일때, Apache는 플레이어 중 가장 가까운 거리에 있는 대상을 탐색하여 공격해야한다.
public class ApacheAi : MonoBehaviourPun, IPunObservable
{
    private readonly string tankTag = "TANK";
    public enum ApacheState { PATROL, ATTACK, DESTROY }
    public ApacheState state = ApacheState.PATROL;

    [Header("PATROL")]
    [SerializeField] private List<Transform> pathPoint;

    [Header("Speed")]
    [SerializeField] float rotSpeed = 15f, moveSpeed = 10f;
    private Transform myTr;
    private int curIndex = 0;
    private float wayCheck = 10f;

    [Header("Player Tank")]
    [SerializeField] private GameObject[] playerTanks = null;
    public TankDamage tank;
    Transform closetTank;

    Vector3 newWorkPosition = Vector3.zero;
    Quaternion newWorkRotation = Quaternion.identity;

    void Awake()
    {
        myTr = GetComponent<Transform>();
        tank = Resources.Load<GameObject>("Tank").GetComponent<TankDamage>();
    }
    private void Start()
    {
        var path = GameObject.Find("PatrolGroup").transform;
        if (path != null)
        {
            path.GetComponentsInChildren<Transform>(pathPoint);
        }
        pathPoint.RemoveAt(0);
        newWorkPosition = myTr.position;
        newWorkRotation = myTr.rotation;

        if (PhotonNetwork.IsMasterClient)
            InvokeRepeating("UpdateTankList", 0f, 0.5f);    // StartCoroutine으로 할 수도 있다.
    }

    void FixedUpdate()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            switch (state)
            {
                case ApacheState.PATROL:
                    WayPatrol();
                    break;
                case ApacheState.ATTACK:
                    Attack();
                    break;
            }
        }
        else // 다른 클라이언트는 네트워크에서 받은 위치로 부드럽게 이동
        {
            myTr.position = Vector3.Lerp(myTr.position, newWorkPosition, Time.deltaTime * 10f);
            myTr.rotation = Quaternion.Slerp(myTr.rotation, newWorkRotation, Time.deltaTime * 10f);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)   // 로컬인 자신의 이동과 회전은 송신
        {
            stream.SendNext(myTr.position);
            stream.SendNext(myTr.rotation);
            stream.SendNext((int)this.state);  // 상태정보도 동기화 해준다
        }
        else    // 리모트의 이동과 회전은 수신
        {
            newWorkPosition = (Vector3)stream.ReceiveNext();
            newWorkRotation = (Quaternion)stream.ReceiveNext();
            this.state = (ApacheState)stream.ReceiveNext();
        }
    }

    private void Update()
    {
        CheckPoint();
    }

    private void WayPatrol()
    {
        state = ApacheState.PATROL;
        Vector3 movePos = pathPoint[curIndex].position - myTr.position;
        myTr.rotation = Quaternion.Slerp(myTr.rotation, Quaternion.LookRotation(movePos), Time.fixedDeltaTime * rotSpeed);
        myTr.Translate(Vector3.forward * moveSpeed * Time.fixedDeltaTime);

        if (closetTank != null && Vector3.Distance(closetTank.position, myTr.position) < 80f)
            state = ApacheState.ATTACK;
    }

    void UpdateTankList()
    {
        if (PhotonNetwork.IsMasterClient)
            closetTank = FindClosetTank();
    }

    private Transform FindClosetTank()
    {
        playerTanks = GameObject.FindGameObjectsWithTag(tankTag);
        
        // 탱크가 한대도 없으면 null을 반환하여 에러방지
        if (playerTanks == null || playerTanks.Length == 0)
        {
            return null;
        }

        Transform target = null;    // 거리를 재기 위한 기준(첫번째 배열에 있는 탱크가 기준)
        float closetDistSqr = Mathf.Infinity;
        foreach (GameObject _tank in playerTanks)
        {   
            // 나와 탱크 사이의 제곱 거리를 계산
            float distSqr = (_tank.transform.position - myTr.position).sqrMagnitude;
            // 현재까지 가장 가까웠던 거리보다 더 가까우면 타겟으로 설정
            if (distSqr < closetDistSqr)
            {
                closetDistSqr = distSqr;
                target = _tank.transform;
            }
        }
        return target;
    }

    private void CheckPoint()
    {
        if (Vector3.Distance(transform.position, pathPoint[Random.Range(0, pathPoint.Count)].position) <= wayCheck)
        {
            if (curIndex == pathPoint.Count - 1)
                curIndex = 0;
            else
                curIndex++;
        }
    }

    private void Attack()
    {
        state = ApacheState.ATTACK;
        Vector3 _normal = (closetTank.position - myTr.position).normalized;
        //Vector3 targetDist = GameObject.FindWithTag(tankTag).transform.position - myTr.position;
        myTr.rotation = Quaternion.Slerp(myTr.rotation, Quaternion.LookRotation(_normal), Time.deltaTime * rotSpeed);

        if (Vector3.Distance(closetTank.position, myTr.position) > 80f)
            state = ApacheState.PATROL;
    }
}
