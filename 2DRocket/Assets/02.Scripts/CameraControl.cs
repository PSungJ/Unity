using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera vc;
    [SerializeField] CinemachineBasicMultiChannelPerlin noise;
    void Awake()
    {
        vc = GetComponent<CinemachineVirtualCamera>();
        GameManager.OnShake += this.ShakeCamera;
    }
    void Start()
    {
        noise = vc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        StopCameraShake();
    }
    public void ShakeCamera()
    {
        noise.m_AmplitudeGain = 3f;
        noise.m_FrequencyGain = 1f;
        Invoke("StopCameraShake", 0.5f);
    }
    public void StopCameraShake()
    {
        noise.m_AmplitudeGain = 0f;
        noise.m_FrequencyGain = 0f;
    }
}
