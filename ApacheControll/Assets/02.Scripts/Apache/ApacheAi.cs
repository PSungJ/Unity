using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApacheAi : MonoBehaviour
{
    public enum ApacheState { PATROL, ATTACK, DESTROY }
    public ApacheState state = ApacheState.PATROL;

    [Header("PATROL")]
    [SerializeField] private List<Transform> pathPoint;

    [Header("Speed")]
    [SerializeField] float rotSpeed = 15f, moveSpeed = 10f;
    private Transform myTr;
    private int curIndex = 0;
    private float wayCheck = 10f;
    public bool isSearch = true;

    void Start()
    {
        myTr = GetComponent<Transform>();
        var path = GameObject.Find("PatrolGroup").transform;
        if (path != null)
        {
            path.GetComponentsInChildren<Transform>(pathPoint);
        }
        pathPoint.RemoveAt(0);
    }

    void FixedUpdate()
    {
        if (isSearch)
            WayPatrol();
        else
        {
            Attack();
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
        
        Search();
    }

    private void Search()
    {
        float tankFindDist = (GameObject.FindWithTag("TANK").transform.position - myTr.position).magnitude;
        if (tankFindDist <= 80)
            isSearch = false;
    }

    private void CheckPoint()
    {
        if (Vector3.Distance(transform.position, pathPoint[curIndex].position) <= wayCheck)
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
        Vector3 targetDist = GameObject.FindWithTag("TANK").transform.position - myTr.position;
        myTr.rotation = Quaternion.Slerp(myTr.rotation, Quaternion.LookRotation(targetDist.normalized), Time.deltaTime * rotSpeed);
        
        if (targetDist.magnitude > 80f)
            isSearch = true;
    }
}
