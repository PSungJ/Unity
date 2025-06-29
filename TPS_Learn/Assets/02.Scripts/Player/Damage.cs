using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Damage : MonoBehaviour
{
    [SerializeField] ParticleSystem blood;
    private readonly string e_bulletTag = "E_BULLET";
    private const string enemyTag = "ENEMY";
    [SerializeField] private float initHp = 100f;
    private float curHp = 0f;
    public Image bloodScreen;
    public Image hpBar;
    private readonly Color initColor = new Color(0f, 1f, 0f, 1f);
    private WaitForSeconds bloodWs;

    //델리게이트 및 이벤트선언
    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;
    private void OnEnable()
    {
        GameManager.OnItemChange += UpdateSetUp;
    }
    void UpdateSetUp()
    {
        initHp = GameManager.Instance.gameData.hp;
        curHp += GameManager.Instance.gameData.hp - curHp;
    }
    void Start()
    {
        hpBar.color = initColor;
        initHp = GameManager.Instance.gameData.hp;
        curHp = initHp;
        blood.Stop();
        bloodWs = new WaitForSeconds(0.1f);
    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(e_bulletTag))
        {
            col.gameObject.SetActive(false);
            curHp -= 5f;
            DisplayHpBar();

            blood.Play();
            if (curHp <= 0f)
                PlayerDie();
            StartCoroutine(ShowBloodScreen());
        }

    }

    private void DisplayHpBar()
    {
        curHp = Mathf.Clamp(curHp, 0, 100f);
        hpBar.fillAmount = curHp / initHp;
        if (hpBar.fillAmount <= 0.3f)
            hpBar.color = Color.red;
        else if (hpBar.fillAmount <= 0.5f)
            hpBar.color = Color.yellow; ;
    }

    IEnumerator ShowBloodScreen()
    {
        bloodScreen.color = new Color(1f, 0f, 0f, Random.Range(0.25f, 0.35f));
        yield return bloodWs;
        bloodScreen.color = Color.clear;

    }
    void PlayerDie()
    {
        GameManager.Instance.isGameOver = true;
        OnPlayerDie();
        //플레이어가 죽었다고 적들 한테 알려주어야 함
        //GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        ////하이라키에서 ENEMY 태그를 갖고 있는 오브젝트들을 enemies 배열에 저장 
        //for (int i = 0; i < enemies.Length; i++)
        //{
        //    enemies[i].SendMessage("OnPlayerDie",SendMessageOptions.DontRequireReceiver);
        //                    //해당오브젝트 함수호출, 해당 함수가 없거나 오타가 있어도 오류를 발생시키지 않는 옵션
        //}
        //위에 로직은 적이 너무 많거나 하면 동시에 전달을 못받을 가능성이 크다
        //동시에 플레이어가 사망 했다고 알려줘야 하기 때문에 delegate를 사용 해야 한다. 

    }

}
