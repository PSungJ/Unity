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
    public GameObject roomItem; // roomPrefab ������
    public Transform scrollContents;    // roomItem�� ������ ��ġ
    // �� �������� �̸����� �����ϱ� ���� ��ųʸ�
    private Dictionary<string, GameObject> roomItemCache = new Dictionary<string, GameObject>();

    void Awake()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.GameVersion = Version;
            PhotonNetwork.ConnectUsingSettings();   // ���� ��Ʈ��ũ ����
            roomName.text = $"Room_{Random.Range(0, 999).ToString("000")}";
        }
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
        //PhotonNetwork.CreateRoom("����", new RoomOptions { IsOpen = true, IsVisible = true, MaxPlayers = 20});
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

    public void OnClickCreateRoom()
    {
        string _roomName = roomName.text;
        if (string.IsNullOrEmpty(roomName.text))
        {
            _roomName = $"Room_{Random.Range(0, 999).ToString("000")}";
        }
        // ������ ���̵� ����
        PhotonNetwork.NickName = userID.text;
        PlayerPrefs.SetString("USER_ID", userID.text);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 20;

        PhotonNetwork.CreateRoom(_roomName, roomOptions, TypedLobby.Default);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)  // ���� �����ǰų� ������ �ڵ����� ȣ��
    {   // �� �޼���� PUN2���� �����ϴ� �ݹ��Լ�, �ܼ��� ���� ������ �߰��ϸ� �Ʒ��� ������������ ������ ����� ��������ʰ� �ߺ� �����Ǵ� ������ �߻��Ѵ�.
        // �ùٸ� ����� ������ ���� ����, �߰��ǰų� ������ �ٲ� ���� ������ ������Ʈ�ϴ� �������� �����Ͽ����Ѵ�.
        
        // 1. ���� ���� �� ����Ʈ���� ���� ��ȿ�� �� �̸��鸸 ����(���� ��ȸ�� ���� HashSet ���)
        HashSet<string> activeRoomNames = new HashSet<string>();
        foreach (RoomInfo roomInfo in roomList)
        {
            // ��Ȱ��ȭ, ���� �� ���� ��Ͽ��� ���� (PUN �⺻����)
            if (!roomInfo.RemovedFromList)
                activeRoomNames.Add(roomInfo.Name);
        }
        // 2. ���� UI�� ǥ������ ��(ĳ��) �߿��� ������� �� ���� ã�� ����(�� �κ��� "���� �� ����" ������ �ش�ȴ�)
        List<string> roomsToDelete = new List<string>();
        foreach (string cachedRoomName in roomItemCache.Keys)
        {
            if (!activeRoomNames.Contains(cachedRoomName))
                roomsToDelete.Add(cachedRoomName);
        }

        foreach (string roomNameToDelete in roomsToDelete)
        {
            // �ش� GameObject�ı�
            Destroy(roomItemCache[roomNameToDelete]);
            // ĳ�ÿ��� ����
            roomItemCache.Remove(roomNameToDelete);
        }
        // 3. ���� ���� �� ����Ʈ�� �������� UI ������Ʈ �� ����
        foreach (RoomInfo roomInfo in roomList)
        {
            // �̹� ���� ó���� ���̸� �ǳʶٱ�
            if (roomInfo.RemovedFromList) continue;

            GameObject roomItemObject;

            // 3-1. �̹� �����ϴ� ���̸� ������ ������Ʈ
            if (roomItemCache.TryGetValue(roomInfo.Name, out roomItemObject))
            {
                RoomData roomData = roomItemObject.GetComponent<RoomData>();
                roomData.connectPlayer = roomInfo.PlayerCount;
                roomData.maxPlayer = roomInfo.MaxPlayers;
                roomData.DisplayRoomData(); // UI �ؽ�Ʈ ������Ʈ
            }
            // 3-2. ���� ���� ���̸� ����
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

                // ���� ������ ���� ĳ�ÿ� �߰�
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
