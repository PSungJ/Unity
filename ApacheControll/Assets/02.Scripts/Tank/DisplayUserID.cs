using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class DisplayUserID : MonoBehaviourPun
{
    public Text userID;

    void Start()
    {
        if (photonView != null)
        {
            userID.text = photonView.Owner.NickName;
        }
    }
}
