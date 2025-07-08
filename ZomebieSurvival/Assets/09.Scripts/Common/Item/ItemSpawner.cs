using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class ItemSpawner : MonoBehaviour
{
    public GameObject[] items;  // ������ ������
    public Transform playerTr;
    public float maxDistance = 5f;  // �÷��̾� ��ġ�κ��� �������� ��ġ�� �ִ� �ݰ�
    public float timeBetSpawnMax = 7f;  // �ִ� �ð� ����
    public float timeBetSpawnMin = 2f;  // �ּ� �ð� ����
    private float timeBetSpawn; // ���� ����
    private float lastSpawnTime; // ������ ���� ����

    void Start()    // ���� ���ݰ� ������ ���� ���� �ʱ�ȭ
    { 
        timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
        lastSpawnTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // ���� ������ ������ ���� �������� �����ֱ� �̻����� ������ �÷��̾ ���������
        if(Time.time >= lastSpawnTime + timeBetSpawn && playerTr != null)
        {
            lastSpawnTime = Time.time;      // ������ �����ð� ����
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);  // ���� �ֱ⸦ �������� ����
            Spawn();    // ������ ���� ����
        }
    }
    private void Spawn()    // ������ ���� ó��
    {
        // �÷��̾� ��ó���� NavMesh ���� ������ġ ȣ��
        Vector3 spawnPosition = GetRandomPointOnNavMesh(playerTr.position, maxDistance);
        spawnPosition += Vector3.up * 0.5f; // �ٴڿ��� 0.5 ���̸�ŭ ���� �ø���
        // ������ �� �ϳ��� �������� ��� ���� ��ġ�� ����
        GameObject selectedItem = items[Random.Range(0, items.Length)];
        GameObject item = Instantiate(selectedItem, spawnPosition, Quaternion.identity);
        Destroy(item, 5f);  // 5�ʵڿ� ������ �ı�
    }

    // NavMesh ���� ������ ��ġ�� ��ȯ�ϴ� �޼���
    // center�� �߽����� distance �ݰ� �ȿ��� ������ ��ġ�� ã��
    private Vector3 GetRandomPointOnNavMesh(Vector3 center, float distance)     // center�� �߽����� �������� maxDistance�� �� �ȿ����� ������ ��ġ �ϳ��� ����
    {
        // Random.insideUnitSphere�� �������� 1�� �� �ȿ����� ������ �� ���� ��ȯ�ϴ� ������Ƽ
        Vector3 randomPos = Random.insideUnitSphere * distance + center;
        // NavMesh ���ø��� ��� ������ �����ϴ� ����
        NavMeshHit hit;
        // maxDistance �ݰ�ȿ���, randomPos�� ���� ����� NavMesh ���� �� ���� ã��
        NavMesh.SamplePosition(randomPos, out hit, distance, NavMesh.AllAreas);
        // ã�� ���� ��ȯ
        return hit.position;
    }
}
