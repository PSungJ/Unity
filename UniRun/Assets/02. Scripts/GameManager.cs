using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
// 싱글톤 패턴 사용
// 1. 단일 오브젝트 : 객체 생성은 한번만하고
// 2. 전역 접근 : 다른 스크립트에서 쉽게 접근할 수 있도록
// 3. 전역 상태 유지 : 게임이 끝나도 오브젝트가 파괴되지 않도록 한다.
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Text scoreUI;
    public GameObject gameoverUI;
    public bool isGameOver = false;
    private int score = 0;

    void Awake()    //Start()보다 먼저 실행
    {
        if (instance == null)               //instance가 null이라면(아직 생성되지 않음)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
            //SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (instance != this)          //instance가 null이 아니고, 현재 오브젝트가 instance와 다르다면
        {
            Debug.LogWarning("씬에 두개 이상의 게임매니저가 존재합니다.");
            Destroy(gameObject);
        }
    }
    void OnEnable()
    {
        scoreUI = GameObject.Find("Canvas-UI").GetComponentsInChildren<Text>()[0];
        gameoverUI = GameObject.Find("Canvas-UI").transform.GetChild(1).gameObject;
    }
    void Update()
    {
        if (isGameOver && Input.GetMouseButtonDown(0))  // 게임오버 + 마우스 좌클릭 감지
        {
            // 씬 재시작
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            isGameOver = false;
        }
    }
    private void OnDestroy()
    {
        // 씬 로드 이벤트 제거 (메모리 누수 방지)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬 로드될 때마다 다시 UI 찾기
        var canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            scoreUI = canvas.transform.GetChild(0).GetComponent<Text>();
            gameoverUI = canvas.transform.GetChild(1).gameObject;
        }
    }
    public void AddScore(int newScore) // 점수 추가 함수
    {
        if (!isGameOver)
        {
            score += newScore;
            scoreUI.text = $"Score : {score}";
        }
    }
    public void OnPlayerDie()
    {
        isGameOver = true;
        gameoverUI.SetActive(true);
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("종료");
    }
}
