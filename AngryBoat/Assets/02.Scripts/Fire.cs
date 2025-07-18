using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Fire : MonoBehaviour
{
    public Transform firePos;
    public GameObject bulletPrefab;
    public ParticleSystem muzzleFlash;
    private PhotonView pv = null;
    private PlayerInput input;
    private float fireRate = 0.1f;
    private float nextFire = 0f;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        input = GetComponent<PlayerInput>();
        muzzleFlash = firePos.GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (pv.IsMine && input.isMouseClick && Time.time >= nextFire)
        {
            nextFire = Time.time + fireRate;
            FireBullet(pv.Owner.ActorNumber);
            pv.RPC("FireBullet", RpcTarget.Others, pv.Owner.ActorNumber);
            // RpcTarget.AllViaServer, RpcTarget.AllBufferedViaServer
            /* ���� RpcTarget ����� RPCȣ�� �� ���ü��� �ʿ��� ���
            ����Ŭ���� �������� ������ �ִ� ��� ��Ʈ��ũ �������� ���ÿ� RPC�� �����Ѵ�.
            ������ ��Ÿ� �ӵ��� ���� õ�������̶� �ٻ�ġ�� �ش��Ѵٰ� �����ϸ� �ȴ�*/
        }
    }

    [PunRPC]
    void FireBullet(int actorNumber)
    {
        if (!muzzleFlash.isPlaying)
            muzzleFlash.Play();

        GameObject bullet = Instantiate(bulletPrefab, firePos.position, firePos.rotation);
        
        bullet.GetComponent<Bullet>().actorNumber = actorNumber;
    }
}
