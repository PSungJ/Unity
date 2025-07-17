using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    private readonly string version = "1.0";    // ���� ����
    private string userId = "Zack";             // ����ID

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;    // ������ Ŭ���̾�Ʈ �� �ڵ� ����ȭ �ɼ�
        // ������ ���ο� ���� �ε����� ��, �ش� �뿡 ������ �ٸ� ���� �����鿡�Ե� �ڵ����� �ش� ���� �ε����ִ� ����̴�.
        //PhotonNetwork.LoadLevel("");
        PhotonNetwork.GameVersion = version;
        PhotonNetwork.NickName = userId;

        Debug.Log(PhotonNetwork.SendRate);  // ���� �������� �ʴ� ������ ���� Ƚ��
        PhotonNetwork.ConnectUsingSettings();   // ���� ���� ����
    }

    public override void OnConnectedToMaster()  // ������ Ŭ���̾�Ʈ�� ����
    {
        Debug.Log($"Connected to Master");
        print($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()    // �κ� ���� �� ȣ��Ǵ� �ݹ� �Լ�
    {
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinRandomRoom();     // ������ �� ���� �õ�
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //base.OnJoinRandomFailed(returnCode, message);
        print($"Join Random Room Failed {returnCode}\n{message}");  // ������ȣ, ������ ����

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;

        PhotonNetwork.CreateRoom("My Room", roomOptions, TypedLobby.Default);
    }

    public override void OnCreatedRoom()    // �� ������ �Ϸ�� �� ȣ��Ǵ� �ݹ��Լ�
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
            // NickName�� ������ ���� �ƴ϶� ������ �г����� ���� �� �� �ֵ�.
            // ������ ������ ���� �ʿ��� ��� ActorNumber�� ����Ͽ��� �Ѵ�.
        }
    }

    private static void CreatePlayer()
    {
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length);
        PhotonNetwork.Instantiate("Player", points[idx].position, points[idx].rotation, 0, null);
    }
}
