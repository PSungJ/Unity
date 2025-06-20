using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmos : MonoBehaviour
{
    public float _radius = 0.5f;
    public Color _color = Color.red;

    private void OnDrawGizmos()     // 콜백 함수 : 씬화면에 색상이나 선을 그려주는 유니티 지원함수
    {
        Gizmos.color = _color;
        Gizmos.DrawSphere(transform.position, _radius); // 원형으로 포지션과 반경을 그림
    }
}
