using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WomanHealth : LivingEntity
{
    private readonly int hashDie = Animator.StringToHash("Die");
    public Slider healthSlider;     // ü�� �����̴� UI
    public AudioClip deathClip;     // ���� �� ���� Ŭ��
    public AudioClip hitClip;       // �ǰ� ���� Ŭ��
    public AudioClip itemPickUpClip;// ������ ȹ�� ���� Ŭ��
    private AudioSource source;
    private Animator ani;
    private WomanMovement movement; // ĳ���� �̵� ��ũ��Ʈ
    private WomanShooter shooter;   // ĳ���� ���� ��ũ��Ʈ
    private void Awake()
    {
        source = GetComponent<AudioSource>();
        movement = GetComponent<WomanMovement>();
        shooter = GetComponent<WomanShooter>();
    }
    protected override void OnEnable()  // �θ� Ŭ���������� ���� ��ɸ� �����Ǿ� �����Ƿ�, �ڽ� Ŭ�������� ������ɿ� �ǰ��Ͽ� UI�� �����Ѵ�(������)
    {
        base.OnEnable();    // �θ� Ŭ������ OnEnable ȣ��
        healthSlider.gameObject.SetActive(true);
        healthSlider.maxValue = startingHealth;
        healthSlider.value = health;

        movement.enabled = true;
        shooter.enabled = true;     // ĳ������ �̵�, ���� ������Ʈ Ȱ��ȭ
    }
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!dead)  // ���� ���� ���¶��
            source.PlayOneShot(hitClip);    // �ǰ� ���� ���
        
        base.OnDamage(damage, hitPoint, hitNormal);
        healthSlider.value = health;
    }
    public override void RestoreHealth(float newHealth)
    {
        base.RestoreHealth(newHealth);
        // ĳ���Ͱ� ü�� ȸ���� �߰����� ������ �ʿ��ϴٸ� ���⿡ �ۼ����ش�.
        healthSlider.value = health;
    }
    public override void Die()
    {
        base.Die();
        healthSlider.gameObject.SetActive(false);   // ü�� �����̴� UI ��Ȱ��ȭ
        source.PlayOneShot(deathClip);
        ani.SetTrigger(hashDie);
        movement.enabled = false;
        shooter.enabled = false;        // ĳ���� �̵�, ���� ��ũ��Ʈ ��Ȱ��ȭ
    }
    private void OnTriggerEnter(Collider other) //isTrigger
    {
        if (!dead)
        {
            I_Item item = other.GetComponent<I_Item>(); // I_Item �������̽��� ������ ������Ʈ ȣ��
            if (item != null)
            {
                item.Use(gameObject);
                source.PlayOneShot(itemPickUpClip);
            }
        }
    }
}
