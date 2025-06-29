using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DataInfo;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public bool isGameOver = false;
    public GameObject Panel_Weapon;
    public CanvasGroup inventoryCG;

    //public int killCount = 0;
    //public GameData gameData; // ���� ������ Ŭ���� �ν��Ͻ�
    public GameDataObject gameData;
    public Text killCountText;
    public DataManager dataManager; // ������ �Ŵ��� �ν��Ͻ�

    //�κ��丮�� �������� ���� �Ǿ��� �� �߻� ��ų �̺�Ʈ ���� 
    public delegate void ItemChangeDelegate();
    public static event ItemChangeDelegate OnItemChange; // ������ ���� �̺�Ʈ

    public GameObject slotList;// �κ��丮 ���� ����Ʈ ������Ʈ
    public GameObject[] itemObjects;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        // �ν��Ͻ��� �Ҵ�� Ŭ������  �ν��Ͻ��� �ٸ� ��� ���λ����� Ŭ������ �ǹ���
        else if (Instance != this)
            Destroy(this.gameObject);
        // �ٸ������� �Ѿ���� ���� ���� �ʰ� ������ 
        DontDestroyOnLoad(gameObject);
        dataManager = GetComponent<DataManager>();
        dataManager.Initialize(); // ������ �Ŵ��� �ʱ�ȭ
        LoadGameData();
    }
    void LoadGameData()
    {              //killCount Ű�� ����� ���� �ε� 
        //killCount = PlayerPrefs.GetInt("KILL_COUNT", 0);

        //GameData data = dataManager.Load(); // ������ �Ŵ����� ���� ���� ������ �ε�

        //gameData.hp = data.hp; // �ε�� �������� hp ��
        //gameData.damage = data.damage; // �ε�� �������� damage ��
        //gameData.speed = data.speed; // �ε�� �������� speed ��
        //gameData.killCount = data.killCount; // �ε�� �������� killCount ��
        //gameData.equipItems = data.equipItems; // �ε�� �������� equipItems ��
        
        if (gameData.equipItem.Count > 0) // �κ��丮�� �������� �ִٸ�
        {
            InventroySetUp(); // �κ��丮 ���� �Լ� ȣ��
        }
        killCountText.text = $"KILL :<color=#ff0000>{gameData.killCount.ToString("000")}</color>";

        //.asset ���Ͽ� ������ ����
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
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(gameData);
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

        inventoryCG.alpha = (isOpened) ? 1.0f : 0.0f;
        inventoryCG.interactable = true;
        inventoryCG.blocksRaycasts = true;
    }
    public void IncrementKillCount()
    {
        //++killCount;
        ++gameData.killCount; //���� �������� ų ī��Ʈ ����
                              //PlayerPrefs.SetInt("KILL_COUNT", killCount);
                              //ų�� ���� 
        killCountText.text = $"KILL :<color=#ff0000>{gameData.killCount.ToString("000")}</color>";
    }
    private void OnDisable()
    {

    }
    private void OnApplicationQuit()
    {
        SaveGameData(); // ���� ���� �� ������ ����
    }
}
