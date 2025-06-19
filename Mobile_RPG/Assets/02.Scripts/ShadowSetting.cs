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
    // 이벤트 리스너 : 특정 이벤트가 발생하면 지정된 함수를 실행
    // 안해도 되는 사항 : 오브젝트를 넣을 필요 없고 해당함수를 찾을 필요가 없다.
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
            Debug.LogError("Directional Light, Dropdown 중 하나가 없음");
        }
    }
}
