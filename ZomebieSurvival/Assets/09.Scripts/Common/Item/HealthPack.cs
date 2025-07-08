using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour, I_Item
{
    public int health = 50;
    public void Use(GameObject target)
    {
        LivingEntity life = target.GetComponent<LivingEntity>();    // 전달받은 게임 오브젝트로부터 LivingEntity 컴포넌트 가져오기

        if (life != null)   // LivingEntity 컴포넌트가 있다면
        {
            life.RestoreHealth(health); // 체력 회복 실행
        }
        Destroy(gameObject);
    }
}
