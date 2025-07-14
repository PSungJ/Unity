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
    public InputField roomName;
    public GameObject roomItem; // roomPrefab 프리팹
    public Transform scrollContents;    // roomItem이 생성될 위치

    void Awake()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.GameVersion = Version;
            PhotonNetwork.ConnectUsingSettings();   // 포톤 네트워크 접속
            roomName.text = $"Room_{Random.Range(0, 999).ToString("000")}";
        }
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
        //PhotonNetwork.CreateRoom("ㅏㅏ", new RoomOptions { IsOpen = true, IsVisible = true, MaxPlayers = 20});
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

    public void OnClickCreateRoom()
    {
        string _roomName = roomName.text;
        if (string.IsNullOrEmpty(roomName.text))
        {
            _roomName = $"Room_{Random.Range(0, 999).ToString("000")}";
        }
        // 설정한 아이디를 대입
        PhotonNetwork.NickName = userID.text;
        PlayerPrefs.SetString("USER_ID", userID.text);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 20;

        PhotonNetwork.CreateRoom(_roomName, roomOptions, TypedLobby.Default);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)  // 룸이 생성되거나 삭제시 자동으로 호출
    {
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("ROOMITEM"))
        {
            Destroy(obj);   // 삭제 할 때마다 룸을 처음부터 다시 구성한다.
        }

        foreach (RoomInfo roomInfo in roomList)
        {
            //RoomItem 프리팹 동적 생성
            GameObject roomPrefab = Instantiate(roomItem);
            roomPrefab.transform.SetParent(scrollContents.transform, false);

            RoomData roomData = roomPrefab.GetComponent<RoomData>();
            roomData.roomName = roomInfo.Name;
            roomData.connectPlayer = roomInfo.PlayerCount;
            roomData.maxPlayer = roomInfo.MaxPlayers;
            
            roomData.DisplayRoomData();
            // RoomItem의 Button 컴퍼넌트에 클릭 이벤트를 동적으로 연결
            // 이것을 동적 이벤트 리스너라고 한다.
            roomData.GetComponent<Button>().onClick.AddListener(delegate { OnClickRoomItem(roomData.roomName); });            
        }
    }

    void OnClickRoomItem(string roomName)
    {
        PhotonNetwork.NickName = userID.text;
        PlayerPrefs.SetString("USER_ID", userID.text);
        PhotonNetwork.JoinRoom(roomName);
    }

    public void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.NetworkClientState.ToString());
    }
}
