using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;       // ����Ƽ ���� ���� Photon Unity Network
using Photon.Realtime;  // ���� ���� ���� ���̺귯��

// ��ġ����ŷ(������) ������ �� ���� ���
public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1.0";
    public Text connectInfoTxt; // ��Ʈ��ũ ������ ǥ���� �ؽ�Ʈ
    public Button joinBtn;      // �� ���� ��ư

    private void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();   // �����Ʈ��ũ�� ���� �� ����
        joinBtn.interactable = false;
        connectInfoTxt.text = $"Connect Into HostServer...";
    }

    // ������ ���� ���� ���� �� �ڵ� ����
    public override void OnConnectedToMaster()
    {
        joinBtn.interactable = true;
        connectInfoTxt.text = $"Host Server is Connected!!";
    }

    // ������ ���� ���� ���� �� �ڵ� ����
    public override void OnDisconnected(DisconnectCause cause)
    {
        joinBtn.interactable = false;
        connectInfoTxt.text = $"Host Server Disconnected!!\nReconnecting Server...";
    }

    // �� ���� �õ� - ��ư OnClick�� ÷��
    public void Connect()
    {
        joinBtn.interactable = false;   // �ߺ� Ŭ�� ������ ���� ��ư Ŭ�� �� ��Ȱ��ȭ
        if (PhotonNetwork.IsConnected)
        {
            connectInfoTxt.text = $"Connecting...";
        }
        else
        {
            connectInfoTxt.text = $"Host Server Disconnected!!\nReconnecting Server...";
            PhotonNetwork.ConnectUsingSettings();   // ������ ������ ������ �õ�
        }
    }

    // �� ���� ��� ���� �� ������ ������ �� ����
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectInfoTxt.text = $"No Games Available!!\nAuto Create New Game...";
        PhotonNetwork.CreateRoom(null, new RoomOptions { IsOpen = true, IsVisible = true, MaxPlayers = 4 }, TypedLobby.Default);
    }

    // �� ���� ������ �ڵ� ����
    public override void OnJoinedRoom()
    {
        connectInfoTxt.text = $"Success Join Game!!";
        PhotonNetwork.LoadLevel("MainScene");
        // ���� �޼��尡 ���� �Ǹ� �ٸ� �÷��̾���� ��ǻ�Ϳ����� �ڵ����� PhotonNetwork.LoadLevel("MainScene")�� ����Ǿ� ����� ���� ���� �ε��ϰԵȴ�.
        // PhotonNetwork.LoadLevel �ϸ� ���� ���� �ڴʰ� �ش� �뿡 ������ �ٸ� �÷��̾ PhotonNetwork.LoadLevel�� ���� �÷��̾��� ���� ���� �������� ��,
        // ���߿� ������ �÷��̾ �ش� ���� ����� �ٸ� �÷��̾ ���� ���� ����� �����ϰ� �ڵ� �����Ǿ� ���ϴ�.
    }
}
