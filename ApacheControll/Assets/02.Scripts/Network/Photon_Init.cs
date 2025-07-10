using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Photon.Pun.Demo.Cockpit;

public class Photon_Init : MonoBehaviourPunCallbacks
{
    public string Version = "V1.1.0";

    void Start()
    {
        PhotonNetwork.GameVersion = Version;
        PhotonNetwork.ConnectUsingSettings();   // ���� ��Ʈ��ũ ����
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log($"ȣ��Ʈ ����");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log($"�κ� ����");
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        print("�� ���� ����!!");
        PhotonNetwork.CreateRoom("����", new RoomOptions { IsOpen = true, IsVisible = true, MaxPlayers = 20});
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("�� ���� ����");
    }
}
