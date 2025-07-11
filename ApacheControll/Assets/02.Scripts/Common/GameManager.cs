using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public bool isGameOver = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
        // Photon Network �����κ��� ���� �޽����� �޴´�.
        PhotonNetwork.IsMessageQueueRunning = true;
        CreateTank();
    }
    void CreateTank()
    {
        float pos = Random.Range(-100f, 100f);
        PhotonNetwork.Instantiate("Tank", new Vector3(pos, 3f, pos), Quaternion.identity, 0, null);
    }
}
