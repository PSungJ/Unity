using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, I_Item
{
    public int score = 200; // ������ ����
    public void Use(GameObject target)
    {
        GameManager.instance.AddScore(score);   // ���ӸŴ����� �����Ͽ� ���� ����
        Destroy(gameObject);    // ��� �� �ı�
    }
}
