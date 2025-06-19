using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// 겨누기 시작한 시간 , 지속시간 ,크기 변화 범위 설정
//색상 변경 , 겨누었는지 아닌지 파악
public class CrossHair : MonoBehaviour
{
    Transform tr;
    public Image crosshairImg; // UI 이미지 컴포넌트
    private float startTime = 0f;
    private float duration = 0.3f; // 0.3초 동안 크기 변화
    public float minSize = 0.7f; // 최소 크기
    public float maxSize = 1.2f; // 최대 크기
    private Color originColor = Color.white; // 원래 색상
    private Color gazeColor = Color.red; // 겨누기 색상
    public bool isGaze = false; // 겨누기 상태 여부
    void Start()
    {
        crosshairImg = GetComponent<Image>(); // UI 이미지 컴포넌트 가져오기
        crosshairImg.color = originColor; // 원래 색상으로 초기화
        tr = GetComponent<Transform>();
        startTime = Time.time; // 시작 시간 초기화
        tr.localScale = Vector3.one * minSize; // 초기 크기 설정
                // x,y, z 축 모두 동일한 크기로 설정
    }
    void Update()
    {
        if (isGaze)
        {
            float t = (Time.time - startTime) / duration; // 시간 비율 계산
            tr.localScale = Vector3.one * Mathf.Lerp(minSize, maxSize, t); // 크기 변화
                                                                           //선형보간함수
            crosshairImg.color = gazeColor;
        }
        else
        {
            tr.localScale = Vector3.one * minSize; // 최소 크기로 설정
            crosshairImg.color = originColor; // 원래 색상으로 설정
            startTime = Time.time; // 시작 시간 초기화
        }
    }
}
