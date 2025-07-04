using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum State
    {
        READY, FIRE, RELOAD
    }
    public State state = State.READY;
    public GunData gunData;

    private AudioSource source;
    public AudioClip shotClip;
    public AudioClip reloadClip;
    public float damage = 20f;      // �Ѿ� ������
    public int magCapacity = 25;    // źâ �뷮
    public float timeBetFire = 0.1f;// �߻� ����
    public float reloadTime = 1.8f; // ������ �ð�

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }
}
