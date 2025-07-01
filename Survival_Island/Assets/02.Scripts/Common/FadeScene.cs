using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    public CanvasGroup fadeCG;
    [Range(0.5f, 2.0f)] public float fadeDuration = 1f;

    public Dictionary<string, LoadSceneMode> loadScene = new Dictionary<string, LoadSceneMode>();

    void InitSceneInfo()
    {
        loadScene.Add("LevelScene", LoadSceneMode.Additive);
        loadScene.Add("MainScene", LoadSceneMode.Additive);
    }
    IEnumerator Start()
    {
        InitSceneInfo();
        fadeCG.alpha = 1f;

        foreach (var scene in loadScene)
        {
            yield return StartCoroutine(LoadScene(scene.Key, scene.Value));
        }
        StartCoroutine(Fade(0f));
    }
    IEnumerator LoadScene(string sceneName, LoadSceneMode mode)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, mode);

        Scene loadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(loadedScene);
    }
    IEnumerator Fade(float finalAlpha)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("LevelScene"));
        fadeCG.blocksRaycasts = true;

        float fadespeed = Mathf.Abs(fadeCG.alpha - finalAlpha) / fadeDuration;
        while (!Mathf.Approximately(fadeCG.alpha, finalAlpha))
        {
            fadeCG.alpha = Mathf.MoveTowards(fadeCG.alpha, finalAlpha, fadespeed * Time.deltaTime);
            yield return null;
        }
        fadeCG.blocksRaycasts = false;

        SceneManager.UnloadSceneAsync("FadeInScene");
    }
}
