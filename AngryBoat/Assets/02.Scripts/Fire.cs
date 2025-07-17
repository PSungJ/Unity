using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public Transform firePos;
    public GameObject bulletPrefab;
    public ParticleSystem muzzleFlash;
    private PhotonView pv = null;
    private PlayerInput input;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        input = GetComponent<PlayerInput>();
        muzzleFlash = firePos.GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        if (pv.IsMine && input.isMouseClick)
        {
            FireBullet();
            pv.RPC("FireBullet", RpcTarget.Others);
            // RpcTarget.AllViaServer, RpcTarget.AllBufferedViaServer
            /* ���� RpcTarget ����� RPCȣ�� �� ���ü��� �ʿ��� ���
            ����Ŭ���� �������� ������ �ִ� ��� ��Ʈ��ũ �������� ���ÿ� RPC�� �����Ѵ�.
            ������ ��Ÿ� �ӵ��� ���� õ�������̶� �ٻ�ġ�� �ش��Ѵٰ� �����ϸ� �ȴ�*/
        }
    }

    [PunRPC]
    void FireBullet()
    {
        if (!muzzleFlash.isPlaying)
            muzzleFlash.Play();

        GameObject bullet = Instantiate(bulletPrefab, firePos.position, firePos.rotation);
    }
}
