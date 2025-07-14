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

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
        // Photon Network �����κ��� ���� �޽����� �޴´�.
        PhotonNetwork.IsMessageQueueRunning = true;
        CreateTank();
    }
    void CreateTank()
    {
        float pos = Random.Range(-100f, 100f);
        PhotonNetwork.Instantiate("Tank", new Vector3(pos, 3f, pos), Quaternion.identity, 0, null);
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
        PhotonNetwork.LeaveRoom();  // ���� ��������
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("LobbyScene");
    }
}
