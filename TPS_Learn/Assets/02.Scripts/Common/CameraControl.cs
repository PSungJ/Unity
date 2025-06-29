
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraControl : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCamera vc;
    [SerializeField]
    CinemachineBasicMultiChannelPerlin noise;
    
    void Awake()
    {
        vc = GetComponent<CinemachineVirtualCamera>();
        
    }
    
    void Start()
    {
        noise = vc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        StopCameraShake();
        BarrelCtrl.OnShake += this.ShakeCamera;
    }
    
 
    public void ShakeCamera()
    {
        noise.m_AmplitudeGain = 5f;
        noise.m_FrequencyGain = 3f;
        Invoke("StopCameraShake", 1.5f);
        
    }
   

    public void StopCameraShake()
    {
        noise.m_AmplitudeGain = 0f;
        noise.m_FrequencyGain = 0f;
    }
}
