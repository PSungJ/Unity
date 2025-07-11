using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager S_instance;
    public bool isMute = false;

    void Awake()
    {
        if (S_instance == null)
            S_instance = this;
        else if (S_instance != this)
            Destroy(gameObject);
    }

    public void PlaySfx(Vector3 pos, AudioClip clip, bool isLooped)
    {
        if (isMute) return;

        GameObject soundObj = new GameObject("SoundSFX~~");
        soundObj.transform.position = pos;
        AudioSource audioSource = soundObj.AddComponent<AudioSource>(); // 컴퍼넌트가 없으면 새로 생성
        audioSource.clip = clip;
        audioSource.loop = isLooped;
        audioSource.minDistance = 20f;
        audioSource.maxDistance = 100f;
        audioSource.volume = 1.0f;
        audioSource.Play();

        Destroy(soundObj, audioSource.clip.length);
    }

    public void PlayBGM(Vector3 pos, AudioClip clip, bool isLooped)
    {
        if (isMute) return;
        GameObject soundObj = new GameObject("BGMSound");
        soundObj.transform.position = pos;
        AudioSource audioSource = soundObj.AddComponent<AudioSource>(); // 컴퍼넌트가 없으면 새로 생성
        audioSource.clip = clip;
        audioSource.loop = isLooped;
        audioSource.minDistance = 20f;
        audioSource.maxDistance = 100f;
        audioSource.volume = 1.0f;
        audioSource.Play();
    }
}
