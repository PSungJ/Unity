using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 콜라이더에 부딪혔을 때 라이트가  켜지도록 구현 
//콜라이더 밖에 나가면 라이트가 꺼지도록 구현
public class LightOnOff : MonoBehaviour
{
    public Light _light;
    private AudioSource source; //오디오 소스
    public AudioClip _lightOnSound; //오디오   클립
    public AudioClip _lightOffSound;
    void Start()
    {
        _light = GetComponent<Light>();
        source = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other) //istrriger체크 되고 콜라이더안에 들어왔을 때
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _light.enabled = true;
            source.PlayOneShot(_lightOnSound);
        }
    }
    private void OnTriggerExit(Collider other)//istrriger체크 되고 콜라이더를 빠져 나갔을 때
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _light.enabled = false;
            source.PlayOneShot(_lightOffSound);
        }
    }
}

