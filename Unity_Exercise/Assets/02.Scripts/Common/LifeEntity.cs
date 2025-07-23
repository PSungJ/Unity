using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeEntity : MonoBehaviour, IDamage
{
    public float startingHealth = 100f; // 시작 체력
    public float health { get; protected set; }  // 현재 체력
    public bool dead { get; protected set; }  // 사망 여부
    public event Action onDeath;    // 사망시 호출되는 이벤트

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
        if (health <= 0 && !dead)   //체력이 0이하 and 아직 죽지 않았다면
        {
            Die();
        }
    }

    public virtual void Die()
    {
        if (onDeath != null)    // onDeath 이벤트가 구독되어 있다면
        {
            onDeath();  // 이벤트 호출
        }
        dead = true;
    }
}
