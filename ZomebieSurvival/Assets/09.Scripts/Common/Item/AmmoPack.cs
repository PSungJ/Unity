using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPack : MonoBehaviour, I_Item
{
    public int ammo = 30;
    public void Use(GameObject target)
    {
        WomanShooter womanShooter = target.GetComponent<WomanShooter>();    // WomanShooter ������Ʈ ��������
        // WomanShooter ������Ʈ�� ������, gun ������Ʈ�� �����ϸ�
        if (womanShooter != null && womanShooter.gun != null)
        {
            womanShooter.gun.ammoRemain += ammo;    // gun�� ���� źȯ���� ammo��ŭ ���Ѵ�.
        }
        // ������ ���Ǿ����Ƿ� �ı�
        Destroy(gameObject);
    }

}
