using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Chat;
using ExitGames.Client.Photon;
using AuthenticationValues = Photon.Chat.AuthenticationValues;  // 포톤 챗 인증

public class ChatManager : MonoBehaviourPun, IChatClientListener
{
    private ChatClient chatClient;
    private string userName;
    private string curChName;

    public InputField inputChat;
    public Text outPutText;
    public Text channelCheck;

    void Start()
    {
        Application.runInBackground = true;     // 백그라운드에서 구동되게한다.
        userName = DateTime.Now.ToShortTimeString();    // 현재 시간을 userName으로 임시설정
        curChName = "Channel 001";
        chatClient = new ChatClient(this);
        chatClient.UseBackgroundWorkerForSending = true;    // true가 아니면 앱이 백그라운드로 갈 때 연결이 끊어짐
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, "1.0", new AuthenticationValues(userName));
        AddLine(string.Format("연결시도", userName));
    }

    public void AddLine(string line)
    {
        channelCheck.text = line + "\r\n";
    }

    public void OnApplicationQuit() // 앱 종료시 호출
    {
        if (chatClient != null)
            chatClient.Disconnect();
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        if (level == ExitGames.Client.Photon.DebugLevel.ERROR)
            Debug.LogError(message);
        else if (level == ExitGames.Client.Photon.DebugLevel.WARNING)
            Debug.LogWarning(message);
        else
            Debug.Log(message);
    }

    public void OnConnected()   // 채팅 서버 연결 성공
    {
        AddLine("채팅 서버에 연결되었습니다.");
        chatClient.Subscribe(new string[] { curChName }, 10);
    }

    public void OnDisconnected()
    {
        AddLine("채팅 서버와의 연결이 끊어졌습니다.");
    }

    public void OnChatStateChange(ChatState state)  // 현재 클라이언트 상태 확인
    {
        Debug.Log("OnChatStateChange = " + state);
    }

    public void OnSubscribed(string[] channels, bool[] results) // 채팅 채널 입장
    {
        AddLine(string.Format("채널 입장 ({0})", string.Join(",", channels)));
    }

    public void OnUnsubscribed(string[] channels)   // 채팅 채널 퇴장
    {
        AddLine(string.Format("채널 퇴장 ({0})", string.Join(",", channels)));
    }

    // Update()에서 chatClient.Service() 호출 시 해당 메서드 자동 호출
    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        if (channelName.Equals(curChName))
            this.ShowChannel(curChName);
    }

    public void ShowChannel(string channelName)
    {
        if (string.IsNullOrEmpty(channelName)) return;
        ChatChannel chatChannel = null;
        bool isfound = this.chatClient.TryGetChannel(channelName, out chatChannel);
        if (!isfound)
        {
            Debug.Log("Failed to find Channel : " +  channelName);
            return;
        }
        this.curChName = channelName;
        this.outPutText.text = chatChannel.ToStringMessages();  // 유저 이름과 채팅입력내용이 한번에 보여짐
        Debug.Log("ShowChannel : " + curChName);
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        Debug.Log("status : " + string.Format("{0} is {1}, Msg : {2} ", user, status, message));
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        Debug.Log("OnPrivateMessage : " + message);
    }

    void Update()
    {
        chatClient.Service();
    }

    public void OnUserSubscribed(string channel, string user)
    {
        throw new NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        throw new NotImplementedException();
    }

    // InputField OnEndEdit 호출 메서드
    public void OnEnter()
    {
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
        {
            this.SendChat(this.inputChat.text);
            this.inputChat.text = "";
        }
    }
    
    // 게임내에서 Enter키 입력 시 OnEnter로 호출시킬 메서드
    void SendChat(string inputLine)
    {
        if (string.IsNullOrEmpty(inputLine)) return;
        this.chatClient.PublishMessage(curChName, inputLine);
    }
}
