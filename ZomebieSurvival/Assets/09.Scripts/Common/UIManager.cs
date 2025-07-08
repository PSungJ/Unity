using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private static UIManager m_instance;
    public static UIManager UI_instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<UIManager>();

            return m_instance;
        }
    }

    public Text ammoText;   // 탄창표시 텍스트
    public Text scoreText;  // 점수표시 텍스트
    public Text waveText;   // 웨이브 표시 텍스트
    public GameObject gameOverUI;   // 게임오버시 활성화될 UI

    public void UpdateAmmoText(int magAmmo, int remainAmmo) // 탄창 텍스트 갱신
    {
        ammoText.text = $"{magAmmo}/{remainAmmo}";
    }
    
    public void UpdateScoreText(int newScore)   // 점수 텍스트 갱신
    {
        scoreText.text = $"Score : {newScore}";
    }

    public void UpdateWaveText(int waves, int count)
    {
        waveText.text = $"Wave : {waves}\nEnemyLeft : {count}";
    }

    public void SetActiveGameOverUI(bool active)
    {
        gameOverUI.SetActive(active);
    }

    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
