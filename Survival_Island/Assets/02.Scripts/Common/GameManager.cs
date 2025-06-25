using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject skelPrefab;
    public CanvasGroup inventoryCanvas;
    public List<Transform> spawnList;
    private float timePrev;
    private int maxSkel = 5;
    private readonly string skeletonTag = "ENEMY";
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        timePrev = Time.time;
        Transform[] spawnPoints = GameObject.Find("SpawnPoint").GetComponentsInChildren<Transform>();
        if (spawnPoints != null)
            spawnList = new List<Transform>(spawnPoints);
        spawnList.RemoveAt(0);
    }

    void Update()
    {
        if (Time.time - timePrev >= 3f)
        {
            timePrev = Time.time;
            int zombieCount = GameObject.FindGameObjectsWithTag(skeletonTag).Length;
            if (zombieCount < maxSkel)
                CreateEnemy();
        }
    }
    private void CreateEnemy()
    {
        int idx = Random.Range(0, spawnList.Count);
        Instantiate(skelPrefab, spawnList[idx].position, spawnList[idx].rotation);
    }
    public void MousePointerVisible()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void MousePointerDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private bool isPaused;
    public void OnInventoryClick(bool isOpened)
    {
        isPaused = !isPaused;
        Time.timeScale = (isPaused ? 0f : 1.0f);
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        MonoBehaviour[] scripts = playerObj.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
            script.enabled = !isPaused;

        inventoryCanvas.alpha = (isOpened ? 1.0f : 0f);
    }   
}
