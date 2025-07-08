using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public Zombie zombiePrefab;         // 생성할 좀비 프리팹
    public ZombieData[] zombieDatas;    // 사용할 좀비 셋업 데이터
    public List<Transform> spawnPointList;     // 좀비를 소환할 위치
    
    private List<Zombie> zombieList = new List<Zombie>();  // 생성된 좀비를 담는 리스트
    private int wave;   // 현재 웨이브 단계

    private void Start()
    {
        //var spawnPoints = GameObject.Find("Spawn Points");
        //if (spawnPoints != null)
        //{
        //    spawnPoints.GetComponentsInChildren<Transform>(spawnPointList);
        //}
        //spawnPointList.RemoveAt(0);

        zombieDatas = Resources.LoadAll<ZombieData>("ZombieData");
    }

    void Update()
    {
        // 게임오버 상태라면 실행하지 않음
        if (GameManager.instance != null && GameManager.instance.isGameOver)
            return;

        // 좀비를 모두 물리친 경우 다음 스폰 실행
        if (zombieList.Count <= 0)
            SpawnWave();

        UpdateUI();
    }

    private void UpdateUI() // 웨이브 정보를 UI에 표시
    {
        UIManager.UI_instance.UpdateWaveText(wave, zombieList.Count);
    }

    private void SpawnWave()    // 현재 웨이브에 맞춰 좀비 생성
    {
        wave++;
        // 현재 웨이브에 * 1.5를 반올림한 수만 큼 좀비 생성
        int spawnCount = Mathf.RoundToInt(wave * 1.5f);

        // 웨이브에 따라 생성할 좀비 수 증가
        for (int i = 0; i < spawnCount; i++)
        {
            CreateZombie();
        }
    }

    private void CreateZombie() // 좀비를 생성하고 생성한 좀비에게 추적대상 할당
    {
        ZombieData zombieData = zombieDatas[Random.Range(0, zombieDatas.Length)];       // 좀비 랜덤 생성
        Transform spawnPoint = spawnPointList[Random.Range(0, spawnPointList.Count)];   // 스폰 위치 랜덤
        Zombie zombie = Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation);    // 좀비 생성
        zombie.SetUp(zombieData);   // 좀비 스펙 설정
        zombieList.Add(zombie);     // 생성한 좀비를 리스트에 추가
        // 람다식으로 익명 메서드 등록
        zombie.onDeath += () => zombieList.Remove(zombie);  // 좀비가 죽으면 리스트에서 제거
        zombie.onDeath += () => Destroy(zombie.gameObject, 10f);    // 좀비가 죽으면 게임 오브젝트 파괴
        zombie.onDeath += () => GameManager.instance.AddScore(100); // 좀비가 죽으면 점수 추가
    }
}
