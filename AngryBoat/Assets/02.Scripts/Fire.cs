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
            /* 위의 RpcTarget 방식은 RPC호출 시 동시성이 필요한 경우
            포톤클라우드 서버에서 접속해 있는 모든 네트워크 유저에게 동시에 RPC를 전송한다.
            하지만 통신망 속도에 따라 천차만별이라 근사치에 해당한다고 생각하면 된다*/
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
