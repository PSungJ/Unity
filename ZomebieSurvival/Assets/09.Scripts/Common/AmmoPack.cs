using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPack : MonoBehaviour, I_Item
{
    public int ammo = 30;
    public void Use(GameObject target)
    {
        // ź���� �߰�
        Debug.Log($"ź������ {ammo}" );
    }

}
