using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Coin : MonoBehaviourPun, I_Item
{
    public int score = 200; // 증가할 점수
    public void Use(GameObject target)
    {
        GameManager.instance.AddScore(score);   // 게임매니저로 접근하여 점수 증가
        PhotonNetwork.Destroy(gameObject);    // 사용 후 파괴
    }
}
