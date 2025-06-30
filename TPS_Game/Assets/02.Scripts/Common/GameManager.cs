using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DataInfo;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance  =null;
    public bool isGameOver = false;
    public GameObject Panel_Weapon;
    public CanvasGroup inventoryCG;
    //public int killCount = 0;
    //public GameData gameData; // ���ӵ����� Ŭ���� �ν��Ͻ�
    public GameDataObject gameData;
    public Text killCountText;
    public DataManager dataManager; // ������ �Ŵ��� �ν��Ͻ�

    //�κ��丮�� �������� ���� �Ǿ��� �� �߻� ��ų �̺�Ʈ ����
    public delegate void ItemChangeDelegate();
    public static event ItemChangeDelegate OnItemChange;
    public GameObject slotList; // �κ��丮 ���� ����Ʈ ������Ʈ
    public GameObject[] itemObjects;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        // �ν��Ͻ��� �Ҵ�� Ŭ������  �ν��Ͻ��� �ٸ� ��� ���λ����� Ŭ������ �ǹ���
        else if(Instance!= this)
            Destroy(this.gameObject);
        // �ٸ������� �Ѿ���� ���� ���� �ʰ� ������ 
        DontDestroyOnLoad(gameObject);
        dataManager = GetComponent<DataManager>();
        dataManager.Initalize();
        LoadGameData();
    }
  
    void LoadGameData()
    {
        //killCount = PlayerPrefs.GetInt("KILL_COUNT", 0);
        //GameData data = dataManager.Load();
        //gameData.hp = data.hp;
        //gameData.damage = data.damage;
        //gameData.speed = data.speed;
        //gameData.killCount = data.killCount;
        //gameData.equipItems = data.equipItems; 
        if(gameData.equipItems.Count > 0)
        {
            InventorySetUp(); // �κ��丮 ���� �Լ� ȣ��
        }
        killCountText.text = $"KILL: <color=#ff0000>{gameData.killCount.ToString("000")}</color>";

        //.asset ���Ͽ� ������ ����
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(gameData);
#else
   // ����� ���ӿ��� ����ɼ� �ֵ��� �ڵ带 ������ ��
   //saveGameData(); //���� ������ ���� �Լ� ȣ��
#endif
        // ��ũ���ͺ� ������Ʈ�� ���������� ���� �ϱ⿡ ������ �ε� ������ �ʿ����� �ʴ�.
    }
    void InventorySetUp()
    {
        // �κ��丮 ���� ����Ʈ ������Ʈ�� �ڽ� ������Ʈ���� ������
        var slots = slotList.GetComponentsInChildren<Transform>();
        //������ ������ ���� ��ŭ �ݺ�
        for(int x = 0; x < gameData.equipItems.Count; x++)
        {
            // �κ��丮 UI�� �ִ� SLOT ���� ��ŭ �ݺ�
            for(int y = 1; y < slots.Length; y++)
            {
                if (slots[y].childCount > 0) continue;
                // ���Կ� �̹� �������� ������ �����ϰ� ���� �ε����� �Ѿ
                int itemIdx = (int)gameData.equipItems[x].itemType;
                // �������� ������ ���� �ε��� ����

                itemObjects[itemIdx].GetComponent<Transform>().SetParent(slots[x]);
                // ������ ������Ʈ�� ���Կ� �߰�

                itemObjects[itemIdx].GetComponent<ItemInfo>().itemData = gameData.equipItems[x];
                //������ ������Ʈ�� ItemInfo ������Ʈ�� ������ �Ҵ�
                break; 
            }
        }
    }
    void SaveGameData()
    {
        //dataManager.Save(gameData);
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(gameData);
#else
   // ����� ���ӿ��� ����ɼ� �ֵ��� �ڵ带 ������ ��
   //saveGameData(); //���� ������ ���� �Լ� ȣ��
#endif
    }
    public void AddItem(Item item) // �κ��丮���� �������� �߰� �Ҷ� ������ ������ �����ϴ� �Լ�
    {
        if (gameData.equipItems.Contains(item)) return; // �̹� ���� �������� �����ϸ� �߰����� ����

        gameData.equipItems.Add(item); // �������� GameData.equipItem �迭��  �߰�
        switch(item.itemType) // �������� ������ ���� �б�
        {
            case Item.ItemType.HP:
                if(item.itemCalc == Item.ItemCalc.VALUE)
                {
                    gameData.hp += item.value;
                }
                else
                {
                    gameData.hp += gameData.hp * item.value;
                }
                    break;
            case Item.ItemType.DAMAGE:
                if (item.itemCalc == Item.ItemCalc.VALUE)
                {
                    gameData.damage += item.value;
                }
                else
                {
                    gameData.damage += gameData.damage * item.value;
                }
                break;
            case Item.ItemType.SPEED:
                if (item.itemCalc == Item.ItemCalc.VALUE)
                {
                    gameData.speed += item.value;
                }
                else
                {
                    gameData.speed += gameData.speed * item.value;
                }
                break;
            case Item.ItemType.GRENADE:

                break;
        }
        OnItemChange();
        //.asset ���Ͽ� ������ ����
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(gameData);
#else
   // ����� ���ӿ��� ����ɼ� �ֵ��� �ڵ带 ������ ��
   //saveGameData(); //���� ������ ���� �Լ� ȣ��
#endif
    }
    public void RemoveItem(Item item)
    {
        gameData.equipItems.Remove(item);
        switch (item.itemType) // �������� ������ ���� �б�
        {
            case Item.ItemType.HP:
                if (item.itemCalc == Item.ItemCalc.VALUE)
                {
                    gameData.hp -= item.value;
                }
                else
                {
                    gameData.hp = gameData.hp /(1.0f + item.value);
                }
                break;
            case Item.ItemType.DAMAGE:
                if (item.itemCalc == Item.ItemCalc.VALUE)
                {
                    gameData.damage -= item.value;
                }
                else
                {
                    gameData.damage = gameData.damage / (1.0f + item.value);
                }
                break;
            case Item.ItemType.SPEED:
                if (item.itemCalc == Item.ItemCalc.VALUE)
                {
                    gameData.speed -= item.value;
                }
                else
                {
                    gameData.speed = gameData.speed / (1.0f + item.value);
                }
                break;
            case Item.ItemType.GRENADE:

                break;
        }
        OnItemChange();
        //.asset ���Ͽ� ������ ����
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(gameData);
#else
   // ����� ���ӿ��� ����ɼ� �ֵ��� �ڵ带 ������ ��
   //saveGameData(); //���� ������ ���� �Լ� ȣ��
#endif
    }
    void Start()
    {
        OnInventoryOpen(false);
    }
    private bool isPaused;
    public void OnPauseClick()
    {
        isPaused = !isPaused;
        OnStopPlay(isPaused);
        var canvasGroup = Panel_Weapon.GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = !isPaused;
    }
    private void OnStopPlay(bool isStop)
    {
        Time.timeScale = (isStop ? 0f : 1.0f);
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        MonoBehaviour[] scripts = playerObj.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
            script.enabled = !isPaused;
    }
    public void OnInventoryOpen(bool isOpened)
    {
         OnStopPlay(isOpened); 
        inventoryCG.alpha =(isOpened)? 1.0f: 0.0f;
        inventoryCG.interactable = true;
        inventoryCG.blocksRaycasts = true;
    }
    public void IncrementKillCount()
    {
       // killCount++;
       ++gameData.killCount;
        //PlayerPrefs.SetInt("KILL_COUNT", killCount);
        killCountText.text = $"KILL: <color=#ff0000>{gameData.killCount.ToString("000")}</color>";
    }
    private void OnDisable()
    {
        //PlayerPrefs.DeleteKey("KILL_COUNT");
    }
    private void OnApplicationQuit()
    {
        SaveGameData();
    }
}
