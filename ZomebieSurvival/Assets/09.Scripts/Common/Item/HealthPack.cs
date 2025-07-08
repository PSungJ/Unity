using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour, I_Item
{
    public int health = 50;
    public void Use(GameObject target)
    {
        LivingEntity life = target.GetComponent<LivingEntity>();    // ���޹��� ���� ������Ʈ�κ��� LivingEntity ������Ʈ ��������

        if (life != null)   // LivingEntity ������Ʈ�� �ִٸ�
        {
            life.RestoreHealth(health); // ü�� ȸ�� ����
        }
        Destroy(gameObject);
    }
}
