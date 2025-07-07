using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// I_Damageable�� ��ӹ޴� �θ� Ŭ���� -> Player, Enemy�� ����ü�� ���� ����� ����
public class LivingEntity : MonoBehaviour, I_Damageable
{
    public float startingHealth = 100f; // ���� ü��
    public float health {  get; protected set; }  // ���� ü��
    public bool dead { get; protected set; }  // ��� ����
    public event Action onDeath;    // ����� ȣ��Ǵ� �̺�Ʈ

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
    public virtual void RestoreHealth(float newHealth) // ü�� ȸ�� �޼���
    {
        if (dead) return;   // ���� �����̸� �Ʒ� �Լ� ������

        health += newHealth;    // HealthPack ü�� ȸ�� ��ġ��ŭ ü�� ����
    }
    public virtual void Die()
    {
        dead = true;
        if (onDeath != null)    // onDeath �̺�Ʈ�� �����Ǿ� �ִٸ�
        {
            onDeath();  // �̺�Ʈ ȣ��
        }
        gameObject.SetActive(false);    // ���ӿ�����Ʈ ��Ȱ��ȭ
    }

}
