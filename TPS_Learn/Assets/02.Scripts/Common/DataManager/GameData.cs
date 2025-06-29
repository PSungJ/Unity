using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataInfo
{
    [System.Serializable]
    public class GameData   // Entity 클래스 : 기능은 없고 데이터만 담당
    {                       // 중요한 기능을 담당하는 클래스 컨트롤 클래스 혹은 핸들러 클래스라고도 함
        public int killCount = 0;
        public float hp = 120f;
        public float damage = 25f;
        public float speed = 6.0f;
        public List<Item> equipItems = new List<Item>();
    }
    [System.Serializable]
    public class Item
    {
        public enum ItemType
        {
            HP, SPEED, GRENADE, DAMAGE
        }
        public enum ItemCalc
        {
            VALUE, PERCENT
        }
        public ItemType itemType;   // 아이템 종류
        public ItemCalc itemCalc;   // 아이템 계산 방식

        public string name;         // 아이템 이름
        public string description;  // 아이템 설명
        public float value;         // 아이템 효과 값
    }
}
