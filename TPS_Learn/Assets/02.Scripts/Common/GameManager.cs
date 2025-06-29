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
    //public GameData gameData; // 게임 데이터 클래스 인스턴스
    public GameDataObject gameData;
    public Text killCountText;
    public DataManager dataManager; // 데이터 매니저 인스턴스

    //인벤토리의 아이템이 변경 되었을 때 발생 시킬 이벤트 정의 
    public delegate void ItemChangeDelegate();
    public static event ItemChangeDelegate OnItemChange; // 아이템 변경 이벤트

    public GameObject slotList;// 인벤토리 슬롯 리스트 오브젝트
    public GameObject[] itemObjects;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        // 인스턴스에 할당된 클래스의  인스턴스가 다를 경우 새로생성된 클래스를 의미함
        else if (Instance != this)
            Destroy(this.gameObject);
        // 다른씬으로 넘어가더라도 삭제 하지 않고 유지함 
        DontDestroyOnLoad(gameObject);
        dataManager = GetComponent<DataManager>();
        dataManager.Initialize(); // 데이터 매니저 초기화
        LoadGameData();
    }
    void LoadGameData()
    {              //killCount 키로 저장된 값을 로드 
        //killCount = PlayerPrefs.GetInt("KILL_COUNT", 0);

        //GameData data = dataManager.Load(); // 데이터 매니저를 통해 게임 데이터 로드

        //gameData.hp = data.hp; // 로드된 데이터의 hp 값
        //gameData.damage = data.damage; // 로드된 데이터의 damage 값
        //gameData.speed = data.speed; // 로드된 데이터의 speed 값
        //gameData.killCount = data.killCount; // 로드된 데이터의 killCount 값
        //gameData.equipItems = data.equipItems; // 로드된 데이터의 equipItems 값
        
        if (gameData.equipItem.Count > 0) // 인벤토리에 아이템이 있다면
        {
            InventroySetUp(); // 인벤토리 설정 함수 호출
        }
        killCountText.text = $"KILL :<color=#ff0000>{gameData.killCount.ToString("000")}</color>";

        //.asset 파일에 데이터 저장
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
        ++gameData.killCount; //게임 데이터의 킬 카운트 증가
                              //PlayerPrefs.SetInt("KILL_COUNT", killCount);
                              //킬수 저장 
        killCountText.text = $"KILL :<color=#ff0000>{gameData.killCount.ToString("000")}</color>";
    }
    private void OnDisable()
    {

    }
    private void OnApplicationQuit()
    {
        SaveGameData(); // 게임 종료 시 데이터 저장
    }
}
