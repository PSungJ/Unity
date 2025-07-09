using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Transform tr;
    private int terrainLayer;

    void Start()
    {
        tr = GetComponent<Transform>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
        lineRenderer.enabled = false;
        terrainLayer = LayerMask.GetMask("TERRAIN");
    }
    public void FireRay()
    {
        Ray ray = new Ray(tr.position + (Vector3.up * 0.02f), tr.forward);
        RaycastHit hit;
        // ���η����� ù��° ���� ����������
        lineRenderer.SetPosition(0, tr.InverseTransformPoint(ray.origin));
        if(Physics.Raycast(ray, out hit, 200f, terrainLayer)) // 200 ���� ���� �ȿ��� ������ �¾Ҵٸ�
        {
            lineRenderer.SetPosition(1, tr.InverseTransformPoint(hit.point));
        }
        else
        {
            lineRenderer.SetPosition(1, tr.InverseTransformPoint(ray.GetPoint(200f)));  // ���� ���� ��� ������ ���� 200�������� ����
        }
        StartCoroutine(ShowLaserBeam());
    }
    IEnumerator ShowLaserBeam()
    {
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
        lineRenderer.enabled = false;
    }
}
