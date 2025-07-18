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
                    var actorNum = col.collider.GetComponent<Bullet>().actorNumber; // 맞은 총알의 ActorNumber 추출
                    Player lastShooterPlayer = PhotonNetwork.CurrentRoom.GetPlayer(actorNum);   // ActorNumber로 현재 입장한 플레이어 추출
                    // 메세지 출력을 위한 문자열 포맷
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
        
        yield return new WaitForSeconds(3f); // 3초후 사망처리 오브젝트 삭제 후 리스폰
        ani.SetBool(hashRespawn, true);
        SetVisible(false);
        
        yield return new WaitForSeconds(1.5f);  // 생성 위치 재조정
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
