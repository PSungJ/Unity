using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class MoveAgent : MonoBehaviour
{
    [SerializeField] private List<Transform> wayPointList = new List<Transform>();
    [SerializeField] private int nextIdx = 0;
    [SerializeField] private NavMeshAgent navi;
    private readonly float patrollSpeed = 1.5f;
    private readonly float traceSpeed = 4.0f;
    private float damping = 1.0f;
    private Transform EnemyTr;
    private bool _patrolling;   // 순찰여부
    public bool patrolling
    {
        get { return _patrolling; }
        set
        {
            _patrolling = value;
            if(_patrolling)
            {
                navi.speed = patrollSpeed;
                damping = 1.0f; // 순찰 할 때 회전 계수
            }
        }
    }
    private Vector3 _traceTarget;
    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set
        {
            _traceTarget = value;
            navi.speed = traceSpeed;
            TraceTarget(_traceTarget);
            damping = 7.0f; // 추적 할 때 회전 계수
        }
    }
    public float speed
    {
        get { return navi.velocity.magnitude; }
    }
    void TraceTarget(Vector3 pos)
    {
        if (navi.isPathStale) return;
        navi.destination = pos;
        navi.isStopped = false;
    }
    public void Stop()
    {
        navi.isStopped = true;  // 추적 중지
        navi.velocity = Vector3.zero;   // 캐릭터가 멈춤
        _patrolling = false;
    }
    void Start()
    {
        #region 웨이포인트 List잡는방법 1
        //Transform[] wayPoints = GameObject.Find("WayPointGroup").GetComponentsInChildren<Transform>();
        //if (wayPoints != null)
        //{
        //    foreach (Transform point in wayPoints)
        //        wayPointList.Add(point);

        //    wayPointList.RemoveAt(0);
        //}
        #endregion

        #region 웨이포인트 List잡는방법 2
        var group = GameObject.Find("WayPointGroup");
        if (group != null)
        {
            group.GetComponentsInChildren<Transform>(wayPointList);
            wayPointList.RemoveAt(0);
        }
        #endregion
        navi = GetComponent<NavMeshAgent>();
        navi.autoBraking = false;
        navi.updateRotation = false;    // 에이전트에서 회전하는 기능 비활성화
        navi.speed = patrollSpeed;
        this._patrolling = true;
        EnemyTr = GetComponent<Transform>();
    }
    void MoveWayPoint()
    {
        // 최단 경로계산이 끝나지 않으면 다음을 수행하지 않는다.
        if (navi.isPathStale) return;
        navi.destination = wayPointList[nextIdx].position;
        navi.isStopped = false;
    }

    void FixedUpdate()
    {
        if (navi.isStopped == false)
        {   // NavMeshAgent가 가야할 방향 벡터를 Quaternion 타입의 각도로 변환
            Quaternion rot = Quaternion.LookRotation(navi.desiredVelocity);
            EnemyTr.rotation = Quaternion.Slerp(EnemyTr.rotation, rot, Time.deltaTime * damping);
        }
        if (!_patrolling) return;

        if (navi.remainingDistance <= 0.5f)
        {
            nextIdx = ++nextIdx % wayPointList.Count;
            MoveWayPoint();
        }
    }
}
