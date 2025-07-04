using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Damageable
{
    void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);   // 데미지값, 맞은 위치, 맞은 방향
}
