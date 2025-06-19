using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Enemy Spawn
    //1. 스폰 위치 지정
    //2. 스폰 시간
    //3. 스폰 종류
    public static GameManager Instance;

    public GameObject zombieprefab;
    public GameObject skeletonprefab;
    
    public List<Transform> spawnList;

    public Text killTxt;
    private float timePrev;
    private float timePrev2;
    private int maxZombieCount = 10;
    private int maxSkeletonCount = 5;
    public int totalkill = 0;
    private readonly string zombieTag = "ZOMBIE";
    private readonly string skeletonTag = "SKELETON";

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        MousePointerVisible();
        if(Instance != null)
            killTxt = GameObject.Find("Panel-Kill").transform.GetChild(0).GetComponent<Text>();
        else
            killTxt = null;

            timePrev = Time.time;   // 좀비 생성시간 초기화
        timePrev2 = Time.time;  // 스켈레톤 생성시간 초기화
        Transform[] spawnPoints = GameObject.Find("SpwnPoint").GetComponentsInChildren<Transform>();
        if (spawnPoints != null)
            spawnList = new List<Transform>(spawnPoints);
        spawnList.RemoveAt(0);
    }

    void Update()
    {
        if (Time.time -  timePrev >= 3f)
        {
            timePrev = Time.time;
            int zombieCount = GameObject.FindGameObjectsWithTag(zombieTag).Length;
            if (zombieCount < maxZombieCount)
                CreateZombie();
        }
        if (Time.time - timePrev2 >= 5f)
        {
            timePrev2 = Time.time;
            int skeletonCount = GameObject.FindGameObjectsWithTag(skeletonTag).Length;
            if (skeletonCount < maxSkeletonCount)
                CreateSkeleton();
        }
    }
    void CreateZombie()
    {
            int idx = Random.Range(0, spawnList.Count); // 랜덤한 인덱스 생성
            Instantiate(zombieprefab, spawnList[idx].position, spawnList[idx].rotation);
            //프리팹 생성함수 (What, Where, Rotation)
    }
    void CreateSkeleton()
    {
            int idx = Random.Range(0, spawnList.Count); // 랜덤한 인덱스 생성
            Instantiate(skeletonprefab, spawnList[idx].position, spawnList[idx].rotation);
    }
    public void UpdateKillCount(int killCount)
    {
        totalkill += killCount;
        killTxt.text = $"<color=#ff0000>{totalkill.ToString()}</color> Kill";
    }
    public void MousePointerVisible()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void MousePointerDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
