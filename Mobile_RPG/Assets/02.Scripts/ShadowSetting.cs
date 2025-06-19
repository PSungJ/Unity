using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShadowSetting : MonoBehaviour
{
    [SerializeField] private Dropdown dropDown;
    [SerializeField] private Light _light;
    void Start()
    {
        dropDown = GameObject.Find("Canvas-UI").transform.GetChild(4).GetChild(2).GetChild(5).GetComponent<Dropdown>();
        _light = GameObject.Find("Directional Light").GetComponent<Light>();
        dropDown.onValueChanged.AddListener(OnShadowDropValue);
    }
    // �̺�Ʈ ������ : Ư�� �̺�Ʈ�� �߻��ϸ� ������ �Լ��� ����
    // ���ص� �Ǵ� ���� : ������Ʈ�� ���� �ʿ� ���� �ش��Լ��� ã�� �ʿ䰡 ����.
    public void OnShadowDropValue(int value)
    {
        if(dropDown != null && _light != null)
        {
            switch (value)
            {
                case 0:
                    _light.shadows = LightShadows.None; 
                    break;
                case 1:
                    _light.shadows = LightShadows.Soft;
                    break;
                case 2:
                    _light.shadows = LightShadows.Hard;
                    break;
            }
        }
        else
        {
            Debug.LogError("Directional Light, Dropdown �� �ϳ��� ����");
        }
    }
}
