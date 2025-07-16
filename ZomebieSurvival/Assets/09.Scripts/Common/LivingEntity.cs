using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
// I_Damageable�� ��ӹ޴� �θ� Ŭ���� -> Player, Enemy�� ����ü�� ���� ����� ����
public class LivingEntity : MonoBehaviourPun, I_Damageable
{
    public float startingHealth = 100f; // ���� ü��
    public float health {  get; protected set; }  // ���� ü��
    public bool dead { get; protected set; }  // ��� ����
    public event Action onDeath;    // ����� ȣ��Ǵ� �̺�Ʈ

    [PunRPC]    // ȣ��Ʈ -> ��� Ŭ���̾�Ʈ �������� ü�°� ������¸� ����ȭ �ϴ� �޼���
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
            health -= damage;   // ��������ŭ ü�°���
            photonView.RPC("ApplyUpdatedHealth", RpcTarget.Others, health, dead);   // ȣ��Ʈ���� Ŭ���̾�Ʈ�� ����ȭ
            photonView.RPC("OnDamage", RpcTarget.Others, damage, hitPoint, hitNormal);  // �ٸ� Ŭ���̾�Ʈ�鵵 OnDamage�� �����ϵ��� ��
        }
        
        if (health <= 0 && !dead)   //ü���� 0���� and ���� ���� �ʾҴٸ�
        {
            Die();
        }
    }

    [PunRPC]
    public virtual void RestoreHealth(float newHealth) // ü�� ȸ�� �޼���
    {
        if (dead) return;   // ���� �����̸� �Ʒ� �Լ� ������

        if (PhotonNetwork.IsMasterClient)
        {
            health += newHealth;    // HealthPack ü�� ȸ�� ��ġ��ŭ ü�� ����
            photonView.RPC("ApplyUpdatedHealth", RpcTarget.Others, health, dead);
            photonView.RPC("RestoreHealth", RpcTarget.Others, newHealth);
        }
        
    }
    public virtual void Die()
    {
        if (onDeath != null)    // onDeath �̺�Ʈ�� �����Ǿ� �ִٸ�
        {
            onDeath();  // �̺�Ʈ ȣ��
        }
        dead = true;
        //gameObject.SetActive(false);    // ���ӿ�����Ʈ ��Ȱ��ȭ
    }

}
