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

    public Text ammoText;   // źâǥ�� �ؽ�Ʈ
    public Text scoreText;  // ����ǥ�� �ؽ�Ʈ
    public Text waveText;   // ���̺� ǥ�� �ؽ�Ʈ
    public GameObject gameOverUI;   // ���ӿ����� Ȱ��ȭ�� UI

    public void UpdateAmmoText(int magAmmo, int remainAmmo) // źâ �ؽ�Ʈ ����
    {
        ammoText.text = $"{magAmmo}/{remainAmmo}";
    }
    
    public void UpdateScoreText(int newScore)   // ���� �ؽ�Ʈ ����
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
