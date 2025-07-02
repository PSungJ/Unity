using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    [Header("카메라 무빙")]
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
        cameraDistance -= Input.GetAxis("Mouse ScrollWheel") * 2f;   // 마우스 휠로 카메라 거리 조정
        cameraDistance = Mathf.Clamp(cameraDistance, 2f, 7f);
    }
}
