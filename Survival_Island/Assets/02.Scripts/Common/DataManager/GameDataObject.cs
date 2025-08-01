using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataInfo;

[CreateAssetMenu(fileName = "GameDataSO", menuName = "ScriptableObjects/GameDataSO", order = 1)]
public class GameDataObject : ScriptableObject
{
    public float hp = 120f;
    public float damage = 25f;
    public float speed = 6f;
    public List<Item> equipItem = new List<Item>();
}
