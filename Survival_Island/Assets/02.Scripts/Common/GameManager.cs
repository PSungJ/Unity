using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataInfo;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject skelPrefab;
    public CanvasGroup inventoryCanvas;
    public List<Transform> spawnList;
    private float timePrev;
    private int maxSkel = 5;
    private readonly string skeletonTag = "ENEMY";

    public GameDataObject gameData;
    public DataManager dataManager;

    //인벤토리의 아이템이 변경 되었을 때 발생 시킬 이벤트 정의 
    public delegate void ItemChangeDelegate();
    public static event ItemChangeDelegate OnItemChange; // 아이템 변경 이벤트

    public GameObject slotList;// 인벤토리 슬롯 리스트 오브젝트
    public GameObject[] itemObjects;
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
        dataManager = GetComponent<DataManager>();
        dataManager.Initialize();
    }
    void LoadGameData()
    {
        if (gameData.equipItem.Count > 0) // 인벤토리에 아이템이 있다면
        {
            InventroySetUp(); // 인벤토리 설정 함수 호출
        }
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(gameData);
#endif
    }
    void InventroySetUp()
    {
        // 인벤토리 슬롯 리스트 오브젝트의 자식 오브젝트들을 가져옴
        var slots = slotList.GetComponentsInChildren<Transform>();
        //보유한 아이템 갯수 만큼 반복
        for (int i = 0; i < gameData.equipItem.Count; i++)
        {
            //인벤토리 UI에 있는 Slot 개수 만큼 반복
            for (int j = 1; j < slots.Length; j++)
            {
                if (slots[j].childCount > 0) continue;
                // 슬롯에 이미 아이템이 있으면 무시하고 다음 인덱스로 넘어감

                int itemIndex = (int)gameData.equipItem[i].itemType;
                // 아이템의 종류에 따라 인덱스를 추출

                itemObjects[itemIndex].GetComponent<Transform>().SetParent(slots[j]);
                // 아이템 오브젝트를 슬롯에 추가해서 아이템 오브젝트는 자식이 된다.

                itemObjects[itemIndex].GetComponent<ItemInfo>().itemData = gameData.equipItem[i];
                // 아이템 오브젝트의 ItemInfo 컴포넌트에 아이템 데이터를 할당

                break; // 아이템을 추가한 후에는 더 이상 반복하지 않음
            }
        }
    }
    void SaveGameData()
    {
        //dataManager.Save(gameData); // 데이터 매니저를 통해 게임 데이터 저장
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(gameData);
#endif
    }
    public void AddItem(Item item) //인벤토리에서 아이템을 추가 할때 데이터정보를 갱신하는 함수
    {
        if (gameData.equipItem.Contains(item)) return; // 이미 같은 아이템이 존재하면 추가하지 않음

        gameData.equipItem.Add(item); // 아이템을  GameData.equipItem 배열에  추가
        switch (item.itemType) //아이템의 종류에 따라 분기 
        {
            case Item.ItemType.HP:
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.hp += item.value;
                else
                    gameData.hp += gameData.hp * item.value;

                break;
            case Item.ItemType.DAMAGE:

                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.damage += item.value;
                else
                    gameData.damage += gameData.damage * item.value;
                break;

            case Item.ItemType.SPEED:

                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.speed += item.value;
                else
                    gameData.speed += gameData.speed * item.value;

                break;
            case Item.ItemType.GRENADE:


                break;

        }
        OnItemChange();
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(gameData);
#endif

    }
    public void RemoveItem(Item item) //인벤토리에서 아이템을 제거 할때 데이터정보를 갱신하는 함수
    {
        gameData.equipItem.Remove(item); // 아이템을 GameData.equipItem 배열에서 제거

        switch (item.itemType) //아이템의 종류에 따라 분기 
        {
            case Item.ItemType.HP:
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.hp -= item.value;
                else
                    gameData.hp = gameData.hp / (1.0f + item.value);
                break;
            case Item.ItemType.DAMAGE:
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.damage -= item.value;
                else
                    gameData.damage = gameData.damage / (1.0f + item.value);
                break;
            case Item.ItemType.SPEED:
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.speed -= item.value;
                else
                    gameData.speed = gameData.speed * (1f + item.value);
                break;
            case Item.ItemType.GRENADE:

                break;
        }
        OnItemChange();
    }

    private void Start()
    {
        timePrev = Time.time;
        Transform[] spawnPoints = GameObject.Find("SpawnPoint").GetComponentsInChildren<Transform>();
        if (spawnPoints != null)
            spawnList = new List<Transform>(spawnPoints);
        spawnList.RemoveAt(0);
    }

    private void Update()
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
    private void OnApplicationQuit()
    {
        SaveGameData(); // 게임 종료 시 데이터 저장
    }
}
