using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AmmoPack : MonoBehaviourPun, I_Item
{
    public int ammo = 30;
    public void Use(GameObject target)
    {
        WomanShooter womanShooter = target.GetComponent<WomanShooter>();    // WomanShooter ������Ʈ ��������
        // WomanShooter ������Ʈ�� ������, gun ������Ʈ�� �����ϸ�
        if (womanShooter != null && womanShooter.gun != null)
        {
            womanShooter.gun.photonView.RPC("AddAmmo", RpcTarget.All, ammo);    // gun�� ���� źȯ���� ammo��ŭ ���Ѵ�.
        }
        // ������ ���Ǿ����Ƿ� �ı�
        PhotonNetwork.Destroy(gameObject);
    }

}
