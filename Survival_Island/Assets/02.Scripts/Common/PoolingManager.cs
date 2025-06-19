using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager p_Instance = null;
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private int maxPool = 10;
    [SerializeField] private List<GameObject> bulletPool = new List<GameObject>();
    void Awake()
    {
        if (p_Instance == null)
            p_Instance = this;
        else if (p_Instance != this)
            Destroy(this.gameObject);

        CreateBullet();
    }
    private void CreateBullet()
    {
        GameObject objectPools = new GameObject("ObjectPools");
        for (int i = 0; i < maxPool; i++)
        {
            var bullet = Instantiate(BulletPrefab, objectPools.transform);
            bullet.name = $"ÃÑ¾Ë{i + 1}";
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }
    }
    public GameObject GetBullet()
    {
        for (int i = 0;i < bulletPool.Count; i++)
        {
            if (bulletPool[i].activeSelf == false)
            {
                return bulletPool[i];
            }
        }
        return null;
    }
}
