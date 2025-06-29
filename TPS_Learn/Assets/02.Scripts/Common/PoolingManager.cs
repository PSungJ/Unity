using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager p_instance= null;
    [Header("Object Player bullet Pool")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int maxPool =10; //풀링할 최대  갯수
    [SerializeField] private List<GameObject> bulletPool = new List<GameObject>();
    [Header("Object Enemy bullet Pool")]
    [SerializeField] private GameObject E_bulletPrefab;
    [SerializeField] private int E_maxPool = 20; //풀링할 최대  갯수
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
        else if (p_instance != this) //p_instance 할당된 클래스의 인스턴스가 다를 경우 새로생성된 클래스를 의미함
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
            enemy.name =$"{i+1} : 명";
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
            bullet.name = $"총알 {i + 1}발";
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }
    }
    public GameObject GetBullet()
    {
        for (int i = 0; i < bulletPool.Count; i++)
        {                     //활성화인지 비활성화인지 자동체크
            if (bulletPool[i].activeSelf == false)
            {
                return bulletPool[i]; //비활성화 된 것만 반환
            }
        }
        return null; //활성화 되었다면 null 반환
    }
    private void CreateE_Bullet()
    {
        GameObject E_objectPools = new GameObject("E_ObjectPools");
        for (int i = 0; i < E_maxPool; i++)
        {
            var E_Bullet = Instantiate(E_bulletPrefab, E_objectPools.transform);
            E_Bullet.name = $"총알 {i + 1}발";
            E_Bullet.SetActive(false);
            E_bulletPool.Add(E_Bullet);
        }
    }
    public GameObject GetE_Bullet()
    {
        for (int i = 0; i < E_bulletPool.Count; i++)
        {                     //활성화인지 비활성화인지 자동체크
            if (E_bulletPool[i].activeSelf == false)
            {
                return E_bulletPool[i]; //비활성화 된 것만 반환
            }
        }
        return null; //활성화 되었다면 null 반환
    }
    void CreateEnemyHpbarPooling()
    {
        for(int i = 0; i<maxPool; i++)
        {
            var _hpBar = Instantiate(hpBarprefab,uiCanvas.transform);
            _hpBar.name = $"{i + 1}번째 enemyHpbar";
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
