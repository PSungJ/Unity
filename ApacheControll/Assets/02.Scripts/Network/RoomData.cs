using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class RoomData : MonoBehaviourPun
{
    public string roomName = string.Empty;
    public int connectPlayer = 0;
    public int maxPlayer = 0;
    public Text txtRoomName;
    public Text txtconnectInfo;

    public void DisplayRoomData()
    {
        txtRoomName.text = roomName;
        txtconnectInfo.text = $"({connectPlayer.ToString()}/{maxPlayer.ToString()})";
    }
}
