using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Chat;
using Photon.Realtime;
using ExitGames.Client.Photon;
using AuthenticationValues = Photon.Chat.AuthenticationValues;
using System;  // 포톤 챗 인증

public class PhotonChatTest : MonoBehaviourPun, IChatClientListener
{
    private ChatClient chatClient;
    private string userName;
    private string currentChannelName;

    public InputField inputFieldChat;
    public Text outPutText;
    public Text channelText;

    void Start()
    {
        Application.runInBackground = true;
        //유저 닉네임은 서버에 접속 하면 변경이 힘들다
        //테스트 시 유저 구분을 위해 현재 시간을 유저명으로 임시 지정
        userName = DateTime.Now.ToShortTimeString();
        currentChannelName = "Channel 001";
        chatClient = new ChatClient(this);
        //true가 아닌 경우 어플이 백 그라운드로 갈 때 연결이 끊어진다.
        chatClient.UseBackgroundWorkerForSending = true;
        //포톤 챗 연결 
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, "1.0", new AuthenticationValues(userName));
        AddLine(string.Format("연결시도", userName));
    }

    public void AddLine(string llineString)
    {
        channelText.text += llineString + "\r\n";
    }

    public void OnApplicationQuit() // 앱이 종료될 때 호출
    {
        if(chatClient != null)
            chatClient.Disconnect();
    }

    public void DebugReturn(DebugLevel level, string message)   // DebugReturn에 정의된 enum 타입에 따라 메세지를 출력
    {
        if (level == ExitGames.Client.Photon.DebugLevel.ERROR)
        {
            Debug.LogError(message);
        }
        else if (level == ExitGames.Client.Photon.DebugLevel.WARNING)
        {
            Debug.LogWarning(message);
        }
        else
        {
            Debug.Log(message);
        }
    }    

    public void OnConnected()   // 서버 연결을 성공
    {
        AddLine("서버에 연결 되었습니다.");
        chatClient.Subscribe(new string[] { currentChannelName }, 10);  // 지정한 채널명으로 접속
    }

    public void OnDisconnected()    // 서버와의 연결이 끊어졌다면
    {
        AddLine("서버와의 연결이 끊어졌습니다.");
    }

    public void OnChatStateChange(ChatState state)  // 현재 클라이언트의 상태를 출력
    {
        Debug.Log("OnChatStateChange = " + state);
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        AddLine(string.Format("채널입장 ({0})", string.Join(",", channels)));
    }

    public void OnUnsubscribed(string[] channels)
    {
        AddLine(string.Format("채널퇴장 ({0})", string.Join(",", channels)));
    }

    // Update()에서 chatClient.Service()가 호출 시 OnGetMessages 메서드를 호출한다.
    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        if (channelName.Equals(currentChannelName))
        {
            this.ShowChannel(currentChannelName);
        }
    }

    public void ShowChannel(string channelName)
    {
        if (string.IsNullOrEmpty(channelName)) return;
        ChatChannel channel = null;
        bool found = this.chatClient.TryGetChannel(channelName, out channel);
        if (!found)
        {
            Debug.Log("ShowChannel failed to find Channel : " + channelName);
            return;
        }
        this.currentChannelName = channelName;
        // 채널에 저장된 모든 채팅 메세지를 불러온다.
        // 유저 이름과 채팅내용이 한꺼번에 불러와진다.
        this.outPutText.text = channel.ToStringMessages();
        Debug.Log("ShowChannel : " + currentChannelName);
    }

    // 개인 메세지를 보낼 경우 사용하는 메서드
    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        Debug.Log("OnPrivateMessage : " + message);
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        Debug.Log("status : " + string.Format("{0} is {1}, Msg : {2} ", user, status, message));
    }

    // 포톤엔진 공식 홈페이지에 기술되어있다.
    // chatClient.Service()를 Update에서 호출하던지, 필요에 따라 chatClient.Service()를 반드시 호출해야한다.
    void Update()
    {
        chatClient.Service();
    }

    public void OnUserSubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }

    // 인스펙터의 InputField에서 입력받은 메세지를 보내기 위해 메서드를 구현
    public void OnEnterSend()
    {
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
        {
            this.SendChatMessage(this.inputFieldChat.text);
            this.inputFieldChat.text = "";
        }
    }

    // OnEnterSend()에서 호출 받을 메서드 구현
    void SendChatMessage(string inputLine)
    {
        if (string.IsNullOrEmpty(inputLine)) return;
        this.chatClient.PublishMessage(currentChannelName, inputLine);
    }
}
