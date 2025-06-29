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

    //��������Ʈ �� �̺�Ʈ����
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
        //�÷��̾ �׾��ٰ� ���� ���� �˷��־�� ��
        //GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        ////���̶�Ű���� ENEMY �±׸� ���� �ִ� ������Ʈ���� enemies �迭�� ���� 
        //for (int i = 0; i < enemies.Length; i++)
        //{
        //    enemies[i].SendMessage("OnPlayerDie",SendMessageOptions.DontRequireReceiver);
        //                    //�ش������Ʈ �Լ�ȣ��, �ش� �Լ��� ���ų� ��Ÿ�� �־ ������ �߻���Ű�� �ʴ� �ɼ�
        //}
        //���� ������ ���� �ʹ� ���ų� �ϸ� ���ÿ� ������ ������ ���ɼ��� ũ��
        //���ÿ� �÷��̾ ��� �ߴٰ� �˷���� �ϱ� ������ delegate�� ��� �ؾ� �Ѵ�. 

    }

}
