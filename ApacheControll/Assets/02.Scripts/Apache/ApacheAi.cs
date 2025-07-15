using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// �÷��̾� ��ũ�� �������϶�, Apache�� �÷��̾� �� ���� ����� �Ÿ��� �ִ� ����� Ž���Ͽ� �����ؾ��Ѵ�.
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
            InvokeRepeating("UpdateTankList", 0f, 0.5f);    // StartCoroutine���� �� ���� �ִ�.
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
        else // �ٸ� Ŭ���̾�Ʈ�� ��Ʈ��ũ���� ���� ��ġ�� �ε巴�� �̵�
        {
            myTr.position = Vector3.Lerp(myTr.position, newWorkPosition, Time.deltaTime * 10f);
            myTr.rotation = Quaternion.Slerp(myTr.rotation, newWorkRotation, Time.deltaTime * 10f);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)   // ������ �ڽ��� �̵��� ȸ���� �۽�
        {
            stream.SendNext(myTr.position);
            stream.SendNext(myTr.rotation);
            stream.SendNext((int)this.state);  // ���������� ����ȭ ���ش�
        }
        else    // ����Ʈ�� �̵��� ȸ���� ����
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
        
        // ��ũ�� �Ѵ뵵 ������ null�� ��ȯ�Ͽ� ��������
        if (playerTanks == null || playerTanks.Length == 0)
        {
            return null;
        }

        Transform target = null;    // �Ÿ��� ��� ���� ����(ù��° �迭�� �ִ� ��ũ�� ����)
        float closetDistSqr = Mathf.Infinity;
        foreach (GameObject _tank in playerTanks)
        {   
            // ���� ��ũ ������ ���� �Ÿ��� ���
            float distSqr = (_tank.transform.position - myTr.position).sqrMagnitude;
            // ������� ���� ������� �Ÿ����� �� ������ Ÿ������ ����
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
