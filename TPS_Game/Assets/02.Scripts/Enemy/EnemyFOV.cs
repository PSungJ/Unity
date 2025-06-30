using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    // �� ĳ���� ���� �����Ÿ��� ����
    public float viewRange = 15f;
    // �� ĳ���� ���� �þ߰��� ����
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
        angle += transform.eulerAngles.y;   //���� ��ǥ�� �������� �����ϱ� ���� �� ĳ������ y�� ȸ������ ������
        //������ ��ǥ�� ���ϴ� ����
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
        //�Ϲ� ������ ���� ������ ��ȯ ��Ŵ
        //Mathf.Deg2Rad( PI * 2 / 360)
        //Mathf.Rad2Deg(1f) * Mathf.PI / 180f; ���� ������ �Ϲ� ������ ��ȯ ��Ŵ
    }
    public bool isTracePlayer()
    {
        bool isTrace = false;
        Collider[] cols = Physics.OverlapSphere(enemyTr.position, viewRange, 1 << playerLayer);
        //�迭�� ������ 1�� �� ���ΰ��� �����ȿ� �ִٰ� �Ǵ�
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
        //���� ĳ��Ʈ�� �����ؼ� ��ֹ� ���θ� �Ǵ�
        if(Physics.Raycast(enemyTr.position, dir, out hit, viewRange, layerMask))
        {
            isVeiw = hit.collider.CompareTag(playerTag);
        }
        return isVeiw;
    }
}
