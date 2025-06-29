using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpBar : MonoBehaviour
{
    private Camera uiCamera;
    private Canvas uiCanvas;
    private RectTransform rectParent;
    private RectTransform rectHp;
    [HideInInspector] public Vector3 offset = Vector3.zero;
    [HideInInspector] public Transform targetTr;
    void Start()
    {
        uiCanvas = GetComponentInParent<Canvas>();
        uiCamera = uiCanvas.worldCamera;
        rectParent = uiCanvas.GetComponent<RectTransform>();
        rectHp = GetComponent<RectTransform>();

    }
    void LateUpdate()
    {
        // 월드좌표를 스크린 좌표로 변환
        var screenPos = Camera.main.WorldToScreenPoint(targetTr.position + offset);
        //카메라의 뒷쪽 영역(180도) 회전일때 좌표값을 고침 보정
        if (screenPos.z < 0.0f)
            screenPos.z *= -1.0f;

        var localPos = Vector2.zero;
        //스크린 좌표를 RectTransform 기준의 좌표로 변환            
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent  //부모트랜스폼
                                                                 , screenPos,//월드에서 스크린좌표로 변환 한 값
                                                                 uiCamera,  //UI카메라
                                                                 out localPos); // 로컬 좌표로 변환 된값
        rectHp.localPosition = localPos;



    }
}
