using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public Transform shakeCamera; //세이크 효과를 줄 카메라
    public bool shakeRotate = false; //회전 할 것인지를 판단 하는 불변수
    private Vector3 originPos = Vector3.zero; //세이크 하고 나서 원래 위치를 되돌릴 벡터 변수
    private Quaternion originRot = Quaternion.identity;//세이크 하고 나서 원래 회전으로 되돌릴 쿼터니언 변수

    void Start()
    {
        originPos = shakeCamera.transform.position;
        originRot = shakeCamera.transform.rotation;
    }
    public IEnumerator ShakeCamera(float duration =0.05f
        ,float magnitudePos =0.03f
        ,float magnitudeRot =0.1f)
    {
        float passTime = 0.0f;//시간을 누적할 변수 
        //세이크 시간동안 루프를 순회 하기 위개 
        while (passTime < duration)
        {   //불규칙한 위치를 추출 
            Vector3 shakePos = Random.insideUnitSphere;
               float random =  Random.Range(0,20f);
            shakeCamera.transform.position = shakePos * magnitudePos;
            if(shakeRotate)
            {
                Vector3 shakeRot = new Vector3(0f, 0f, Mathf.PerlinNoise(Time.time * magnitudeRot, 0f));
                shakeCamera.rotation = Quaternion.Euler(shakeRot);
            }
            passTime += Time.deltaTime; // 쉐이크 시간을 누적
            yield return null;
        }
        //쉐이크가 끝난후 카메라를 원래 위치와 회전으로 설정 
        shakeCamera.position = originPos;
        shakeCamera.rotation=originRot;
    }
  
    
}
