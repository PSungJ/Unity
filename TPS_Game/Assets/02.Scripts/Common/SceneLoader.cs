using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    public CanvasGroup fadeCG;
    [Range(0.5f,2.0f)] public float fadeDuration = 1f;

    // 호출 할 씬과 로드 방식을 저장할 딕셔너리
    public Dictionary<string, LoadSceneMode> loadScene = new Dictionary<string, LoadSceneMode>();
    
    void InitSceneInfo()    // 호출할 씬의 정보를 설정
    {
        loadScene.Add("LevelScene", LoadSceneMode.Additive);
        loadScene.Add("MainScene", LoadSceneMode.Additive);
    }
    IEnumerator Start()
    {
        InitSceneInfo();
        fadeCG.alpha = 1f;
        
        // 여러개의 씬을 코루틴으로 호출
        foreach(var scene in loadScene)
        {
            yield return StartCoroutine(LoadScene(scene.Key, scene.Value));
        }
        StartCoroutine(Fade(0f));
    }
    IEnumerator LoadScene(string sceneName, LoadSceneMode mode)
    {
        // 비동기식으로 씬을 로드하고 로드가 완료될 때까지 대기
        yield return SceneManager.LoadSceneAsync(sceneName, mode);

        Scene loadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1); // -1인 이유는 배열 인덱스와 count가 배열은 0부터, count는 1로 시작하기 때문이다
        SceneManager.SetActiveScene(loadedScene);
    }
    IEnumerator Fade(float finalAlpha)  // Fade In Out 시키는 함수
    {
        // 라이트맵이 깨지는 것을 방지하기 위해 씬이 스테이지 씬 활성화
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("LevelScene"));
        fadeCG.blocksRaycasts = true;   //Raycast 차단 활성화

        // Abs 절대값 함수로 백분률 계산이 양수만 나오게
        float fadespeed = Mathf.Abs(fadeCG.alpha - finalAlpha) / fadeDuration;  // 알파값에 따른 페이드속도
        // 알파값 조정(A, B비교) 앞에 A,B가 같지 않다면
        while (!Mathf.Approximately(fadeCG.alpha, finalAlpha))
        {
            //MoveTowrds는 Lerp와 비슷하지만, 지정된 값으로 이동하는데 사용
            fadeCG.alpha = Mathf.MoveTowards(fadeCG.alpha, finalAlpha, fadespeed * Time.deltaTime);
            yield return null;  // 다음 프레임까지 대기
        }
        fadeCG.blocksRaycasts = false;

        SceneManager.UnloadSceneAsync("FadeInScene");   // FadeInScene 언로드
        //FadeIn이 완료되면 SceneLoader오브젝트 삭제
    }
}
