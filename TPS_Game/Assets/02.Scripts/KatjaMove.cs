using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KatjaMove : MonoBehaviour
{
    public string moveZAxisName = "Vertical";
    public string moveXAxisName = "Horizontal";

    public float moveZ { get; private set; }
    public float moveX { get; private set; }

    public Transform G_Pos;
    public Slider hpBar;
    public TMP_Text hpTxt;
    private Animator ani;
    private float Speed = 0f;

    public GameObject GrenadePrefab;

    void Start()
    {
        ani = GetComponent<Animator>();        
    }

    void Update()
    {
        ViewHp();
        ShowAni();
        MoveInput();
        Movement();
        ThrowGrenade();
    }

    void ViewHp()
    {
        hpTxt.text = $"HP:{((int)(hpBar.value * 100)).ToString()}";
    }

    void ShowAni()
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

    void MoveInput()
    {
        moveZ = Input.GetAxis(moveZAxisName);
        moveX = Input.GetAxis(moveXAxisName);

        if (moveX != 0 || moveZ != 0)
        {
            Speed = 5f;
            ani.SetBool("isMove", true);
        }
        else
        {
            ani.SetBool("isMove", false);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            Speed = 10f;
            ani.SetFloat("Move", 1);
        }
        else
        {
            ani.SetFloat("Move", 0);
        }
    }

    void Movement()
    {
        Vector3 moveDir = (Vector3.forward * moveZ * Time.deltaTime) + (Vector3.right * moveX * Time.deltaTime);
        transform.Translate(moveDir.normalized * Speed * Time.deltaTime);
    }

    void ThrowGrenade()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ani.SetTrigger("Throw");
            Instantiate(GrenadePrefab, G_Pos.position, Quaternion.identity);
        }
        Destroy(GrenadePrefab, 6f);
    }
}
