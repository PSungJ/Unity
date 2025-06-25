using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    public bool isPlay = false;

    public void GameStart()
    {
        isPlay = true;
        SceneManager.LoadScene("PlayScene");    //유니티의 지정한 Scene으로 변경
    }
    public void GameQuit()
    {
#if UNITY_EDITOR    //매크로 지정 : 컴파일러가 에디터에서 실행중인지 확인
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
