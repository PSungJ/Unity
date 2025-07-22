using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HpCtrl : MonoBehaviour
{
    public Slider hpBar;
    public TMP_Text hpTxt;
    private Animator ani;
    float temp = 1f;

    void Start()
    {
        ani = GetComponent<Animator>();
        ViewHp();
    }

    void Update()
    {
        ViewHp();
        ShowAnimation();
        Move();
        ThorwAction();
    }

    void ViewHp()
    {
        hpTxt.text = $"HP:{((int)(hpBar.value * 100f)).ToString()}";
    }

    void ShowAnimation()
    {
        if (hpBar.value > 0.5f)
        {
            ani.SetLayerWeight(1, 0);
        }
        else
        {
            ani.SetLayerWeight(1, 1);
        }
    }

    void Move()
    {
        if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            ani.SetBool("isMove", true);
        }
        else
        {
            ani.SetBool("isMove", false);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            ani.SetFloat("Move", 1);
        }
        else
        {
            ani.SetFloat("Move", 0);
        }
    }

    void ThorwAction()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ani.SetTrigger("Throw");
        }

        //if (ani.GetCurrentAnimatorStateInfo(2).normalizedTime > 0.75f)
        //{
        //    if (temp >= 0)
        //    {
        //        temp -= Time.deltaTime;
        //    }
        //    ani.SetLayerWeight(2, temp);
        //}
    }
}
