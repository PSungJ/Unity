using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WomanHealth : LivingEntity
{
    private readonly int hashDie = Animator.StringToHash("Die");
    public Slider healthSlider;     // 체력 슬라이더 UI
    public AudioClip deathClip;     // 죽을 때 사운드 클립
    public AudioClip hitClip;       // 피격 사운드 클립
    public AudioClip itemPickUpClip;// 아이템 획득 사운드 클립
    private AudioSource source;
    private Animator ani;
    private WomanMovement movement; // 캐릭터 이동 스크립트
    private WomanShooter shooter;   // 캐릭터 슈팅 스크립트
    private void Awake()
    {
        source = GetComponent<AudioSource>();
        movement = GetComponent<WomanMovement>();
        shooter = GetComponent<WomanShooter>();
    }
    protected override void OnEnable()  // 부모 클래스에서는 논리적 기능만 구현되어 있으므로, 자식 클래스에서 논리적기능에 의거하여 UI를 구현한다(다형성)
    {
        base.OnEnable();    // 부모 클래스의 OnEnable 호출
        healthSlider.gameObject.SetActive(true);
        healthSlider.maxValue = startingHealth;
        healthSlider.value = health;

        movement.enabled = true;
        shooter.enabled = true;     // 캐릭터의 이동, 슈팅 컴포넌트 활성화
    }
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!dead)  // 죽지 않은 상태라면
            source.PlayOneShot(hitClip);    // 피격 사운드 재생
        
        base.OnDamage(damage, hitPoint, hitNormal);
        healthSlider.value = health;
    }
    public override void RestoreHealth(float newHealth)
    {
        base.RestoreHealth(newHealth);
        // 캐릭터가 체력 회복시 추가적인 로직이 필요하다면 여기에 작성해준다.
        healthSlider.value = health;
    }
    public override void Die()
    {
        base.Die();
        healthSlider.gameObject.SetActive(false);   // 체력 슬라이더 UI 비활성화
        source.PlayOneShot(deathClip);
        ani.SetTrigger(hashDie);
        movement.enabled = false;
        shooter.enabled = false;        // 캐릭터 이동, 슈팅 스크립트 비활성화
    }
    private void OnTriggerEnter(Collider other) //isTrigger
    {
        if (!dead)
        {
            I_Item item = other.GetComponent<I_Item>(); // I_Item 인터페이스를 구현한 컴포넌트 호출
            if (item != null)
            {
                item.Use(gameObject);
                source.PlayOneShot(itemPickUpClip);
            }
        }
    }
}
