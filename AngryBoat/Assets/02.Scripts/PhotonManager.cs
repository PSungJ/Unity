using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;    // TextMeshPro 관련 라이브러리
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    private readonly string version = "1.0";    // 게임 버전
    private string userId = "Zack";             // 유저ID
    public TMP_InputField userName, roomName;

    private Dictionary<string, GameObject> rooms = new Dictionary<string, GameObject>();
    private GameObject roomItemPrefab;
    public Transform scrollContents;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;    // 마스터 클라이언트 씬 자동 동기화 옵션
        // 방장이 새로운 씬을 로딩했을 때, 해당 룸에 입장한 다른 접속 유저들에게도 자동으로 해당 씬을 로딩해주는 기능이다.
        //PhotonNetwork.LoadLevel("");
        PhotonNetwork.GameVersion = version;
        PhotonNetwork.NickName = userId;

        roomItemPrefab = Resources.Load<GameObject>("RoomItem");
        if (PhotonNetwork.IsConnected == false) // 포톤 서버 연결이 끊어져있다면
        {
            PhotonNetwork.ConnectUsingSettings(); // 포톤 서버 접속
        }
    }

    private void Start()
    {
        // 저장된 유저명 로드
        userId = PlayerPrefs.GetString("USER_ID", $"USER_{Random.Range(1, 21):00}");
        userName.text = userId;

        //접속유저의 닉네임 등록
        PhotonNetwork.NickName = userId;
    }

    public string SetRoomName() // 방 이름의 입력 여부를 확인하는 함수
    {
        if (string.IsNullOrEmpty(roomName.text))
        {
            roomName.text = $"Room_{Random.Range(1, 101):000}";
        }
        return roomName.text;
    }

    public void SetUserID() // 유저명을 설정하는 로직
    {
        if(string.IsNullOrEmpty(userName.text))
        {
            userId = $"USER_{Random.Range(1, 22):00}";
        }
        else
        {
            userId = userName.text;
        }
        PlayerPrefs.SetString("USER_ID", userId);   // 유저명 저장
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
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print($"Join Random Room Failed {returnCode}\n{message}");  // 오류번호, 오류의 이유

        OnMakeRoom();
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
        if (PhotonNetwork.IsMasterClient)   // 마스터 클라이언트인 경우 룸에 입장 후 메인 씬을 로딩한다.
        {
            //PhotonNetwork.IsMessageQueueRunning = false;    // 씬 이동시 포톤 서버 네트워크 메시지 수신 금지, LoadLevel 메서드에 포함되어있는 기능이다.
            PhotonNetwork.LoadLevel("AngryBotScene");   // 마스터 클라이언트가 접속한 씬으로 나머지 클라이언트들이 자동접속
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)  // 방 생성 및 제거 시 자동호출
    {
        // roomList 룸 정보 변화가 발생할 때마다 콜백함수가 호출된다.
        // 삭제된 룸에 대한 정보도 넘어온다. 룸 삭제여부는 RemovedFromList 속성으로 확인
        
        GameObject tempRoom = null; // 삭제된 RoomItem을 저장할 임시 변수
        foreach (var roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList)
            {
                // 딕셔너리에서 룸 이름으로 검색하여 저장된 RoomItem 프리팹 추출
                rooms.TryGetValue(roomInfo.Name, out tempRoom);
                Destroy(tempRoom);  // RoomItem 프리팹 삭제
                rooms.Remove(roomInfo.Name);    // 딕셔너리에서 해당 룸 이름의 데이터 삭제
            }
            else    // 룸 정보가 변경된 경우
            {
                if (rooms.ContainsKey(roomInfo.Name) == false)  // 룸 새로 생성
                {
                    // RoomItem 프리팹을 scrollContents하위로 생성
                    GameObject roomPrefab = Instantiate(roomItemPrefab, scrollContents);
                    // 룸 정보를 표시하기 위해 RoomItem의 RoomInfo 정보전달
                    roomPrefab.GetComponent<RoomItem>().RoomInfo = roomInfo;
                    // 딕셔너리 자료형에 데이터 추가
                    rooms.Add(roomInfo.Name, roomPrefab);
                }
                else    // 기존 룸 정보 갱신
                {
                    rooms.TryGetValue(roomInfo.Name, out tempRoom);
                    tempRoom.GetComponent<RoomItem>().RoomInfo = roomInfo;
                }
            }
        }
    }

    #region UI_BUTTON_EVENT
    public void OnLoginBtn()
    {
        SetUserID();
        PhotonNetwork.JoinRandomRoom();     // 무작위로 추출한 룸으로 입장 
    }
    
    public void OnMakeRoom()
    {
        SetUserID();

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;

        PhotonNetwork.CreateRoom(SetRoomName(), roomOptions, TypedLobby.Default, null);
    }
    #endregion

    private void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.SendRate.ToString()); // 포톤 서버와의 초당 데이터 전송횟수
    }
}
