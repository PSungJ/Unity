using DataInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GameDataSO", menuName = "Create GameData", order = 3)]
public class GameDataObject : ScriptableObject
                        // ��ũ���ͺ� ������Ʈ�� Save�� Load 
{
    public int killCount = 0; // ų ī��Ʈ
    public float hp = 120f; // ü��
    public float damage = 25f; // ���ݷ�
    public float speed = 6f;
    public List<Item> equipItems = new List<Item>();
}
