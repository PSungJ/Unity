using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Chat;
using Photon.Realtime;
using ExitGames.Client.Photon;
using AuthenticationValues = Photon.Chat.AuthenticationValues;
using System;  // ���� ê ����

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
        //���� �г����� ������ ���� �ϸ� ������ �����
        //�׽�Ʈ �� ���� ������ ���� ���� �ð��� ���������� �ӽ� ����
        userName = DateTime.Now.ToShortTimeString();
        currentChannelName = "Channel 001";
        chatClient = new ChatClient(this);
        //true�� �ƴ� ��� ������ �� �׶���� �� �� ������ ��������.
        chatClient.UseBackgroundWorkerForSending = true;
        //���� ê ���� 
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, "1.0", new AuthenticationValues(userName));
        AddLine(string.Format("����õ�", userName));
    }

    public void AddLine(string llineString)
    {
        channelText.text += llineString + "\r\n";
    }

    public void OnApplicationQuit() // ���� ����� �� ȣ��
    {
        if(chatClient != null)
            chatClient.Disconnect();
    }

    public void DebugReturn(DebugLevel level, string message)   // DebugReturn�� ���ǵ� enum Ÿ�Կ� ���� �޼����� ���
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

    public void OnConnected()   // ���� ������ ����
    {
        AddLine("������ ���� �Ǿ����ϴ�.");
        chatClient.Subscribe(new string[] { currentChannelName }, 10);  // ������ ä�θ����� ����
    }

    public void OnDisconnected()    // �������� ������ �������ٸ�
    {
        AddLine("�������� ������ ���������ϴ�.");
    }

    public void OnChatStateChange(ChatState state)  // ���� Ŭ���̾�Ʈ�� ���¸� ���
    {
        Debug.Log("OnChatStateChange = " + state);
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        AddLine(string.Format("ä������ ({0})", string.Join(",", channels)));
    }

    public void OnUnsubscribed(string[] channels)
    {
        AddLine(string.Format("ä������ ({0})", string.Join(",", channels)));
    }

    // Update()���� chatClient.Service()�� ȣ�� �� OnGetMessages �޼��带 ȣ���Ѵ�.
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
        // ä�ο� ����� ��� ä�� �޼����� �ҷ��´�.
        // ���� �̸��� ä�ó����� �Ѳ����� �ҷ�������.
        this.outPutText.text = channel.ToStringMessages();
        Debug.Log("ShowChannel : " + currentChannelName);
    }

    // ���� �޼����� ���� ��� ����ϴ� �޼���
    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        Debug.Log("OnPrivateMessage : " + message);
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        Debug.Log("status : " + string.Format("{0} is {1}, Msg : {2} ", user, status, message));
    }

    // ���濣�� ���� Ȩ�������� ����Ǿ��ִ�.
    // chatClient.Service()�� Update���� ȣ���ϴ���, �ʿ信 ���� chatClient.Service()�� �ݵ�� ȣ���ؾ��Ѵ�.
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

    // �ν������� InputField���� �Է¹��� �޼����� ������ ���� �޼��带 ����
    public void OnEnterSend()
    {
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
        {
            this.SendChatMessage(this.inputFieldChat.text);
            this.inputFieldChat.text = "";
        }
    }

    // OnEnterSend()���� ȣ�� ���� �޼��� ����
    void SendChatMessage(string inputLine)
    {
        if (string.IsNullOrEmpty(inputLine)) return;
        this.chatClient.PublishMessage(currentChannelName, inputLine);
    }
}
