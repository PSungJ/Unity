using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoint : MonoBehaviour
{
    public Color lineColor = Color.red;
    public List<Transform> NodeList;
    private void OnDrawGizmos()
    {
        Gizmos.color = lineColor;
        NodeList = new List<Transform>();
        Transform[] PathTr = GetComponentsInChildren<Transform>();
        for (int i = 0; i < PathTr.Length; i++)
        {
            if (PathTr[i] != this.transform)
                NodeList.Add(PathTr[i]);
        }
        
        for (int i = 0; i < NodeList.Count; i++)
        {
            Vector3 currnetNode = NodeList[i].position; // ���� ���
            Vector3 previousNode = Vector3.zero;        // ���� ���

            if (i > 0)
                previousNode = NodeList[i - 1].position;
            else if (i == 0 && NodeList.Count > 1)
            {
                previousNode = NodeList[NodeList.Count - 1].position;   // ù ��° ����� �� ������ ���� ����
            }
            Gizmos.DrawLine(previousNode, currnetNode); // ���� ���� ���� ��带 �����ϴ� �� �׸���
            Gizmos.DrawSphere(currnetNode, 0.8f);       // ���� ��� ��ġ�� ��ü �׸���
        }
    }
}
