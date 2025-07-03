using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseCart : MonoBehaviour
{
    [Header("Path Line")]
    [SerializeField] private List<Transform> NodeList;
    [SerializeField] private int currentNodeIdx = 0;

    [SerializeField] private Animator ani;
    [SerializeField] private Transform tr;

    void Start()
    {
        tr = GetComponent<Transform>();
        ani = GetComponent<Animator>();
        var path = GameObject.Find("PathPoint").transform;
        if (path != null)
        {
            path.GetComponentsInChildren<Transform>(NodeList);
        }
        NodeList.RemoveAt(0);
    }
    void FixedUpdate()
    {
        WayPointMove();
        CheckDistance();
    }
    void WayPointMove()
    {
        Vector3 movePos = NodeList[currentNodeIdx].position - tr.position;
        tr.rotation = Quaternion.Slerp(tr.rotation, Quaternion.LookRotation(movePos), Time.fixedDeltaTime * 10f);
        if (tr.rotation.y <= 0)
        {
            ani.SetBool("isWalk", true);
            ani.SetTrigger("Left");
        }
        else
            ani.SetTrigger("Right");
            

            tr.Translate(Vector3.forward * 10f * Time.fixedDeltaTime);
    }
    void CheckDistance()
    {
        if (Vector3.Distance(transform.position, NodeList[currentNodeIdx].position) <= 2.5f)
        {
            if (currentNodeIdx == NodeList.Count - 1)
                currentNodeIdx = 0;
            else
                currentNodeIdx++;
        }
    }
}
