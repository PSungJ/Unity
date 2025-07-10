using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager p_instance;

    [Header("Explosion Effect Pooling")]
    [SerializeField] private GameObject expPrefab;
    [SerializeField] private int maxPool = 15;
    [SerializeField] private List<GameObject> expPool = new List<GameObject>();

    void Awake()
    {
        if (p_instance == null)
            p_instance = this;
        else if (p_instance != this)
            Destroy(this.gameObject);

        CreateExp();
    }

    private void CreateExp()
    {
        GameObject obj = new GameObject("Explosion");
        for (int i = 0; i < maxPool; i++)
        {
            var eff = Instantiate(expPrefab, obj.transform);
            eff.name = $"Æø¹ßÈ¿°ú {i + 1}";
            eff.SetActive(false);
            expPool.Add(eff);
        }
    }
    public GameObject GetExp()
    {
        for (int i = 0; i < expPool.Count; i++)
        {
            if (expPool[i].activeSelf == false)
                return expPool[i];
        }
        return null;
    }
}
