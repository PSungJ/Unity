using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;       // 유니티 전용 포톤 Photon Unity Network
using Photon.Realtime;  // 포톤 서비스 관련 라이브러리

// 매치메이킹(마스터) 서버와 룸 접속 담당
public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1.0";
    public Text connectInfoTxt; // 네트워크 정보를 표시할 텍스트
    public Button joinBtn;      // 룸 접속 버튼

    private void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();   // 포톤네트워크로 버전 별 접속
        joinBtn.interactable = false;
        connectInfoTxt.text = $"Connect Into HostServer...";
    }

    // 마스터 서버 접속 성공 시 자동 실행
    public override void OnConnectedToMaster()
    {
        joinBtn.interactable = true;
        connectInfoTxt.text = $"Host Server is Connected!!";
    }

    // 마스터 서버 접속 실패 시 자동 실행
    public override void OnDisconnected(DisconnectCause cause)
    {
        joinBtn.interactable = false;
        connectInfoTxt.text = $"Host Server Disconnected!!\nReconnecting Server...";
    }

    // 룸 접속 시도 - 버튼 OnClick에 첨부
    public void Connect()
    {
        joinBtn.interactable = false;   // 중복 클릭 방지를 위해 버튼 클릭 시 비활성화
        if (PhotonNetwork.IsConnected)
        {
            connectInfoTxt.text = $"Connecting...";
        }
        else
        {
            connectInfoTxt.text = $"Host Server Disconnected!!\nReconnecting Server...";
            PhotonNetwork.ConnectUsingSettings();   // 마스터 서버로 재접속 시도
        }
    }

    // 빈 방이 없어서 랜덤 룸 참가에 실패할 때 실행
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectInfoTxt.text = $"No Games Available!!\nAuto Create New Game...";
        PhotonNetwork.CreateRoom(null, new RoomOptions { IsOpen = true, IsVisible = true, MaxPlayers = 4 }, TypedLobby.Default);
    }

    // 룸 접속 성공시 자동 실행
    public override void OnJoinedRoom()
    {
        connectInfoTxt.text = $"Success Join Game!!";
        PhotonNetwork.LoadLevel("MainScene");
        // 위의 메서드가 실행 되면 다른 플레이어들의 컴퓨터에서도 자동으로 PhotonNetwork.LoadLevel("MainScene")이 실행되어 방장과 같은 씬을 로드하게된다.
        // PhotonNetwork.LoadLevel 하면 좋은 점은 뒤늦게 해당 룸에 입장한 다른 플레이어가 PhotonNetwork.LoadLevel로 기존 플레이어들과 같은 씬에 도착했을 때,
        // 도중에 참가한 플레이어도 해당 씬의 모습이 다른 플레이어가 보는 씬의 모습과 동일하게 자동 구성되어 편하다.
    }
}
