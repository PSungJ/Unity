using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPack : MonoBehaviour, I_Item
{
    public int ammo = 30;
    public void Use(GameObject target)
    {
        WomanShooter womanShooter = target.GetComponent<WomanShooter>();    // WomanShooter 컴포넌트 가져오기
        // WomanShooter 컴포넌트가 있으며, gun 오브젝트가 존재하면
        if (womanShooter != null && womanShooter.gun != null)
        {
            womanShooter.gun.ammoRemain += ammo;    // gun의 남은 탄환수를 ammo만큼 더한다.
        }
        // 아이템 사용되었으므로 파괴
        Destroy(gameObject);
    }

}
