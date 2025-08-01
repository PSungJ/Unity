using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIScript : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.MousePointerVisible();
    }
    public void PlaySceneMove()
    {
        //SceneManager.LoadScene("LevelScene");
        //SceneManager.LoadScene("MainScene", LoadSceneMode.Additive);
        SceneManager.LoadScene("FadeInScene"); 
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
