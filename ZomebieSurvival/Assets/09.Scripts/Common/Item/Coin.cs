using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Coin : MonoBehaviourPun, I_Item
{
    public int score = 200; // ������ ����
    public void Use(GameObject target)
    {
        GameManager.instance.AddScore(score);   // ���ӸŴ����� �����Ͽ� ���� ����
        PhotonNetwork.Destroy(gameObject);    // ��� �� �ı�
    }
}
