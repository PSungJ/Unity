using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Transform CanvarTr;
    public Transform CameraTr;

    void Start()
    {
        CanvarTr = this.transform;
        CameraTr = Camera.main.transform; //메인 카메라의 트랜스폼을 가져옴
    }
    void Update()
    {
        CanvarTr.LookAt(CameraTr); //캔버스가 카메라를 바라보도록 설정
    }
}
