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
        PhotonNetwork.ConnectUsingSettings();   // 포톤 네트워크 접속
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log($"호스트 접속");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log($"로비에 접속");
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        print("룸 연결 실패!!");
        PhotonNetwork.CreateRoom("ㅏㅏ", new RoomOptions { IsOpen = true, IsVisible = true, MaxPlayers = 20});
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("룸 접속 성공");
    }
}
