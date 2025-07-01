using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void OnClickStartBtn()
    {
        // SceneLoad�� LevelScene�� MainScene�� ��ģ��.
        //SceneManager.LoadScene("LevelScene");
        //SceneManager.LoadScene("MainScene", LoadSceneMode.Additive);
        SceneManager.LoadScene("FadeInScene");  // FadeInScene���� �� ������ �Ͽ����Ƿ� �ش� ���� ȣ��
    }
    public void OnClickQuitBtn()
    {
#if UNITY_EDITOR    //�����Ϳ����� ��� �����ϴ� ��ũ��(��ó����)
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
