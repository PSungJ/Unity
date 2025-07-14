using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class TankDamage : MonoBehaviourPun
{
    [SerializeField] private MeshRenderer[] renderers;
    private readonly string tankTag = "TANK";
    private readonly string apacheTage = "APACHE";
    private readonly int InitHp = 100;
    private int curHp = 0;
    private GameObject expEffect = null;
    public Image hpBar;
    private WaitForSeconds ws;
    public Canvas tankCanvas;


    void Start()
    {
        renderers = GetComponentsInChildren<MeshRenderer>();
        expEffect = Resources.Load<GameObject>("Effects/BigExplosionEffect");
        curHp = InitHp;
        hpBar.color = Color.green;
        ws = new WaitForSeconds(5f);
    }

    public void OnDamage(string Tag)
    {
        photonView.RPC("OnDamageRPC", RpcTarget.All, Tag);
        // 원격지 함수 호출
        Debug.Log("데미지 전달");
    }
    [PunRPC]
    void OnDamageRPC(string Tag)
    {
        if (curHp > 0 && Tag == tankTag)
        {
            // 데미지 전달
            hpBarInit(Tag);
            if (curHp <= 0)
                StartCoroutine(ExplosionTank());
        }
    }

    IEnumerator ExplosionTank()
    {
        var eff = Instantiate(expEffect, transform.position, Quaternion.identity);
        Destroy(eff, 2f);
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        tankCanvas.enabled = false;
        SetTankVisible(false);
        this.gameObject.tag = "Untagged";

        yield return ws;
        SetTankVisible(true);
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
        curHp = InitHp;
        tankCanvas.enabled = true;
        hpBar.fillAmount = 1.0f;
        hpBar.color = Color.green;
    }

    void SetTankVisible(bool isVisible)
    {
        foreach (var meshR in renderers)
        {
            meshR.enabled = isVisible;
        }
    }

    void hpBarInit(string Tag)
    {
        if (Tag == tankTag)
            curHp -= 25;
        else
            curHp -= 1;

            hpBar.fillAmount = (float)curHp / (float)InitHp;
        if (hpBar.fillAmount <= 0.7f)
        {
            hpBar.color = Color.yellow;
        }
        else if (hpBar.fillAmount <= 0.3f)
        {
            hpBar.color = Color.red;
        }
    }
}
