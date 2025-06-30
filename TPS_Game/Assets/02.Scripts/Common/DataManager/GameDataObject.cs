using DataInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GameDataSO", menuName = "Create GameData", order = 3)]
public class GameDataObject : ScriptableObject
                        // 스크립터블 오브젝트로 Save와 Load 
{
    public int killCount = 0; // 킬 카운트
    public float hp = 120f; // 체력
    public float damage = 25f; // 공격력
    public float speed = 6f;
    public List<Item> equipItems = new List<Item>();
}
