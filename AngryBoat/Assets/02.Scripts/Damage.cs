using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Damage : MonoBehaviourPun
{
    private readonly int hashRespawn = Animator.StringToHash("Respawn");
    private readonly int hashDie = Animator.StringToHash("Die");
    private readonly string bulletTag = "BULLET";

    [SerializeField] private Renderer[] renderers;
    private Animator ani;
    private CharacterController charCon;
    private int initHp = 100;
    private int curHp = 0;

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        ani = GetComponent<Animator>();
        charCon = GetComponent<CharacterController>();
        curHp = initHp;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (curHp > 0 && col.collider.CompareTag(bulletTag))
        {
            curHp -= 20;
            if (curHp <= 0)
            {
                StartCoroutine(PlayerDie());
            }
        }
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
