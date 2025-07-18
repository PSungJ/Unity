using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomItem : MonoBehaviourPun
{
    [SerializeField] private PhotonManager photonManager;
    [SerializeField] private TMP_Text roomInfoText;
    private RoomInfo _roomInfo;

    public RoomInfo RoomInfo
    {
        get { return _roomInfo; }
        set
        {
            _roomInfo = value;
            roomInfoText.text = $"{_roomInfo.Name} ({_roomInfo.PlayerCount}/{_roomInfo.MaxPlayers})";  // ���� �̺�Ʈ ������ �����Ͽ� ������ �� �ش� �뿡 ����
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(()=>OnEnterRoom(_roomInfo.Name));
        }
    }

    void Awake()
    {
        roomInfoText = GetComponentInChildren<TMP_Text>();
        photonManager = GameObject.Find("PhotonManager").GetComponent<PhotonManager>();
    }

    void OnEnterRoom(string roomName)
    {
        photonManager.SetUserID();

        RoomOptions roomOptions = new RoomOptions();    // �� �ɼ� ����
        roomOptions.MaxPlayers = 20;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;

        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default, null);  // �� ����
    }
}
