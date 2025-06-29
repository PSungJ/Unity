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

    private bool _patrolling; //���� ���θ� �Ǵ�
    public bool patrolling //������Ƽ 
    {
        get { return _patrolling; }
        set
        {
            _patrolling = value;
            if(_patrolling)
            {
                agent.speed = patrollSpeed;
                damping = 1f;//���� �Ҷ� ȸ�� ��� 
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
            damping = 7f;//���� �Ҷ� ȸ�� ��� 
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
        agent.isStopped =true; //��������
        agent.velocity = Vector3.zero;  //ĳ���Ͱ� ����
        _patrolling=false;

    }
    void Start()
    {
        #region  ��������Ʈ ��ġ ��� ���1
        //Transform[] wayPoints =GameObject.Find("WayPointGroup").GetComponentsInChildren<Transform>();
        //if (wayPoints != null )
        //{
        //    foreach (Transform point in wayPoints) 
        //        wayPointList.Add(point);

        //    wayPointList.RemoveAt(0);
        //}
        #endregion
        #region  ��������Ʈ ��ġ ��� ���2
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
        agent.updateRotation = false; //������Ʈ���� ȸ���ϴ� ��� ��Ȱ��ȭ
        agent.speed = patrollSpeed;
        enemyTr = GetComponent<Transform>();
        this._patrolling = true;
    }
    void MovWayPoint()
    {
        //�ִ� ��ΰ���� ������ ������  ���� ���� ���� �ʴ´�.
        if (agent.isPathStale) return;
        //���� �����      =  ��Ʈ�� ��ġ�� ���� �ִ� wayPointList �迭�� �ε���
        agent.destination = wayPointList[nextIdx].position;
        agent.isStopped = false; //���� Ȱ��ȭ 

    }

    void Update()
    {      //��ĳ���Ͱ� �̵����̶�� 
        if(agent.isStopped ==false&& agent.desiredVelocity !=Vector3.zero)
        {    //NavMeshAgent�� ������ ���� ���͸� ���ʹϾ� Ÿ���� ������ ��ȯ 
          
             Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
            //��麸�� �Լ��� �̿��ؼ� ���������� �ε巴�� ȸ�� ��Ŵ 
            
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
