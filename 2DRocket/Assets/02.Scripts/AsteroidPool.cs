using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidPool : MonoBehaviour
{
    public static AsteroidPool p_instance = null;
    [Header("Asteroid Pool")]
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private List<GameObject> asteroidPool;
    [SerializeField] private int maxPool = 10;
    private void Awake()
    {
        if (p_instance == null)
            p_instance = this;
        else if (p_instance != this)
            Destroy(this.gameObject);

        StartCoroutine(CreateAsteroid());
    }
    IEnumerator CreateAsteroid()
    {
        yield return new WaitForSeconds(0.3f);
        var AstGroup = new GameObject("AstGroup");
        for (int i = 0; i < maxPool; i++)
        {
            var ast = Instantiate(asteroidPrefab, AstGroup.transform);
            ast.name = $"{i + 1}°³";
            ast.SetActive(false);
            asteroidPool.Add(ast);
        }
    }
    public GameObject GetAst()
    {
        for (int i = 0; i < asteroidPool.Count; i++)
        {
            if (asteroidPool[i].activeSelf == false)
            {
                return asteroidPool[i];
            }
        }
        return null;
    }
    void Update()
    {
        
    }
}
