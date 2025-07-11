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
        PhotonNetwork.ConnectUsingSettings();   // 포톤 네트워크 접속
        //SoundManager.S_instance.PlayBGM("BGM");
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
        print("룸 연결 실패!!");
        PhotonNetwork.CreateRoom("ㅏㅏ", new RoomOptions { IsOpen = true, IsVisible = true, MaxPlayers = 20});
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("룸 접속 성공");
        StartCoroutine(LoadBattleField());  // 다른 씬으로 이동하기 위해 코루틴 선언
    }

    IEnumerator LoadBattleField()
    {
        // 씬 이동하는 동안 Photon Cloud 서버로부터 네트워크 메시지 수신 중단
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
