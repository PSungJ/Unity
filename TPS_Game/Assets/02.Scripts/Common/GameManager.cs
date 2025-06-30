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
    //public GameData gameData; // 게임데이터 클래스 인스턴스
    public GameDataObject gameData;
    public Text killCountText;
    public DataManager dataManager; // 데이터 매니저 인스턴스

    //인벤토리의 아이템이 변경 되었을 때 발생 시킬 이벤트 정의
    public delegate void ItemChangeDelegate();
    public static event ItemChangeDelegate OnItemChange;
    public GameObject slotList; // 인벤토리 슬롯 리스트 오브젝트
    public GameObject[] itemObjects;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        // 인스턴스에 할당된 클래스의  인스턴스가 다를 경우 새로생성된 클래스를 의미함
        else if(Instance!= this)
            Destroy(this.gameObject);
        // 다른씬으로 넘어가더라도 삭제 하지 않고 유지함 
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
            InventorySetUp(); // 인벤토리 설정 함수 호출
        }
        killCountText.text = $"KILL: <color=#ff0000>{gameData.killCount.ToString("000")}</color>";

        //.asset 파일에 데이터 저장
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(gameData);
#else
   // 빌드된 게임에서 실행될수 있도록 코드를 만들어야 함
   //saveGameData(); //게임 데이터 저장 함수 호출
#endif
        // 스크립터블 오브젝트는 전역적으로 접근 하기에 별도의 로드 과정이 필요하지 않다.
    }
    void InventorySetUp()
    {
        // 인벤토리 슬롯 리스트 오브젝트의 자식 오브젝트들을 가져옴
        var slots = slotList.GetComponentsInChildren<Transform>();
        //보유한 아이템 갯수 만큼 반복
        for(int x = 0; x < gameData.equipItems.Count; x++)
        {
            // 인벤토리 UI에 있는 SLOT 갯수 만큼 반복
            for(int y = 1; y < slots.Length; y++)
            {
                if (slots[y].childCount > 0) continue;
                // 슬롯에 이미 아이템이 있으면 무시하고 다음 인덱스로 넘어감
                int itemIdx = (int)gameData.equipItems[x].itemType;
                // 아이템의 종류에 따라 인덱스 추출

                itemObjects[itemIdx].GetComponent<Transform>().SetParent(slots[x]);
                // 아이템 오브젝트를 슬롯에 추가

                itemObjects[itemIdx].GetComponent<ItemInfo>().itemData = gameData.equipItems[x];
                //아이템 오브젝트의 ItemInfo 컴포넌트에 아이템 할당
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
   // 빌드된 게임에서 실행될수 있도록 코드를 만들어야 함
   //saveGameData(); //게임 데이터 저장 함수 호출
#endif
    }
    public void AddItem(Item item) // 인벤토리에서 아이템을 추가 할때 데이터 정보를 갱신하는 함수
    {
        if (gameData.equipItems.Contains(item)) return; // 이미 같은 아이템이 존재하면 추가하지 않음

        gameData.equipItems.Add(item); // 아이템을 GameData.equipItem 배열에  추가
        switch(item.itemType) // 아이템의 종류에 따라 분기
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
        //.asset 파일에 데이터 저장
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(gameData);
#else
   // 빌드된 게임에서 실행될수 있도록 코드를 만들어야 함
   //saveGameData(); //게임 데이터 저장 함수 호출
#endif
    }
    public void RemoveItem(Item item)
    {
        gameData.equipItems.Remove(item);
        switch (item.itemType) // 아이템의 종류에 따라 분기
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
        //.asset 파일에 데이터 저장
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(gameData);
#else
   // 빌드된 게임에서 실행될수 있도록 코드를 만들어야 함
   //saveGameData(); //게임 데이터 저장 함수 호출
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
