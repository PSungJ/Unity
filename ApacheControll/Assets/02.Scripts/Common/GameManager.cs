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
        // Photon Network 서버로부터 오는 메시지를 받는다.
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
            // 방 생성자가 나가도 Apache 객채는 방에 남아있어야 하기 때문에 Instantiate가 아닌 InstantiateRoomObject를 사용한다.
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

    public override void OnPlayerEnteredRoom(Player newPlayer)  // 새로운 플레이어 입장
    {
        GetConnectPlayerCount();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)   // 이용중인 플레이어 퇴장
    {
        GetConnectPlayerCount();
    }

    public void OnClickExitRoom()
    {
        string msg = $"\n<color=#ff0000>[{PhotonNetwork.NickName}] Disconnected</color>";
        photonView.RPC("LogMsg", RpcTarget.AllBuffered, msg);

        PhotonNetwork.LeaveRoom();  // 룸을 빠져나가면서 생성한 모든 네트워크를 삭제
    }

    public override void OnLeftRoom()   // 룸에서 접속이 종료되었을 때 호출되는 콜백 함수(자동호출)
    {
        SceneManager.LoadScene("LobbyScene");
    }

    [PunRPC]
    void LogMsg(string msg)
    {
        txtLogMsg.text += msg;
    }
}
