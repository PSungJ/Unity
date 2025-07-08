using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
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

    //public WomanHealth womanHealth;

    private void Awake()
    {
        if (instance != this)   // 씬에 싱글톤 오브젝트가 된 다른 GameManager오브젝트가 있다면 자신을 파괴
            Destroy(gameObject);
    }

    private void Start()
    {
        // 씬에 있는 모든 게임 오브젝트를 순회하며 검색하기 때문에 매우느리다.
        FindAnyObjectByType<WomanHealth>().onDeath += EndGame;

        //womanHealth.onDeath += EndGame;
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
}
