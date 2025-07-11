using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Photon.Pun.Demo.Cockpit;
using UnityEngine.UI;

public class Photon_Init : MonoBehaviourPunCallbacks
{
    public string Version = "V1.1.0";
    public InputField userID;

    void Awake()
    {
        PhotonNetwork.GameVersion = Version;
        PhotonNetwork.ConnectUsingSettings();   // ���� ��Ʈ��ũ ����
        //SoundManager.S_instance.PlayBGM("BGM");
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
        userID.text = GetUserID();
    }

    string GetUserID()
    {
        string userID = PlayerPrefs.GetString("USER_ID");
        if (string.IsNullOrEmpty(userID))
        {
            userID = $"USER_{Random.Range(0, 999).ToString("000")}";
        }
        return userID;
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
        StartCoroutine(LoadBattleField());  // �ٸ� ������ �̵��ϱ� ���� �ڷ�ƾ ����
    }

    IEnumerator LoadBattleField()
    {
        // �� �̵��ϴ� ���� Photon Cloud �����κ��� ��Ʈ��ũ �޽��� ���� �ߴ�
        PhotonNetwork.IsMessageQueueRunning = false;
        AsyncOperation ao = SceneManager.LoadSceneAsync("BattlelField");

        yield return ao;
    }

    public void OnClickJoinRandom()
    {
        PhotonNetwork.NickName = userID.text;
        PlayerPrefs.SetString("USER_ID", userID.text);
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.NetworkClientState.ToString());
    }
}
