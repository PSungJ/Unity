using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Player = Photon.Realtime.Player;

public class Damage : MonoBehaviourPunCallbacks
{
    private readonly int hashRespawn = Animator.StringToHash("Respawn");
    private readonly int hashDie = Animator.StringToHash("Die");
    private readonly string bulletTag = "BULLET";

    [SerializeField] private Renderer[] renderers;
    private Animator ani;
    private CharacterController charCon;
    private int initHp = 100;
    private int curHp = 0;

    public GameManager manager;

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        ani = GetComponent<Animator>();
        charCon = GetComponent<CharacterController>();
        curHp = initHp;
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnCollisionEnter(Collision col)
    {
        if (curHp > 0 && col.collider.CompareTag(bulletTag))
        {
            curHp -= 20;
            if (curHp <= 0)
            {
                if (photonView.IsMine)
                {
                    var actorNum = col.collider.GetComponent<Bullet>().actorNumber; // ���� �Ѿ��� ActorNumber ����
                    Player lastShooterPlayer = PhotonNetwork.CurrentRoom.GetPlayer(actorNum);   // ActorNumber�� ���� ������ �÷��̾� ����
                    // �޼��� ����� ���� ���ڿ� ����
                    string msg = string.Format($"\n<color=#00ff00>{lastShooterPlayer.NickName}</color> killed <color=#ff0000>{photonView.Owner.NickName}</color>");
                    photonView.RPC("KillMessage", RpcTarget.AllBufferedViaServer, msg);
                }
                StartCoroutine(PlayerDie());
            }
        }
    }

    [PunRPC]
    void KillMessage(string msg)
    {
        manager.killLogMsg.text += msg;
    }

    IEnumerator PlayerDie()
    {
        charCon.enabled = false;
        ani.SetBool(hashRespawn, false);
        ani.SetTrigger(hashDie);
        
        yield return new WaitForSeconds(3f); // 3���� ���ó�� ������Ʈ ���� �� ������
        ani.SetBool(hashRespawn, true);
        SetVisible(false);
        
        yield return new WaitForSeconds(1.5f);  // ���� ��ġ ������
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length);
        transform.position = points[idx].position;
        SetVisible(true);
        curHp = initHp;
        charCon.enabled = true;
    }

    void SetVisible(bool isVisible)
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = isVisible;
        }
    }
}
