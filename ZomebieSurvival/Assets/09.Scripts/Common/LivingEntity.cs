using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
// I_Damageable을 상속받는 부모 클래스 -> Player, Enemy등 생명체의 공통 기능을 구현
public class LivingEntity : MonoBehaviourPun, I_Damageable
{
    public float startingHealth = 100f; // 시작 체력
    public float health {  get; protected set; }  // 현재 체력
    public bool dead { get; protected set; }  // 사망 여부
    public event Action onDeath;    // 사망시 호출되는 이벤트

    [PunRPC]    // 호스트 -> 모든 클라이언트 방향으로 체력과 사망상태를 동기화 하는 메서드
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

    [PunRPC]
    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            health -= damage;   // 데미지만큼 체력감소
            photonView.RPC("ApplyUpdatedHealth", RpcTarget.Others, health, dead);   // 호스트에서 클라이언트로 동기화
            photonView.RPC("OnDamage", RpcTarget.Others, damage, hitPoint, hitNormal);  // 다른 클라이언트들도 OnDamage를 실행하도록 함
        }
        
        if (health <= 0 && !dead)   //체력이 0이하 and 아직 죽지 않았다면
        {
            Die();
        }
    }

    [PunRPC]
    public virtual void RestoreHealth(float newHealth) // 체력 회복 메서드
    {
        if (dead) return;   // 죽음 상태이면 아래 함수 비진행

        if (PhotonNetwork.IsMasterClient)
        {
            health += newHealth;    // HealthPack 체력 회복 수치만큼 체력 증가
            photonView.RPC("ApplyUpdatedHealth", RpcTarget.Others, health, dead);
            photonView.RPC("RestoreHealth", RpcTarget.Others, newHealth);
        }
        
    }
    public virtual void Die()
    {
        if (onDeath != null)    // onDeath 이벤트가 구독되어 있다면
        {
            onDeath();  // 이벤트 호출
        }
        dead = true;
        //gameObject.SetActive(false);    // 게임오브젝트 비활성화
    }

}
