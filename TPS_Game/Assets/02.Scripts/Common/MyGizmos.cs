using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmos : MonoBehaviour
{
    public enum  TYPE{ NORMAL=0,WAYPOINT=1 }
    private readonly string wayPointFie = "Enemy";
    public TYPE type = TYPE.NORMAL;

    public float _radius = 0.5f;
    public Color _color = Color.red;

    private void OnDrawGizmos() //�ݹ��Լ�:��ȭ�鿡 �����̳� ���� �׷��ִ� ����Ƽ �����Լ� 
    {
        if (type == TYPE.NORMAL)
        {
            Gizmos.color = _color; //����
            Gizmos.DrawSphere(transform.position, _radius);
            //�������� �׸���(������, �ݰ�)
        }
        else
        {
            Gizmos.color = _color;
            Gizmos.DrawIcon(transform.position +Vector3.up *1.0f ,wayPointFie,true);
                                  //��ġ  , ����������ȿ� �ִ� �̹�����, ������ ���� ����
            Gizmos.DrawWireSphere(transform.position, _radius);

        }
    }

}
