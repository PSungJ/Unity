using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawn : MonoBehaviour
{
    public GameObject platformPrefab;   // 발판 프리팹
    public int count = 3;   // 발판 생성갯수
    public float timeBetSpawnMin = 1.25f;   // 다음 배치 시간 간격 최소값
    public float timeBetSpawnMax = 2.25f;   // 다음 배치 시간 간격 최대값
    private float timeBetSpawn; // 다음 배치 시간간격 변수

    public float yMin = -3.5f;  // 발판 생성 y좌표 위치 최소값
    public float yMax = 1.5f;   // 발판 생성 y좌표 위치 최대값
    private float xPos = 20f;   // 발판이 생성될 x좌표
    
    private GameObject[] platforms; //미리 생성한 발판들
    private int currentIndex = 0;   // 현재 활성화된 발판의 인덱스
    private Vector2 poolPosition = new Vector2(0f, -25f);   // 풀 위치
    private float lastSpawnTime;    //마지막으로 발판을 생성한 시간

    void Start()
    {
        platformPrefab = Resources.Load("Platform") as GameObject;
        platforms = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            platforms[i] = Instantiate(platformPrefab, poolPosition, Quaternion.identity);
        }
        lastSpawnTime = 0f; // 마지막 배치 시점 초기화
        timeBetSpawn = 0f;  // 다음번 배치까지의 시간 간격을 0으로 초기화
    }

    void Update()
    {
        if (GameManager.instance.isGameOver) return;

        if(Time.time >= lastSpawnTime + timeBetSpawn)
        {
            lastSpawnTime = Time.time;
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);  // 배치 시간 랜덤
            float yPos = Random.Range(yMin, yMax);  // 배치 위치 랜덤
            platforms[currentIndex].SetActive(false);   // 오브젝트 끄면 OnDisable 호출
            platforms[currentIndex].SetActive(true);  // 다시 키면 OnEnable 호출

            platforms[currentIndex].transform.position = new Vector3(xPos, yPos);
            currentIndex++;

            if (currentIndex >= count)
                currentIndex = 0;
        }
    }
}
