using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyFOV))]
public class FOVEditor : Editor
{
    private void OnSceneGUI()   // �� �信�� �ڵ��� �׸��� ���� �޼���
    {
        EnemyFOV fov = (EnemyFOV)target;    //EnemyFOV��ũ��Ʈ�� �ν��Ͻ��� �����ɴϴ�.
        Vector3 fromAnglePos = fov.CirclePoint(-fov.viewAngle * 0.5f);
        // ���� �� �������� ��ǥ�� ���(�־��� ������ 1/2�� ����Ͽ� �������� ������ ���)
        Handles.color = Color.white;       //�ڵ��� ������ �����մϴ�.
        // ä���� ȣ�� �׸��� ���� Handles.DrawSolidArc�� ����մϴ�.
        Handles.DrawSolidArc(
            fov.transform.position, //���� �߽� ��ǥ
            Vector3.up,             //y���� �������� ȸ��
            fromAnglePos,           //��ä���� ������ ��ǥ
            fov.viewAngle,          //��߰� ��ä���� ����
            fov.viewRange          //��ä���� ������
        );  //�� �信�� �þ߰��� ��Ÿ���� ȣ�� �׸��ϴ�.

        //�þ߰��� �ؽ�Ʈ�� ǥ���ϱ� ���� Handles.Label�� ���
        Handles.Label(fov.transform.position + (fov.transform.forward * 2f), fov.viewAngle.ToString());
        //�� �信 ���� �߰��Ͽ� �þ߰��� ������ ǥ���մϴ�.
    }
}
