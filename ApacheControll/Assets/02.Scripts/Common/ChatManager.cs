using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Chat;
using ExitGames.Client.Photon;
using AuthenticationValues = Photon.Chat.AuthenticationValues;  // ���� ê ����

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
        Application.runInBackground = true;     // ��׶��忡�� �����ǰ��Ѵ�.
        userName = DateTime.Now.ToShortTimeString();    // ���� �ð��� userName���� �ӽü���
        curChName = "Channel 001";
        chatClient = new ChatClient(this);
        chatClient.UseBackgroundWorkerForSending = true;    // true�� �ƴϸ� ���� ��׶���� �� �� ������ ������
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, "1.0", new AuthenticationValues(userName));
        AddLine(string.Format("����õ�", userName));
    }

    public void AddLine(string line)
    {
        channelCheck.text = line + "\r\n";
    }

    public void OnApplicationQuit() // �� ����� ȣ��
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

    public void OnConnected()   // ä�� ���� ���� ����
    {
        AddLine("ä�� ������ ����Ǿ����ϴ�.");
        chatClient.Subscribe(new string[] { curChName }, 10);
    }

    public void OnDisconnected()
    {
        AddLine("ä�� �������� ������ ���������ϴ�.");
    }

    public void OnChatStateChange(ChatState state)  // ���� Ŭ���̾�Ʈ ���� Ȯ��
    {
        Debug.Log("OnChatStateChange = " + state);
    }

    public void OnSubscribed(string[] channels, bool[] results) // ä�� ä�� ����
    {
        AddLine(string.Format("ä�� ���� ({0})", string.Join(",", channels)));
    }

    public void OnUnsubscribed(string[] channels)   // ä�� ä�� ����
    {
        AddLine(string.Format("ä�� ���� ({0})", string.Join(",", channels)));
    }

    // Update()���� chatClient.Service() ȣ�� �� �ش� �޼��� �ڵ� ȣ��
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
        this.outPutText.text = chatChannel.ToStringMessages();  // ���� �̸��� ä���Է³����� �ѹ��� ������
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

    // InputField OnEndEdit ȣ�� �޼���
    public void OnEnter()
    {
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
        {
            this.SendChat(this.inputChat.text);
            this.inputChat.text = "";
        }
    }
    
    // ���ӳ����� EnterŰ �Է� �� OnEnter�� ȣ���ų �޼���
    void SendChat(string inputLine)
    {
        if (string.IsNullOrEmpty(inputLine)) return;
        this.chatClient.PublishMessage(curChName, inputLine);
    }
}
