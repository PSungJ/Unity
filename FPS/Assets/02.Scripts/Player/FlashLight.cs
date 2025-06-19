using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//F키를 누르면 라이트가 켜지고 다시 F키를 누르면 꺼진다
// 사운드 구현도 같이 : audiosource, audioClip
public class FlashLight : MonoBehaviour
{
    [SerializeField] private Light flashLight;
    [SerializeField] private AudioSource source;
    public AudioClip flashClip;
    void Start()
    {
        flashLight = GetComponent<Light>();
        source = GetComponent<AudioSource>();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            flashLight.enabled = !flashLight.enabled;
            source.PlayOneShot(flashClip, 1.0f);
        }
          
        
    }
}
