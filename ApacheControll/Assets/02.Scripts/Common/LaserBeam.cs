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
        // 라인렌더러 첫번째 점을 시작점으로
        lineRenderer.SetPosition(0, tr.InverseTransformPoint(ray.origin));
        if(Physics.Raycast(ray, out hit, 200f, terrainLayer)) // 200 유닛 범위 안에서 광신이 맞았다면
        {
            lineRenderer.SetPosition(1, tr.InverseTransformPoint(hit.point));
        }
        else
        {
            lineRenderer.SetPosition(1, tr.InverseTransformPoint(ray.GetPoint(200f)));  // 맞지 않은 경우 끝점을 댜략 200유닛으로 지정
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
