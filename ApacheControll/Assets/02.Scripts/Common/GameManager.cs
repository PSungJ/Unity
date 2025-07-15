using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance = null;
    public bool isGameOver = false;
    public Text txtConnect;
    public Text txtLogMsg;

    [SerializeField] private GameObject apachePrefab;
    [SerializeField] private List<Transform> spawnPointList;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
        // Photon Network �����κ��� ���� �޽����� �޴´�.
        PhotonNetwork.IsMessageQueueRunning = true;
        CreateTank();
        var spawnPoint = GameObject.Find("SpawnPoint").transform;
        if (spawnPoint != null)
            spawnPoint.GetComponentsInChildren<Transform>(spawnPointList);
        spawnPointList.RemoveAt(0);
    }

    private void Start()
    {
        string msg = $"\n<color=#00ff00>[{PhotonNetwork.NickName}] Connected</color>";
        photonView.RPC("LogMsg", RpcTarget.AllBuffered, msg);

        if (spawnPointList.Count > 0 && PhotonNetwork.IsMasterClient)
            InvokeRepeating("CreateApache", 0.02f, 3.0f);
    }

    void CreateTank()
    {
        float pos = Random.Range(-100f, 100f);
        PhotonNetwork.Instantiate("Tank", new Vector3(pos, 3f, pos), Quaternion.identity, 0, null);
    }

    void CreateApache()
    {
        if (isGameOver) return;
        int count = (int)GameObject.FindGameObjectsWithTag("APACHE").Length;
        if (count < 10)
        {
            int idx = Random.Range(0, spawnPointList.Count);
            PhotonNetwork.InstantiateRoomObject(apachePrefab.name, spawnPointList[idx].position, spawnPointList[idx].rotation, 0, null);
            // �� �����ڰ� ������ Apache ��ä�� �濡 �����־�� �ϱ� ������ Instantiate�� �ƴ� InstantiateRoomObject�� ����Ѵ�.
        }
    }

    [PunRPC]
    public void ApplyPlayerCountUpdate()
    {
        Room currentRoom = PhotonNetwork.CurrentRoom;
        txtConnect.text = $"{currentRoom.PlayerCount.ToString()}/{currentRoom.MaxPlayers.ToString()}";
    }

    [PunRPC]
    void GetConnectPlayerCount()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("ApplyPlayerCountUpdate", RpcTarget.All);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)  // ���ο� �÷��̾� ����
    {
        GetConnectPlayerCount();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)   // �̿����� �÷��̾� ����
    {
        GetConnectPlayerCount();
    }

    public void OnClickExitRoom()
    {
        string msg = $"\n<color=#ff0000>[{PhotonNetwork.NickName}] Disconnected</color>";
        photonView.RPC("LogMsg", RpcTarget.AllBuffered, msg);

        PhotonNetwork.LeaveRoom();  // ���� ���������鼭 ������ ��� ��Ʈ��ũ�� ����
    }

    public override void OnLeftRoom()   // �뿡�� ������ ����Ǿ��� �� ȣ��Ǵ� �ݹ� �Լ�(�ڵ�ȣ��)
    {
        SceneManager.LoadScene("LobbyScene");
    }

    [PunRPC]
    void LogMsg(string msg)
    {
        txtLogMsg.text += msg;
    }
}
