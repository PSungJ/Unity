using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Animator ani;
    [SerializeField] private Transform enemyTr;
    [SerializeField] private NavMeshAgent navi;
    private LayerMask enemyLayer;
    private LayerMask terrainLayer;
    Vector3 target = Vector3.zero;

    void Start()
    {
        ani = GetComponent<Animator>();
        navi = GetComponent<NavMeshAgent>();
        enemyLayer = LayerMask.GetMask("ENEMY");
        terrainLayer = LayerMask.GetMask("TERRAIN");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MoveMousePoint();
        }
        Attack();

        ani.SetFloat("Move", navi.velocity.magnitude, 0.0002f, Time.deltaTime);
    }

    void MoveMousePoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 30f, enemyLayer))
        {
            navi.isStopped = false;
            Debug.Log(hit.collider.name);
            target = hit.point;
            navi.speed = 5f;
            navi.stoppingDistance = 2f;
            enemyTr = hit.transform;
        }
        else if (Physics.Raycast(ray, out hit, 30f, terrainLayer))
        {
            navi.isStopped = false;
            Debug.Log(hit.collider.name);
            target = hit.point;
            navi.speed = 1.5f;
            enemyTr = null;
        }
        navi.destination = target;
    }

    private void Attack()
    {
        if (navi.remainingDistance <= 2f && enemyTr != null)
            ani.SetTrigger("Attack");
    }
}
