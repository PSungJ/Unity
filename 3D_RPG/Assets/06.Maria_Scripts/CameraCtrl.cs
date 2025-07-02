using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    [Header("ī�޶� ����")]
    public Transform cameraTr;
    public Transform cameraPivotTr;
    public float cameraDistance = 5f;

    private PlayerInput input;

    void Start()
    {
        cameraDistance = 5f;
        cameraTr = Camera.main.transform;
        cameraPivotTr = cameraTr.parent;
    }
    public void CameraDistanceCtrl()
    {
        cameraDistance -= Input.GetAxis("Mouse ScrollWheel") * 2f;   // ���콺 �ٷ� ī�޶� �Ÿ� ����
        cameraDistance = Mathf.Clamp(cameraDistance, 2f, 7f);
    }
}
