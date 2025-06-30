using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager p_instance= null;
    [Header("Object Player bullet Pool")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int maxPool =10; //Ǯ���� �ִ�  ����
    [SerializeField] private List<GameObject> bulletPool = new List<GameObject>();
    [Header("Object Enemy bullet Pool")]
    [SerializeField] private GameObject E_bulletPrefab;
    [SerializeField] private int E_maxPool = 20; //Ǯ���� �ִ�  ����
    [SerializeField] private List<GameObject> E_bulletPool = new List<GameObject>();
    [Header("EnemyObject Pool")]
    public List<Transform> SpawnList;
    public GameObject enemyPrefab;
    public List<GameObject> enemyPool;
    [Header("EnemyHpBar")]
    [SerializeField] Canvas uiCanvas;
    [SerializeField] GameObject hpBarprefab;
    [SerializeField] List<GameObject> hpBarPool;

    void Awake()
    {
        if (p_instance == null)
            p_instance = this;
        else if (p_instance != this) //p_instance �Ҵ�� Ŭ������ �ν��Ͻ��� �ٸ� ��� ���λ����� Ŭ������ �ǹ���
            Destroy(this.gameObject);

        var spawnPos = GameObject.Find("SpawnPoint").gameObject;
        if (spawnPos != null)
            spawnPos.GetComponentsInChildren<Transform>(SpawnList);
        SpawnList.RemoveAt(0);


        CreateBullet();
        CreateE_Bullet();
        StartCoroutine(CreateEnemyPooling());
        
    }
    private void Start()
    {
        
        if (GameManager.Instance.isGameOver == false)
            InvokeRepeating("EnemySpawn", 0.02f, 3.0f);

        hpBarprefab = Resources.Load<GameObject>("UI/EnemyHpBar");
        CreateEnemyHpbarPooling();
    }
    IEnumerator CreateEnemyPooling()
    {
        yield return new WaitForSeconds(0.5f);
        var EnemyGroup = new GameObject("EnemyGroup");
        for(int i =0; i < 10; i++)
        {
            var enemy = Instantiate(enemyPrefab,EnemyGroup.transform);
            enemy.name =$"{i+1} : ��";
            enemy.SetActive(false);
            enemyPool.Add(enemy);
        }
    }
    public void EnemySpawn()
    {
        foreach (var _enemy in enemyPool)
        {
            if (GameManager.Instance.isGameOver) break;
            if(_enemy.activeSelf==false)
            {
                _enemy.transform.position = SpawnList[Random.Range(0,SpawnList.Count)].transform.position;
                _enemy.transform.rotation = SpawnList[Random.Range(0,SpawnList.Count)].transform.rotation;
                _enemy.gameObject.SetActive(true);
                break;

            }

        }


    }

    private void CreateBullet()
    {
        GameObject objectPools = new GameObject("ObjectPools");
        for (int i = 0; i < maxPool; i++)
        {
            var bullet = Instantiate(bulletPrefab,objectPools.transform);
            bullet.name = $"�Ѿ� {i + 1}��";
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }
    }
    public GameObject GetBullet()
    {
        for (int i = 0; i < bulletPool.Count; i++)
        {                     //Ȱ��ȭ���� ��Ȱ��ȭ���� �ڵ�üũ
            if (bulletPool[i].activeSelf == false)
            {
                return bulletPool[i]; //��Ȱ��ȭ �� �͸� ��ȯ
            }
        }
        return null; //Ȱ��ȭ �Ǿ��ٸ� null ��ȯ
    }
    private void CreateE_Bullet()
    {
        GameObject E_objectPools = new GameObject("E_ObjectPools");
        for (int i = 0; i < E_maxPool; i++)
        {
            var E_Bullet = Instantiate(E_bulletPrefab, E_objectPools.transform);
            E_Bullet.name = $"�Ѿ� {i + 1}��";
            E_Bullet.SetActive(false);
            E_bulletPool.Add(E_Bullet);
        }
    }
    public GameObject GetE_Bullet()
    {
        for (int i = 0; i < E_bulletPool.Count; i++)
        {                     //Ȱ��ȭ���� ��Ȱ��ȭ���� �ڵ�üũ
            if (E_bulletPool[i].activeSelf == false)
            {
                return E_bulletPool[i]; //��Ȱ��ȭ �� �͸� ��ȯ
            }
        }
        return null; //Ȱ��ȭ �Ǿ��ٸ� null ��ȯ
    }
    void CreateEnemyHpbarPooling()
    {
        for(int i = 0; i<maxPool; i++)
        {
            var _hpBar = Instantiate(hpBarprefab,uiCanvas.transform);
            _hpBar.name = $"{i + 1}��° enemyHpbar";
            _hpBar .SetActive(false);
             hpBarPool.Add( _hpBar);
        }

    }
    public GameObject GetHpBar()
    {
        foreach (var _hpbar in hpBarPool)
        {
            if (_hpbar.activeSelf == false)
            {
                return _hpbar;
            }
        }
        return null;
    }

}
