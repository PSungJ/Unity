using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// I_Damageable을 상속받는 부모 클래스 -> Player, Enemy등 생명체의 공통 기능을 구현
public class LivingEntity : MonoBehaviour, I_Damageable
{
    public float startingHealth = 100f; // 시작 체력
    public float health {  get; protected set; }  // 현재 체력
    public bool dead { get; protected set; }  // 사망 여부
    public event Action onDeath;    // 사망시 호출되는 이벤트

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
    public virtual void RestoreHealth(float newHealth) // 체력 회복 메서드
    {
        if (dead) return;   // 죽음 상태이면 아래 함수 비진행

        health += newHealth;    // HealthPack 체력 회복 수치만큼 체력 증가
    }
    public virtual void Die()
    {
        dead = true;
        if (onDeath != null)    // onDeath 이벤트가 구독되어 있다면
        {
            onDeath();  // 이벤트 호출
        }
        gameObject.SetActive(false);    // 게임오브젝트 비활성화
    }

}
