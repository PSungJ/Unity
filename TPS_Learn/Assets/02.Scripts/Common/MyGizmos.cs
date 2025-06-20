using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmos : MonoBehaviour
{
    public float _radius = 0.5f;
    public Color _color = Color.red;

    private void OnDrawGizmos()     // �ݹ� �Լ� : ��ȭ�鿡 �����̳� ���� �׷��ִ� ����Ƽ �����Լ�
    {
        Gizmos.color = _color;
        Gizmos.DrawSphere(transform.position, _radius); // �������� �����ǰ� �ݰ��� �׸�
    }
}
