using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null; //싱글톤 패턴 : 객체생성은 한번만 되고 다른 클래스에서 쉽게 접근
    //public GameObject asteroidPrefab; //asteroid prefab 생성
    private float timePreve; //시간 저장
    public delegate void ShakeHandler();
    public static event ShakeHandler OnShake;
    // 주석: 개발자가 쓰고 개발자가 읽음
    // Attribute속성 : 개발자가 쓰고 유니티가 읽어서 실행
    [Header("bool GameOver")]           //Attribute 속성
    public bool isGameOver = false;     //게임오버 여부
    [Header("CameraShake Logic Member")]
    public bool isShake = false;        //카메라 흔들림 여부
    public Vector3 PosCamera;          //카메라 위치 저장
    public float beginTime;            //카메라 흔들림 시작시간
    [Header("HPBar UI")]
    public int hp;
    public int maxHP = 100;
    public Image hpBar;
    public Text hpText;
    [Header("GameOver UI")]
    public GameObject gameoverObj;
    public Text scoreTxt;
    private float curScore = 0; //현재 점수
    private float plusScore = 1f;    //점수 증가량

    void Start()
    {
        if (instance == null)
            instance = this;    //this = GetComponent<GameManager>();
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject); // 씬이 바뀌어도 파괴되지 않는다.
        timePreve = Time.time;  //게임 시작 전 현재시간을 저장
        hp = maxHP;
    }

     void Update()
    {       //현재시간 - 지난시간 = 지나간 시간
        if (Time.time - timePreve > 2.5f && !isGameOver)
        {
            AsteroidSpawn();
        }

        //if (isShake == true)
        //{
        //    float x = Random.Range(-0.1f, 0.1f);
        //    float y = Random.Range(-0.1f, 0.1f);
        //    Camera.main.transform.position += new Vector3(x, y, 0f);

        //    if (Time.time - beginTime > 0.3f)
        //    {
        //        isShake = false;    //카메라 흔들림 종료
        //        Camera.main.transform.position = PosCamera; //카메라 원래 위치로 복귀
        //    }
        //}
        ScoreCount();
    }

    private void ScoreCount()
    {
        curScore += plusScore * Time.realtimeSinceStartup; // 점수 증가
        //Time.realtimeSinceStartup : 게임이 시작한 이후의 시간을 초단위로 반환 readonly속성
        scoreTxt.text = $"{Mathf.FloorToInt(curScore)}"; //점수 UI 갱신
                                                         //Mathf.FloorToInt() float값보다 작거나 같은 정수를 반환
                                                         //Mathf.FloorToInt(3.7) 인 경우 정수 3을 반환
                                                         //Mathf.FloorToInt(-3.2) 인 경우 정수 -4을 반환
    }

    public void TurnOn()
    {
        //isShake = true; //카메라 흔들림 시작
        //PosCamera = Camera.main.transform.position; //카메라가 흔들리기 전에 위치값 저장
        //beginTime = Time.time;  //카메라가 흔들리기 시작한 시간 저장
        OnShake();
    }
    private void AsteroidSpawn()
    {
        //float randomY = Random.Range(-3.7f, 3.7f); //랜덤의 Y좌표 설정
        var asteroid = AsteroidPool.p_instance.GetAst();
        if (asteroid != null)
        {
            asteroid.transform.position = new Vector2(12f, Random.Range(-3.7f, 3.7f));
            asteroid.SetActive(true);
        }
        //Instantiate(asteroidPrefab, new Vector3(12f, randomY, asteroidPrefab.transform.position.z), Quaternion.identity);// 회전하지 않고 생성
        //Instantiate : 오브젝트 생성 함수(what?, where?, how 회전 할 것인가?)
        timePreve = Time.time; // 현재시간 저장
    }
    public void RocketHealthPoint(int damage)
    {
        hp -= damage;
        hp = Mathf.Clamp(hp, 0, 100); // 체력을 0~100으로 제한
        hpBar.fillAmount = (float)hp / (float)maxHP;
        hpText.text = $"HP : <color=#ff0000>{hp}</color>";
        if (hp <= 0)
        {
            isGameOver = true;
            gameoverObj.SetActive(true);
            Invoke("LobbySceneMove", 3.0f);
            //스트링 문자를 읽어서 원하는 시간에 호출하는 함수 - Invoke
        }
    }
    public void LobbySceneMove()
    {
        SceneManager.LoadScene("LobbyScene");
    }
}
