using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;    // TextMeshPro ���� ���̺귯��
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    private readonly string version = "1.0";    // ���� ����
    private string userId = "Zack";             // ����ID
    public TMP_InputField userName, roomName;

    private Dictionary<string, GameObject> rooms = new Dictionary<string, GameObject>();
    private GameObject roomItemPrefab;
    public Transform scrollContents;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;    // ������ Ŭ���̾�Ʈ �� �ڵ� ����ȭ �ɼ�
        // ������ ���ο� ���� �ε����� ��, �ش� �뿡 ������ �ٸ� ���� �����鿡�Ե� �ڵ����� �ش� ���� �ε����ִ� ����̴�.
        //PhotonNetwork.LoadLevel("");
        PhotonNetwork.GameVersion = version;
        PhotonNetwork.NickName = userId;

        roomItemPrefab = Resources.Load<GameObject>("RoomItem");
        if (PhotonNetwork.IsConnected == false) // ���� ���� ������ �������ִٸ�
        {
            PhotonNetwork.ConnectUsingSettings(); // ���� ���� ����
        }
    }

    private void Start()
    {
        // ����� ������ �ε�
        userId = PlayerPrefs.GetString("USER_ID", $"USER_{Random.Range(1, 21):00}");
        userName.text = userId;

        //���������� �г��� ���
        PhotonNetwork.NickName = userId;
    }

    public string SetRoomName() // �� �̸��� �Է� ���θ� Ȯ���ϴ� �Լ�
    {
        if (string.IsNullOrEmpty(roomName.text))
        {
            roomName.text = $"Room_{Random.Range(1, 101):000}";
        }
        return roomName.text;
    }

    public void SetUserID() // �������� �����ϴ� ����
    {
        if(string.IsNullOrEmpty(userName.text))
        {
            userId = $"USER_{Random.Range(1, 22):00}";
        }
        else
        {
            userId = userName.text;
        }
        PlayerPrefs.SetString("USER_ID", userId);   // ������ ����
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
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print($"Join Random Room Failed {returnCode}\n{message}");  // ������ȣ, ������ ����

        OnMakeRoom();
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
        if (PhotonNetwork.IsMasterClient)   // ������ Ŭ���̾�Ʈ�� ��� �뿡 ���� �� ���� ���� �ε��Ѵ�.
        {
            //PhotonNetwork.IsMessageQueueRunning = false;    // �� �̵��� ���� ���� ��Ʈ��ũ �޽��� ���� ����, LoadLevel �޼��忡 ���ԵǾ��ִ� ����̴�.
            PhotonNetwork.LoadLevel("AngryBotScene");   // ������ Ŭ���̾�Ʈ�� ������ ������ ������ Ŭ���̾�Ʈ���� �ڵ�����
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)  // �� ���� �� ���� �� �ڵ�ȣ��
    {
        // roomList �� ���� ��ȭ�� �߻��� ������ �ݹ��Լ��� ȣ��ȴ�.
        // ������ �뿡 ���� ������ �Ѿ�´�. �� �������δ� RemovedFromList �Ӽ����� Ȯ��
        
        GameObject tempRoom = null; // ������ RoomItem�� ������ �ӽ� ����
        foreach (var roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList)
            {
                // ��ųʸ����� �� �̸����� �˻��Ͽ� ����� RoomItem ������ ����
                rooms.TryGetValue(roomInfo.Name, out tempRoom);
                Destroy(tempRoom);  // RoomItem ������ ����
                rooms.Remove(roomInfo.Name);    // ��ųʸ����� �ش� �� �̸��� ������ ����
            }
            else    // �� ������ ����� ���
            {
                if (rooms.ContainsKey(roomInfo.Name) == false)  // �� ���� ����
                {
                    // RoomItem �������� scrollContents������ ����
                    GameObject roomPrefab = Instantiate(roomItemPrefab, scrollContents);
                    // �� ������ ǥ���ϱ� ���� RoomItem�� RoomInfo ��������
                    roomPrefab.GetComponent<RoomItem>().RoomInfo = roomInfo;
                    // ��ųʸ� �ڷ����� ������ �߰�
                    rooms.Add(roomInfo.Name, roomPrefab);
                }
                else    // ���� �� ���� ����
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
        PhotonNetwork.JoinRandomRoom();     // �������� ������ ������ ���� 
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
        GUILayout.Label(PhotonNetwork.SendRate.ToString()); // ���� �������� �ʴ� ������ ����Ƚ��
    }
}
