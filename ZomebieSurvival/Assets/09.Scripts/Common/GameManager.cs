using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    private static GameManager m_instance;
    public static GameManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<GameManager>();

            return m_instance;
        }
    }
    private int score = 0;
    public bool isGameOver { get; private set; }
    public GameObject playerPrefab;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
            stream.SendNext(score);
        else
        {
            score = (int)stream.ReceiveNext();
            UIManager.UI_instance.UpdateScoreText(score);
        }
    }

    private void Awake()
    {
        if (instance != this)   // 씬에 싱글톤 오브젝트가 된 다른 GameManager오브젝트가 있다면 자신을 파괴
            Destroy(gameObject);     
    }

    private void Start()
    {
        CreatePlayer();
    }

    void CreatePlayer()
    {
        Vector3 randomSpawnPos = Random.insideUnitSphere * 5f;
        randomSpawnPos.y = 0;
        PhotonNetwork.Instantiate(playerPrefab.name, randomSpawnPos, Quaternion.identity);

        FindAnyObjectByType<WomanHealth>().onDeath += EndGame;
    }

    public void AddScore(int newScore)
    {
        if (!isGameOver)
        {
            score += newScore;
            UIManager.UI_instance.UpdateScoreText(score);
        }
    }

    public void EndGame()
    {
        isGameOver = true;
        UIManager.UI_instance.SetActiveGameOverUI(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            PhotonNetwork.LeaveRoom();
    }
    
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("LobbyScene");   // 룸을 빠져나가면서 네트워크 객체는 자동 삭제
    }
}
