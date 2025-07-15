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
    // 룸 아이템을 이름으로 관리하기 위한 딕셔너리
    private Dictionary<string, GameObject> roomItemCache = new Dictionary<string, GameObject>();

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
    {   // 이 메서드는 PUN2에서 제공하는 콜백함수, 단순히 삭제 로직만 추가하면 아래의 생성로직으로 생성된 방들이 사라지지않고 중복 생성되는 문제가 발생한다.
        // 올바른 방법은 삭제한 방은 삭제, 추가되거나 정보가 바뀐 방은 정보만 업데이트하는 로직으로 구성하여야한다.
        
        // 1. 새로 받은 룸 리스트에서 현재 유효한 룸 이름들만 저장(빠른 조회를 위해 HashSet 사용)
        HashSet<string> activeRoomNames = new HashSet<string>();
        foreach (RoomInfo roomInfo in roomList)
        {
            // 비활성화, 닫힌 방 등은 목록에서 제외 (PUN 기본설정)
            if (!roomInfo.RemovedFromList)
                activeRoomNames.Add(roomInfo.Name);
        }
        // 2. 현재 UI로 표시중인 룸(캐시) 중에서 사라져야 할 룸을 찾아 삭제(이 부분이 "나간 방 삭제" 로직에 해당된다)
        List<string> roomsToDelete = new List<string>();
        foreach (string cachedRoomName in roomItemCache.Keys)
        {
            if (!activeRoomNames.Contains(cachedRoomName))
                roomsToDelete.Add(cachedRoomName);
        }

        foreach (string roomNameToDelete in roomsToDelete)
        {
            // 해당 GameObject파괴
            Destroy(roomItemCache[roomNameToDelete]);
            // 캐시에서 제거
            roomItemCache.Remove(roomNameToDelete);
        }
        // 3. 새로 받은 룸 리스트를 기준으로 UI 업데이트 및 생성
        foreach (RoomInfo roomInfo in roomList)
        {
            // 이미 삭제 처리된 방이면 건너뛰기
            if (roomInfo.RemovedFromList) continue;

            GameObject roomItemObject;

            // 3-1. 이미 존재하는 룸이면 정보만 업데이트
            if (roomItemCache.TryGetValue(roomInfo.Name, out roomItemObject))
            {
                RoomData roomData = roomItemObject.GetComponent<RoomData>();
                roomData.connectPlayer = roomInfo.PlayerCount;
                roomData.maxPlayer = roomInfo.MaxPlayers;
                roomData.DisplayRoomData(); // UI 텍스트 업데이트
            }
            // 3-2. 새로 생긴 룸이면 생성
            else
            {
                GameObject newRoom = Instantiate(roomItem, scrollContents.transform);
                RoomData roomData = newRoom.GetComponent<RoomData>();
                roomData.roomName = roomInfo.Name;
                roomData.connectPlayer = roomInfo.PlayerCount;
                roomData.maxPlayer = roomInfo.MaxPlayers;
                roomData.DisplayRoomData();

                roomData.GetComponent<Button>().onClick.AddListener(() =>
                {
                    OnClickRoomItem(roomData.roomName);
                });

                // 새로 생성한 룸을 캐시에 추가
                roomItemCache.Add(roomInfo.Name, newRoom);
            }
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
