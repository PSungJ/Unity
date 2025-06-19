using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerCtrl : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private CircleCollider2D circle2D;
    [SerializeField] private Animator ani;
    [SerializeField] private Rigidbody2D rig2D;
    private AudioClip deathClip;
    private AudioClip jumpClip;
    private float jumpForce = 500f;
    private int jumpCount = 0;
    private bool isDead = false;
    private bool isGrounded = false;
    private readonly string GroundTag = "isJump";
    private readonly string dieTag = "Die";
    void Start()
    {
        deathClip = Resources.Load("die") as AudioClip;
        jumpClip = Resources.Load("jump") as AudioClip;
        source = GetComponent<AudioSource>();
        circle2D = GetComponent<CircleCollider2D>();
        ani = GetComponent<Animator>();
        rig2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isDead) return;
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                AndroidInput();
                break;
            case RuntimePlatform.WindowsEditor:
                PcTest();
                break;
        }
        ani.SetBool(GroundTag, isGrounded);
    }

    private void AndroidInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);    // 최초 터치 위치정보를 가져옴
            if (touch.phase == TouchPhase.Began && jumpCount < 2)   // 터치 시작순간 && 최대 점프횟수
            {
                ani.SetTrigger("Jump");
                ani.SetBool(GroundTag, !isGrounded);
                jumpCount++;
                rig2D.velocity = Vector2.zero;
                rig2D.AddForce(new Vector2(0, jumpForce));
                source.PlayOneShot(jumpClip);
            }
            else if (touch.phase == TouchPhase.Ended && rig2D.velocity.y > 0)
            {
                rig2D.velocity = rig2D.velocity * 0.5f;
            }
        }
    }

    private void PcTest()
    {
        if (Input.GetMouseButtonDown(0) && jumpCount < 2)
        {
            ani.SetTrigger("Jump");
            ani.SetBool(GroundTag, !isGrounded);
            jumpCount++;
            rig2D.velocity = Vector2.zero;
            // 점프직전에 속도를 0으로 초기화
            rig2D.AddForce(new Vector2(0, jumpForce));
            // 위쪽으로 힘을 가함
            source.PlayOneShot(jumpClip);
        }
        else if (Input.GetMouseButtonUp(0) && rig2D.velocity.y > 0)
        {
            //ani.SetBool(GroundTag, false);
            // 좌클릭 버튼을 떼면서 속도의 y값이 양수라면(=위로 올라가는 중이라면)
            rig2D.velocity = rig2D.velocity * 0.5f;
            // 속도를 반으로 줄임
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Dead") && !isDead)
        {
            Die();
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.contacts[0].normal.y > 0.7)
        {                   /* 표면의 노말벡터 방향이 위로 향하는지 확인
                            만약 어떤 표면의 노멀벡터 y값이 0.7보다 크다면(즉, 거의 수직에 가깝다면)
                            대략 45도의 경사를 가진채 표면이 위로 향한다는 의미
                            이렇게 하는 이유는 플레이어가 경사면에서 점프할 때 발판에 잘 닿도록 하기 위함*/
            isGrounded = true;
            jumpCount = 0;  // 땅에 닿으면 점프횟수 초기화
        }
    }
    private void OnCollisionExit2D(Collision2D col)
    {
        isGrounded= false;
    }
    private void Die()
    {
        isDead = true;
        ani.SetTrigger(dieTag);
        source.clip = deathClip;
        source.Play();
        rig2D.velocity = Vector2.zero;
        isDead = true;
        GameManager.instance.OnPlayerDie();
    }
}
