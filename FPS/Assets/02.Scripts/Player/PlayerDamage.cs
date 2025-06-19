using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//1.HP값 MAXHP값
//2IMAGE HPBAR TEXT
//3.OnTriggerEnter(Collider col) //충돌시
public class PlayerDamage : MonoBehaviour
{
    private float hp;
    private float maxHp = 100f; 
    public Image hpBar; 
    public Text hpText; 
    private readonly string punchTag = "PUNCH"; //적의 공격 태그  
    private readonly string swordTag = "SWORD"; //적의 검 공격 태그
    public GameObject BlindObj;
    void Start()
    {
        BlindObj = GameObject.Find("Canvas-UI").transform.GetChild(4).gameObject;
        hpBar = GameObject.Find("Canvas-UI").transform.GetChild(0).GetChild(0).GetComponent<Image>();
        hpBar.color = Color.green;
        hp = maxHp; //초기 체력을 최대 체력으로 설정
        //UI에서 HPBar 이미지 컴포넌트를 가져옴
        hpText = GameObject.Find("Canvas-UI").transform.GetChild(0).GetChild(1).GetComponent<Text>();

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(punchTag)) //적의 공격에 맞았을 때
        {
            hp -= 5; //체력 감소
            UIManager();

        }
        else if(other.CompareTag(swordTag))
        {
            hp -= 30; //체력 감소
            UIManager();
        }
     
        if (hp <= 0) //체력이 0 이하가 되면
        {
            PlayerDie(); //죽음 처리
        }

    }

    private void UIManager()
    {
        hp = Mathf.Clamp(hp, 0, maxHp); //체력을 0과 최대 체력 사이로 제한
        hpBar.fillAmount = hp / maxHp; //체력바 UI 업데이트
        hpText.text = $"HP :<color=#ff0000>{hp}</color>"; //체력 텍스트 UI 업데이트
        if (hpBar.fillAmount <= 0.3f)
            hpBar.color = Color.red; //체력바가 30% 이하일 때 빨간색으로 변경
        else if (hpBar.fillAmount <= 0.5f)
            hpBar.color = Color.yellow; //체력바가 50%일 때 노란색으로 변경
    }

    void PlayerDie()
    {
        BlindObj.SetActive(true);
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();   // 현재 오브젝트의 모든 MonoBehaviour 스크립트 가져오기
        foreach (var script in scripts)
        {
            script.enabled = false;     // 모든 스크립트 비활성화
        }
        Invoke("SceneMove", 3f);
    }
    void SceneMove()
    {
        SceneManager.LoadScene("EndScene");
    }
}
