using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    float h, v; // 조이스틱 방향 변수
    [SerializeField] private Animator ani;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CapsuleCollider capCol;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip swordClip;
    [SerializeField] private AudioClip[] footSteps;
    private readonly string speedName = "Speed";
    private readonly int hashCombo = Animator.StringToHash("ComboAttack");
    private readonly int hashAttack = Animator.StringToHash("Attack");
    private readonly int hashDash = Animator.StringToHash("Dash");
    float lastSkillTime, lastDashTime, lastAttackTime;
    bool isAttack, isDash, isSkill = false;

    IEnumerator Start()
    {
        yield return null;
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        capCol = GetComponent<CapsuleCollider>();
        source = GetComponent<AudioSource>();
        swordClip = Resources.Load<AudioClip>("Sounds/coconut_throw");
        footSteps = Resources.LoadAll<AudioClip>("Sounds/Steps");
    }
    public void OnStickPos(Vector3 stickPos)
    {
        h = stickPos.x;
        v = stickPos.y;
    }
    public void OnAttackDown()
    {
        isAttack = true;
        //ani.SetBool(hashCombo, isAttack);
        ani.SetTrigger(hashAttack);
        StartCoroutine(ComboAttackCycle());
    }
    IEnumerator ComboAttackCycle()
    {
        if (Time.time - lastAttackTime > 1f)
        {
            lastAttackTime = Time.time;
            while (isAttack)
            {
                ani.SetBool(hashCombo, true);
                yield return new WaitForSeconds(1f);
            }
        }
    }
    public void OnAttackUp()
    {
        isAttack = false;
        ani.SetBool(hashCombo, isAttack);
    }
    public void OnSkillDown()
    {
        isSkill = true;
        ani.SetBool(hashCombo, false);
        if (Time.time - lastSkillTime > 1f)
        {
            ani.SetTrigger("Skill");
            lastSkillTime = Time.time;
        }
    }
    public void OnSkillUp()
    {
        isSkill = false;
    }
    public void OnDashDown()
    {
        isDash = true;
        if (Time.time - lastDashTime > 1f)
        {
            ani.SetTrigger(hashDash);
            lastDashTime = Time.time;
        }
    }
    public void OnDashUp()
    {
        isDash = false;
    }
    void Update()
    {
        if (ani != null)
        {
            ani.SetFloat(speedName, (h * h + v * v));
            if (rb != null)
            {
                Vector3 speed = rb.velocity;    // 속력
                speed.x = 4 * h;
                speed.z = 4 * v;
                rb.velocity = speed;
                if(h != 0 && v != 0)
                {
                    transform.rotation = Quaternion.LookRotation(new Vector3(h, 0, v)); // 로컬좌표, 특정 방향을 바라보도록 할 때
                                                                                        //Quaternion.Euler(0, 0, 0); // 절대좌표, 특정 각도로 오브젝트를 생성할 때
                }
            }
        }
    }
    public void SwordSound()
    {
        source.clip = swordClip;
        source.Play();
    }
    public void StepSound()
    {
        source.clip = footSteps[Random.Range(0, footSteps.Length)];
        source.Play();
    }
}
