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
            // ���� ī�޶��� FOV�� �����̴��� ���ο� ������ �����մϴ�.
            virtualCamera.m_Lens.FieldOfView = newFovValue;
        }
    }
    void OnDestroy()
    {
        // ��ũ��Ʈ�� �ı��� �� �̺�Ʈ �����ʸ� �����Ͽ� �޸� ������ �����մϴ�.
        if (fovSlider != null)
        {
            fovSlider.onValueChanged.RemoveListener(OnFovSliderChanged);
        }
    }
}
