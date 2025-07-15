using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum State
    {
        READY = 0 , FIRE = 1, RELOAD = 2, EMPTY = 3
    }
    //public State state = State.READY;
    public State state {  get; private set; } = State.READY;
    public Transform firepos;
    public ParticleSystem muzzleFlash;  // �ѱ� ȭ�� ����Ʈ
    public ParticleSystem shellEject;   // ź�� ���� ����Ʈ
    private LineRenderer lineRenderer;  // �Ѿ� ����

    public GunData gunData; // �� ������ Scriptable Object
    private float fireDistance = 100f;  // �Ѿ��� ���ư��� �Ÿ�(�����Ÿ�)
    private AudioSource source;

    public int ammoRemain;      // �����ִ� ��ü �Ѿ� ��
    public int magAmmo;         // ���� źâ�� �����ִ� �Ѿ� ��
    private float lastFireTime; // ������ �Ѿ� �߻� �ð�
    private Vector3 hitPosition;// �Ѿ� Ÿ�� ����
    private WaitForSeconds shotEffectWS;
    private WaitForSeconds reloadWS;

    // GunData ScriptableObject�� �������� �����ؼ� �ʿ���� ����
    //public float damage = 20f;      // �Ѿ� ������
    //public int magCapacity = 25;    // źâ �뷮
    //public float timeBetFire = 0.1f;// �߻� ����
    //public float reloadTime = 1.8f; // ������ �ð�
    //public AudioClip shotClip;
    //public AudioClip reloadClip;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2; // ������ ������ 2�� ����(�������� ����) ������ �������� Ȥ�� ������ �ִ�Ÿ�
        lineRenderer.enabled = false;   // ó���� ��Ȱ��ȭ
        shotEffectWS = new WaitForSeconds(0.03f);
        reloadWS = new WaitForSeconds(gunData.reloadTime);
    }

    private void OnEnable()
    {
        // �� ���� �ʱ�ȭ
        ammoRemain = gunData.startAmmoRemain;   // ���� ���� ��ü ź���� �ʱ�ȭ
        magAmmo = gunData.magCapacity;      // ���� źâ�� �����ִ� �Ѿ� ���� �ʱ�ȭ
        state = State.READY;        // ���� ���¸� READY�� �ʱ�ȭ
        lastFireTime = 0f;          // ������ �߻�ð� �ʱ�ȭ
    }

    public void Fire()
    {
        // �߻� ���� ���ǰ˻� �Լ�
        if (state == State.READY && Time.time >= lastFireTime + gunData.timeBetTime)
        {
            lastFireTime = Time.time;   // ���� �ð��� ������ �߻� �ð����� ����
            Shot(); // ���� �߻�ó�� �Լ� ȣ��
        }
    }

    private void Shot()
    {
        // ���� �߻� ó�� �Լ�
        RaycastHit hit;
        Vector3 hitPos = Vector3.zero;
        if (Physics.Raycast(firepos.position, firepos.forward, out hit, fireDistance))
        {
            I_Damageable target = hit.collider.GetComponent<I_Damageable>();
            // �浹�� ������Ʈ���� �������̽��� ã��
            if(target != null)
            {
                target.OnDamage (gunData.damage, hit.point, hit.normal);
                // �浹�� ������Ʈ�� I_Damageable�� �����ϰ� �ִٸ� ������ ó��
            }
            hitPos = hit.point; // �浹 ���� ����
        }
        else
        {
            hitPos = firepos.position + firepos.forward * fireDistance;
            // �浹�� ������ �����Ÿ� �� �������� ����
        }
        StartCoroutine(ShotEffect(hitPos)); // �߻� ����Ʈ �ڷ�ƾ ����
        magAmmo--;
        if (magAmmo <= 0)
        {
            state = State.EMPTY;    // źâ�� 0������ �� ����ִ� ���·� ����
        }
    }

    IEnumerator ShotEffect(Vector3 hitPosition)
    {
        // �߻� ����Ʈ �ڷ�ƾ
        muzzleFlash.Play();
        shellEject.Play();      // �� �߻�� ��ƼŬ ���
        source.PlayOneShot(gunData.shotClip ,1.0f);  // gunData�� �߻� ���� ȣ�� �� ȣ��� ���� ���
        lineRenderer.SetPosition(0, firepos.position);  // LineRenderer ������ ����
        lineRenderer.SetPosition(1, hitPosition);      // LineRenderer ���� ����
        lineRenderer.enabled = true;    // ���η����� Ȱ��ȭ

        yield return shotEffectWS;
        lineRenderer.enabled = false;   // 0.03�� �� ��Ȱ��ȭ
    }

    public bool Reload()
    {
        // ������ �õ� Ȯ�� �Լ�
        if (state == State.RELOAD || ammoRemain <= 0 || magAmmo >= gunData.magCapacity)
        {
            return false;   // �̹� ������ ���̰ų�, ���� �Ѿ��� ���ų�, źâ�� ���� á�ٸ� ������ �Ұ���
        }
        StartCoroutine(ReloadRoution());
        return true;    // ������ ����
    }

    IEnumerator ReloadRoution()
    {
        // ������ �ڷ�ƾ
        state = State.RELOAD;   // ������ ���·� ����
        source.PlayOneShot(gunData.reloadClip, 1.0f);

        yield return reloadWS;    // gunData ScriptableObject�� �� �ҷ�����

        // źâ�� ä���� �� �Ѿ� �� ���
        int ammoToFill = gunData.magCapacity - magAmmo;
        // źâ�� ä���� �� ź���� ���� ź�˺��� ���ٸ�, ä���� �� ź�� ���� ���� ź�� ���� ���� ����
        if (ammoRemain < ammoToFill)
        {
            ammoToFill = ammoRemain;
        }
        magAmmo += ammoToFill;  // źâ�� �Ѿ� ä���
        ammoRemain -= ammoToFill;   // ���� �Ѿ� �� ����
        state = State.READY;    // ���¸� �߻� �غ���·� ��ȯ
    }
}
