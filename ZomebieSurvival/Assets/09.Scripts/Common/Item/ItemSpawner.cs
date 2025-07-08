using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class ItemSpawner : MonoBehaviour
{
    public GameObject[] items;  // 생성할 아이템
    public Transform playerTr;
    public float maxDistance = 5f;  // 플레이어 위치로부터 아이템이 배치될 최대 반경
    public float timeBetSpawnMax = 7f;  // 최대 시간 간격
    public float timeBetSpawnMin = 2f;  // 최소 시간 간격
    private float timeBetSpawn; // 생성 간격
    private float lastSpawnTime; // 마지막 생성 시점

    void Start()    // 생생 간격과 마지막 생성 시점 초기화
    { 
        timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
        lastSpawnTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // 현재 시점이 마지막 생성 시점에서 생성주기 이상으로 지나고 플레이어가 살아있을때
        if(Time.time >= lastSpawnTime + timeBetSpawn && playerTr != null)
        {
            lastSpawnTime = Time.time;      // 마지막 생성시간 갱신
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);  // 생성 주기를 랜덤으로 변경
            Spawn();    // 아이템 생성 실행
        }
    }
    private void Spawn()    // 아이템 생성 처리
    {
        // 플레이어 근처에서 NavMesh 위의 랜덤위치 호출
        Vector3 spawnPosition = GetRandomPointOnNavMesh(playerTr.position, maxDistance);
        spawnPosition += Vector3.up * 0.5f; // 바닥에서 0.5 높이만큼 위로 올리기
        // 아이템 중 하나를 무작위로 골라 랜덤 위치에 생성
        GameObject selectedItem = items[Random.Range(0, items.Length)];
        GameObject item = Instantiate(selectedItem, spawnPosition, Quaternion.identity);
        Destroy(item, 5f);  // 5초뒤에 아이템 파괴
    }

    // NavMesh 위의 랜덤한 위치를 반환하는 메서드
    // center를 중심으로 distance 반경 안에서 랜덤한 위치를 찾음
    private Vector3 GetRandomPointOnNavMesh(Vector3 center, float distance)     // center를 중심으로 반지름이 maxDistance인 구 안에서의 랜덤한 위치 하나를 저장
    {
        // Random.insideUnitSphere는 반지름이 1인 구 안에서의 랜덤한 한 점을 반환하는 프로퍼티
        Vector3 randomPos = Random.insideUnitSphere * distance + center;
        // NavMesh 샘플링의 결과 정보를 저장하는 변수
        NavMeshHit hit;
        // maxDistance 반경안에서, randomPos에 가장 가까운 NavMesh 위의 한 점을 찾음
        NavMesh.SamplePosition(randomPos, out hit, distance, NavMesh.AllAreas);
        // 찾은 점을 반환
        return hit.position;
    }
}
