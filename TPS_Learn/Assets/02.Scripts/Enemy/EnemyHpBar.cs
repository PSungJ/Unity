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
        // ������ǥ�� ��ũ�� ��ǥ�� ��ȯ
        var screenPos = Camera.main.WorldToScreenPoint(targetTr.position + offset);
        //ī�޶��� ���� ����(180��) ȸ���϶� ��ǥ���� ��ħ ����
        if (screenPos.z < 0.0f)
            screenPos.z *= -1.0f;

        var localPos = Vector2.zero;
        //��ũ�� ��ǥ�� RectTransform ������ ��ǥ�� ��ȯ            
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent  //�θ�Ʈ������
                                                                 , screenPos,//���忡�� ��ũ����ǥ�� ��ȯ �� ��
                                                                 uiCamera,  //UIī�޶�
                                                                 out localPos); // ���� ��ǥ�� ��ȯ �Ȱ�
        rectHp.localPosition = localPos;



    }
}
