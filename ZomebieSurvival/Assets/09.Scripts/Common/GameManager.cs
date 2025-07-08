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
        if (instance != this)   // ���� �̱��� ������Ʈ�� �� �ٸ� GameManager������Ʈ�� �ִٸ� �ڽ��� �ı�
            Destroy(gameObject);
    }

    private void Start()
    {
        // ���� �ִ� ��� ���� ������Ʈ�� ��ȸ�ϸ� �˻��ϱ� ������ �ſ������.
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
