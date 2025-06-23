using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public Transform shakeCamera;   //흔들림 효과를 줄 카메라
    public bool shakeRotate = false; //회전 여부를 판단하는 bool변수
    private Vector3 originPos = Vector3.zero;  //Shake하고나서 원래 위치를 되돌릴 벡터 변수
    private Quaternion originRot = Quaternion.identity; //Shake하고나서 원래 회전을 되돌릴 벡터 변수
    void Start()
    {
        originPos = shakeCamera.transform.position;
        originRot = shakeCamera.transform.rotation;
    }
    public IEnumerator ShakeCamera(float duration = 0.05f, 
        float magnitudePos = 0.03f, 
        float magnitudeRot = 0.1f)
    {
        float passTime = 0.0f;  // 시간을 누적할 변수
        //Shake 시간동안 루프를 순회하기 위해
        while (passTime < duration)
        {   //불규칙한 위치를 추출
            Vector3 shakePos = Random.insideUnitSphere;
            shakeCamera.transform.position = shakePos * magnitudePos;
            if (shakeRotate)
            {
                Vector3 shakeRot = new Vector3(0f, 0f, Mathf.PerlinNoise(Time.time * magnitudeRot, 0f));
                shakeCamera.rotation = Quaternion.Euler(shakeRot);
            }
            passTime += Time.deltaTime; // Shake시간을 누적
            yield return null;
        }
        //Shake가 끝난후 원래 위치로 초기화
        shakeCamera.position = originPos;
        shakeCamera.rotation = originRot;
    }
}
