using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class FOVControl : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Slider fovSlider;
    void Start()
    {
        virtualCamera = GameObject.Find("Virtual Camera(Follow)").GetComponent<CinemachineVirtualCamera>();
        fovSlider = GameObject.Find("Canvas-UI").transform.GetChild(4).GetChild(2).GetChild(4).GetComponent<Slider>();

        if (fovSlider != null && virtualCamera != null)
        {
            virtualCamera.m_Lens.FieldOfView = fovSlider.value;
        }
        if (fovSlider != null)
        {
            fovSlider.onValueChanged.AddListener(OnFovSliderChanged);
        }
    }
    public void OnFovSliderChanged(float newFovValue)
    {
        if (virtualCamera != null)
        {
            // 가상 카메라의 FOV를 슬라이더의 새로운 값으로 설정합니다.
            virtualCamera.m_Lens.FieldOfView = newFovValue;
        }
    }
    void OnDestroy()
    {
        // 스크립트가 파괴될 때 이벤트 리스너를 제거하여 메모리 누수를 방지합니다.
        if (fovSlider != null)
        {
            fovSlider.onValueChanged.RemoveListener(OnFovSliderChanged);
        }
    }
}
