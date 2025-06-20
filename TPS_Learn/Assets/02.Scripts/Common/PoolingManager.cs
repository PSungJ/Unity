using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager p_Instance = null;
    [Header("Player Object Pool")]
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private int maxpool = 10;   // 풀링 최대 개수
    [SerializeField] private List<GameObject> bulletPool = new List<GameObject>();
    [Header("Enemy Object Pool")]
    [SerializeField] private GameObject E_BulletPrefab;
    [SerializeField] private int E_maxpool = 20;   // 풀링 최대 개수
    [SerializeField] private List<GameObject> E_bulletPool = new List<GameObject>();
    void Awake()
    {
        if (p_Instance == null)
            p_Instance = this;
        else if (p_Instance != this)    // p_instance에 할당된 클래스의 인스턴스가 다를 경우
            Destroy(this.gameObject);

        CreateBullet();
        CreateE_Bullet();
    }

    private void CreateBullet()
    {
        GameObject objectPools = new GameObject("ObjectPools");
        for (int i = 0; i < maxpool; i++)
        {
            var bullet = Instantiate(BulletPrefab, objectPools.transform);
            bullet.name = $"{i + 1}발";
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }
    }
    public GameObject GetBullet()
    {
        for (int i = 0; i < bulletPool.Count; i++)  // 활성/비활성 자동체크x
        {
            if (bulletPool[i].activeSelf == false)
            {
                return bulletPool[i];   // 비활성화 된 것만 반환
            }
        }
        return null;    // 활성화 되었다면 null 반환
    }
    private void CreateE_Bullet()
    {
        GameObject E_objectPools = new GameObject("E_ObjectPools");
        for (int i = 0; i < E_maxpool; i++)
        {
            var E_bullet = Instantiate(E_BulletPrefab, E_objectPools.transform);
            E_bullet.name = $"{i + 1}발";
            E_bullet.SetActive(false);
            E_bulletPool.Add(E_bullet);
        }
    }
    public GameObject GetE_Bullet()
    {
        for (int i = 0; i < E_bulletPool.Count; i++)  // 활성/비활성 자동체크x
        {
            if (E_bulletPool[i].activeSelf == false)
            {
                return E_bulletPool[i];   // 비활성화 된 것만 반환
            }
        }
        return null;    // 활성화 되었다면 null 반환
    }
}
