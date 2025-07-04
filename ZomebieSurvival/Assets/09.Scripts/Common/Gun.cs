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
    public float damage = 20f;      // 총알 데미지
    public int magCapacity = 25;    // 탄창 용량
    public float timeBetFire = 0.1f;// 발사 간격
    public float reloadTime = 1.8f; // 재장전 시간

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }
}
