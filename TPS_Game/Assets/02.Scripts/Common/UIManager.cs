using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void OnClickStartBtn()
    {
        // SceneLoad시 LevelScene과 MainScene을 합친다.
        //SceneManager.LoadScene("LevelScene");
        //SceneManager.LoadScene("MainScene", LoadSceneMode.Additive);
        SceneManager.LoadScene("FadeInScene");  // FadeInScene에서 씬 병합을 하였으므로 해당 씬을 호출
    }
    public void OnClickQuitBtn()
    {
#if UNITY_EDITOR    //에디터에서만 명령 실행하는 매크로(전처리기)
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
