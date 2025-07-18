using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public TMP_Text roomName, connectInfo, txtLogMsg, killLogMsg;
    public Button exitBtn;

    IEnumerator Start()
    {
        while (!PhotonNetwork.InRoom)
        {
            yield return null;
        }
        //PhotonNetwork.IsMessageQueueRunning = true; // LoadLevel 메서드에 포함되어있는 함수
        CreatePlayer();
        SetRoomInfo();
        exitBtn.onClick.AddListener(() => OnExitClick());
    }

    private void CreatePlayer()
    {
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length);
        PhotonNetwork.Instantiate("Player", points[idx].position, points[idx].rotation, 0, null);
    }

    void SetRoomInfo() // 룸 접속 정보 출력
    {
        Room room = PhotonNetwork.CurrentRoom;
        roomName.text = room.Name;
        connectInfo.text = $"({room.PlayerCount}/{room.MaxPlayers})";
    }

    void OnExitClick()
    {
        PhotonNetwork.LeaveRoom();  
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("LobbyScene");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        SetRoomInfo();
        string msg = $"\n<color=#00ff00>[{newPlayer}] Connected</color>";
        txtLogMsg.text += msg;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        SetRoomInfo();
        string msg = $"\n<color=#ff0000>[{otherPlayer}] Disconnected</color>";
        txtLogMsg.text += msg;
    }
}
