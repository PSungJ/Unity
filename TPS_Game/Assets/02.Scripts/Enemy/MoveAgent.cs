using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class MoveAgent : MonoBehaviour
{
    public List<Transform>wayPointList = new List<Transform>();
    public int nextIdx = 0;
    private NavMeshAgent agent;
    private readonly float patrollSpeed = 1.5f;
    private readonly float traceSpeed = 4.0f;
    private float damping = 1.0f;
    private Transform enemyTr;

    private bool _patrolling; //순찰 여부를 판단
    public bool patrolling //프로퍼티 
    {
        get { return _patrolling; }
        set
        {
            _patrolling = value;
            if(_patrolling)
            {
                agent.speed = patrollSpeed;
                damping = 1f;//순찰 할때 회전 계수 
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
            agent.speed = traceSpeed;
            TraceTarget(_traceTarget);
            damping = 7f;//추적 할때 회전 계수 
        }
    }
    public float speed { get { return agent.velocity.magnitude; } }

    void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale) return;
        agent.destination = pos;
        agent.isStopped = false;

    }
    public void Stop()
    {
        agent.isStopped =true; //추적중지
        agent.velocity = Vector3.zero;  //캐릭터가 멈춤
        _patrolling=false;

    }
    void Start()
    {
        #region  웨이포인트 위치 잡는 방법1
        //Transform[] wayPoints =GameObject.Find("WayPointGroup").GetComponentsInChildren<Transform>();
        //if (wayPoints != null )
        //{
        //    foreach (Transform point in wayPoints) 
        //        wayPointList.Add(point);

        //    wayPointList.RemoveAt(0);
        //}
        #endregion
        #region  웨이포인트 위치 잡는 방법2
        var group = GameObject.Find("WayPointGroup");
        if (group != null)
        {
            group.GetComponentsInChildren<Transform>(wayPointList);
            wayPointList.RemoveAt(0);
            nextIdx = Random.Range(0, wayPointList.Count);
        }
        #endregion
        agent =GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        agent.updateRotation = false; //에이전트에서 회전하는 기능 비활성화
        agent.speed = patrollSpeed;
        enemyTr = GetComponent<Transform>();
        this._patrolling = true;
    }
    void MovWayPoint()
    {
        //최단 경로계산이 끝나지 않으면  다음 수행 하지 않는다.
        if (agent.isPathStale) return;
        //추적 대상은      =  패트롤 위치를 갖고 있는 wayPointList 배열의 인덱스
        agent.destination = wayPointList[nextIdx].position;
        agent.isStopped = false; //추적 활성화 

    }

    void Update()
    {      //적캐릭터가 이동중이라면 
        if(agent.isStopped ==false&& agent.desiredVelocity !=Vector3.zero)
        {    //NavMeshAgent가 가야할 방향 벡터를 쿼터니언 타입의 각도로 변환 
          
             Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
            //곡면보간 함수를 이용해서 점진적으로 부드럽게 회전 시킴 
            
              enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot ,Time.deltaTime *damping);
                             
        }


        if (!_patrolling) return;

        if(agent.remainingDistance <= 0.5f)
        {

           //nextIdx = ++nextIdx % wayPointList.Count;
           nextIdx = Random.Range(0,wayPointList.Count);
            MovWayPoint() ;
        }
        
    }
}
