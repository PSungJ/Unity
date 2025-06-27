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

    //�κ��丮�� �������� ���� �Ǿ��� �� �߻� ��ų �̺�Ʈ ���� 
    public delegate void ItemChangeDelegate();
    public static event ItemChangeDelegate OnItemChange; // ������ ���� �̺�Ʈ

    public GameObject slotList;// �κ��丮 ���� ����Ʈ ������Ʈ
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
        if (gameData.equipItem.Count > 0) // �κ��丮�� �������� �ִٸ�
        {
            InventroySetUp(); // �κ��丮 ���� �Լ� ȣ��
        }
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(gameData);
#endif
    }
    void InventroySetUp()
    {
        // �κ��丮 ���� ����Ʈ ������Ʈ�� �ڽ� ������Ʈ���� ������
        var slots = slotList.GetComponentsInChildren<Transform>();
        //������ ������ ���� ��ŭ �ݺ�
        for (int i = 0; i < gameData.equipItem.Count; i++)
        {
            //�κ��丮 UI�� �ִ� Slot ���� ��ŭ �ݺ�
            for (int j = 1; j < slots.Length; j++)
            {
                if (slots[j].childCount > 0) continue;
                // ���Կ� �̹� �������� ������ �����ϰ� ���� �ε����� �Ѿ

                int itemIndex = (int)gameData.equipItem[i].itemType;
                // �������� ������ ���� �ε����� ����

                itemObjects[itemIndex].GetComponent<Transform>().SetParent(slots[j]);
                // ������ ������Ʈ�� ���Կ� �߰��ؼ� ������ ������Ʈ�� �ڽ��� �ȴ�.

                itemObjects[itemIndex].GetComponent<ItemInfo>().itemData = gameData.equipItem[i];
                // ������ ������Ʈ�� ItemInfo ������Ʈ�� ������ �����͸� �Ҵ�

                break; // �������� �߰��� �Ŀ��� �� �̻� �ݺ����� ����
            }
        }
    }
    void SaveGameData()
    {
        //dataManager.Save(gameData); // ������ �Ŵ����� ���� ���� ������ ����
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(gameData);
#endif
    }
    public void AddItem(Item item) //�κ��丮���� �������� �߰� �Ҷ� ������������ �����ϴ� �Լ�
    {
        if (gameData.equipItem.Contains(item)) return; // �̹� ���� �������� �����ϸ� �߰����� ����

        gameData.equipItem.Add(item); // ��������  GameData.equipItem �迭��  �߰�
        switch (item.itemType) //�������� ������ ���� �б� 
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
    public void RemoveItem(Item item) //�κ��丮���� �������� ���� �Ҷ� ������������ �����ϴ� �Լ�
    {
        gameData.equipItem.Remove(item); // �������� GameData.equipItem �迭���� ����

        switch (item.itemType) //�������� ������ ���� �б� 
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
        SaveGameData(); // ���� ���� �� ������ ����
    }
}
