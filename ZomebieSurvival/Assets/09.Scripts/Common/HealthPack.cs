using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour, I_Item
{
    public int health = 50;
    public void Use(GameObject target)
    {
        // ü�� ȸ��
        Debug.Log($"ü�� ȸ�� {health}");
    }
}
