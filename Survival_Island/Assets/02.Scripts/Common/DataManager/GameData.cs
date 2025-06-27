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
        public ItemType itemType;   // ������ ����
        public ItemCalc itemCalc;   // ������ ��� ���

        public string name;         // ������ �̸�
        public string description;  // ������ ����
        public float value;         // ������ ȿ�� ��
    }
}

