using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    private readonly string version = "1.0";    // 게임 버전
    private string userId = "Zack";             // 유저ID

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;    // 마스터 클라이언트 씬 자동 동기화 옵션
        // 방장이 새로운 씬을 로딩했을 때, 해당 룸에 입장한 다른 접속 유저들에게도 자동으로 해당 씬을 로딩해주는 기능이다.
        //PhotonNetwork.LoadLevel("");
        PhotonNetwork.GameVersion = version;
        PhotonNetwork.NickName = userId;

        Debug.Log(PhotonNetwork.SendRate);  // 포톤 서버와의 초당 데이터 전송 횟수
        PhotonNetwork.ConnectUsingSettings();   // 포톤 서버 접속
    }

    public override void OnConnectedToMaster()  // 마스터 클라이언트에 접속
    {
        Debug.Log($"Connected to Master");
        print($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()    // 로비에 접속 후 호출되는 콜백 함수
    {
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinRandomRoom();     // 무작위 룸 접속 시도
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //base.OnJoinRandomFailed(returnCode, message);
        print($"Join Random Room Failed {returnCode}\n{message}");  // 오류번호, 오류의 이유

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;

        PhotonNetwork.CreateRoom("My Room", roomOptions, TypedLobby.Default);
    }

    public override void OnCreatedRoom()    // 룸 생성이 완료된 후 호출되는 콜백함수
    {
        Debug.Log($"Room Create Success!!");
        Debug.Log($"Room Name : {PhotonNetwork.CurrentRoom.Name}");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"PhotonNetwork.InRoom : {PhotonNetwork.InRoom}");
        Debug.Log($"PlayerCount : {PhotonNetwork.CurrentRoom.PlayerCount}");

        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log($"{player.Value.NickName}, {player.Value.ActorNumber}");
            // NickName은 고유의 값이 아니라서 동일한 닉네임이 존재 할 수 있따.
            // 접속자 고유의 값이 필요한 경우 ActorNumber를 사용하여야 한다.
        }
    }

    private static void CreatePlayer()
    {
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length);
        PhotonNetwork.Instantiate("Player", points[idx].position, points[idx].rotation, 0, null);
    }
}
