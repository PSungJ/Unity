using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public Zombie zombiePrefab;         // ������ ���� ������
    public ZombieData[] zombieDatas;    // ����� ���� �¾� ������
    public List<Transform> spawnPointList;     // ���� ��ȯ�� ��ġ
    
    private List<Zombie> zombieList = new List<Zombie>();  // ������ ���� ��� ����Ʈ
    private int wave;   // ���� ���̺� �ܰ�

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
        // ���ӿ��� ���¶�� �������� ����
        if (GameManager.instance != null && GameManager.instance.isGameOver)
            return;

        // ���� ��� ����ģ ��� ���� ���� ����
        if (zombieList.Count <= 0)
            SpawnWave();

        UpdateUI();
    }

    private void UpdateUI() // ���̺� ������ UI�� ǥ��
    {
        UIManager.UI_instance.UpdateWaveText(wave, zombieList.Count);
    }

    private void SpawnWave()    // ���� ���̺꿡 ���� ���� ����
    {
        wave++;
        // ���� ���̺꿡 * 1.5�� �ݿø��� ���� ŭ ���� ����
        int spawnCount = Mathf.RoundToInt(wave * 1.5f);

        // ���̺꿡 ���� ������ ���� �� ����
        for (int i = 0; i < spawnCount; i++)
        {
            CreateZombie();
        }
    }

    private void CreateZombie() // ���� �����ϰ� ������ ���񿡰� ������� �Ҵ�
    {
        ZombieData zombieData = zombieDatas[Random.Range(0, zombieDatas.Length)];       // ���� ���� ����
        Transform spawnPoint = spawnPointList[Random.Range(0, spawnPointList.Count)];   // ���� ��ġ ����
        Zombie zombie = Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation);    // ���� ����
        zombie.SetUp(zombieData);   // ���� ���� ����
        zombieList.Add(zombie);     // ������ ���� ����Ʈ�� �߰�
        // ���ٽ����� �͸� �޼��� ���
        zombie.onDeath += () => zombieList.Remove(zombie);  // ���� ������ ����Ʈ���� ����
        zombie.onDeath += () => Destroy(zombie.gameObject, 10f);    // ���� ������ ���� ������Ʈ �ı�
        zombie.onDeath += () => GameManager.instance.AddScore(100); // ���� ������ ���� �߰�
    }
}
