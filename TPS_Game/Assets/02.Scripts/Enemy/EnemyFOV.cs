using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    // 적 캐릭터 추적 사정거리의 범위
    public float viewRange = 15f;
    // 적 캐릭터 추적 시야각의 범위
    [Range(0, 360)] public float viewAngle = 120f;
    private readonly string playerTag = "Player";
    private Transform enemyTr;
    private Transform playerTr;
    private int playerLayer;
    private int obstacleLayer;
    private int barrelLayer;
    private int layerMask;
    void Start()
    {
        enemyTr = GetComponent<Transform>();
        playerTr = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        playerLayer = LayerMask.NameToLayer("PLAYER");
        obstacleLayer = LayerMask.NameToLayer("OBSTACLE");
        barrelLayer = LayerMask.NameToLayer("BARREL");
        layerMask = 1 << playerLayer | 1 << obstacleLayer | 1 << barrelLayer;
    }
    public Vector3 CirclePoint(float angle)
    {
        angle += transform.eulerAngles.y;   //로컬 좌표계 기준으로 설정하기 위해 적 캐릭터의 y축 회전값을 더해줌
        //원형의 좌표를 구하는 공식
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
        //일반 각도를 라디안 각도로 변환 시킴
        //Mathf.Deg2Rad( PI * 2 / 360)
        //Mathf.Rad2Deg(1f) * Mathf.PI / 180f; 라디안 각도를 일반 각도로 변환 시킴
    }
    public bool isTracePlayer()
    {
        bool isTrace = false;
        Collider[] cols = Physics.OverlapSphere(enemyTr.position, viewRange, 1 << playerLayer);
        //배열의 갯수가 1일 때 주인공이 범위안에 있다고 판단
        if(cols.Length == 1)
        {
            Vector3 dir = (playerTr.position - enemyTr.position).normalized;
            if (Vector3.Angle(enemyTr.forward, dir) < viewAngle * 0.5f)
            {
                isTrace = true;
            }
        }
        return isTrace;
    }
    public bool isVeiwPlayer()
    {
        bool isVeiw = false;
        RaycastHit hit;

        Vector3 dir = (playerTr.position - enemyTr.position).normalized;
        //레이 캐스트를 투사해서 장애물 여부를 판단
        if(Physics.Raycast(enemyTr.position, dir, out hit, viewRange, layerMask))
        {
            isVeiw = hit.collider.CompareTag(playerTag);
        }
        return isVeiw;
    }
}
