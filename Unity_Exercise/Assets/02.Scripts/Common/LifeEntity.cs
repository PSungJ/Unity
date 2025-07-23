using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeEntity : MonoBehaviour, IDamage
{
    public float startingHealth = 100f; // ���� ü��
    public float health { get; protected set; }  // ���� ü��
    public bool dead { get; protected set; }  // ��� ����
    public event Action onDeath;    // ����� ȣ��Ǵ� �̺�Ʈ

    public void ApplyUpdatedHealth(float newHealth, bool newDead)
    {
        health = newHealth; ;
        dead = newDead;
    }

    protected virtual void OnEnable()
    {
        dead = false;
        health = startingHealth;
    }

    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        health -= damage;
        if (health <= 0 && !dead)   //ü���� 0���� and ���� ���� �ʾҴٸ�
        {
            Die();
        }
    }

    public virtual void Die()
    {
        if (onDeath != null)    // onDeath �̺�Ʈ�� �����Ǿ� �ִٸ�
        {
            onDeath();  // �̺�Ʈ ȣ��
        }
        dead = true;
    }
}
