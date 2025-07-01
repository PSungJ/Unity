using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    public CanvasGroup fadeCG;
    [Range(0.5f,2.0f)] public float fadeDuration = 1f;

    // ȣ�� �� ���� �ε� ����� ������ ��ųʸ�
    public Dictionary<string, LoadSceneMode> loadScene = new Dictionary<string, LoadSceneMode>();
    
    void InitSceneInfo()    // ȣ���� ���� ������ ����
    {
        loadScene.Add("LevelScene", LoadSceneMode.Additive);
        loadScene.Add("MainScene", LoadSceneMode.Additive);
    }
    IEnumerator Start()
    {
        InitSceneInfo();
        fadeCG.alpha = 1f;
        
        // �������� ���� �ڷ�ƾ���� ȣ��
        foreach(var scene in loadScene)
        {
            yield return StartCoroutine(LoadScene(scene.Key, scene.Value));
        }
        StartCoroutine(Fade(0f));
    }
    IEnumerator LoadScene(string sceneName, LoadSceneMode mode)
    {
        // �񵿱������ ���� �ε��ϰ� �ε尡 �Ϸ�� ������ ���
        yield return SceneManager.LoadSceneAsync(sceneName, mode);

        Scene loadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1); // -1�� ������ �迭 �ε����� count�� �迭�� 0����, count�� 1�� �����ϱ� �����̴�
        SceneManager.SetActiveScene(loadedScene);
    }
    IEnumerator Fade(float finalAlpha)  // Fade In Out ��Ű�� �Լ�
    {
        // ����Ʈ���� ������ ���� �����ϱ� ���� ���� �������� �� Ȱ��ȭ
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("LevelScene"));
        fadeCG.blocksRaycasts = true;   //Raycast ���� Ȱ��ȭ

        // Abs ���밪 �Լ��� ��з� ����� ����� ������
        float fadespeed = Mathf.Abs(fadeCG.alpha - finalAlpha) / fadeDuration;  // ���İ��� ���� ���̵�ӵ�
        // ���İ� ����(A, B��) �տ� A,B�� ���� �ʴٸ�
        while (!Mathf.Approximately(fadeCG.alpha, finalAlpha))
        {
            //MoveTowrds�� Lerp�� ���������, ������ ������ �̵��ϴµ� ���
            fadeCG.alpha = Mathf.MoveTowards(fadeCG.alpha, finalAlpha, fadespeed * Time.deltaTime);
            yield return null;  // ���� �����ӱ��� ���
        }
        fadeCG.blocksRaycasts = false;

        SceneManager.UnloadSceneAsync("FadeInScene");   // FadeInScene ��ε�
        //FadeIn�� �Ϸ�Ǹ� SceneLoader������Ʈ ����
    }
}
