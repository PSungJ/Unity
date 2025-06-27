using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataInfo
{
    [System.Serializable]
    public class GameData : MonoBehaviour
    {
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

