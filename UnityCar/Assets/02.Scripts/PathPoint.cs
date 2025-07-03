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
            Vector3 currnetNode = NodeList[i].position; // 현재 노드
            Vector3 previousNode = Vector3.zero;        // 이전 노드

            if (i > 0)
                previousNode = NodeList[i - 1].position;
            else if (i == 0 && NodeList.Count > 1)
            {
                previousNode = NodeList[NodeList.Count - 1].position;   // 첫 번째 노드일 때 마지막 노드와 연결
            }
            Gizmos.DrawLine(previousNode, currnetNode); // 이전 노드와 현재 노드를 연결하는 선 그리기
            Gizmos.DrawSphere(currnetNode, 0.8f);       // 현재 노드 위치에 구체 그리기
        }
    }
}
