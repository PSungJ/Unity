using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class UIManager : MonoBehaviourPun
{
    [Header("NetworkUI")]
    public Text pingTxt;
    private int goodPing = 50;
    private int warnPing = 100;

    void Update()
    {
        NetworkPingView();
    }

    void NetworkPingView()
    {
        if (PhotonNetwork.IsConnected)
        {
            int ping = PhotonNetwork.GetPing();

            if (ping <= goodPing)
                pingTxt.color = Color.green;
            else if (ping <= warnPing)
                pingTxt.color = Color.yellow;
            else
                pingTxt.color = Color.red;

            pingTxt.text = $"{ping}ms";
        }
    }
}
