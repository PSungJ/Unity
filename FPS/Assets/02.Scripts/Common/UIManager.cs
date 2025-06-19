using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Text killscore;
    void Start()
    {
        killscore.text = $"<size=30>Score</size>\r\n<color=#ff0000>{GameManager.Instance.totalkill}</color> Kill";
        // 씬 전환 시 마우스커서 표시
        GameManager.Instance.MousePointerVisible();
    }
    public void PlaySceneMove()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
