using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretCtrl : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    private Transform tr;
    private int terrainLayer;
    private float rotSpeed = 3f;
    void Start()
    {
        tr = transform;
        terrainLayer = LayerMask.GetMask("TERRAIN");
    }

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, terrainLayer))
        {
            Vector3 relative = tr.InverseTransformDirection(hit.point);         // ������ ���� ������ ������ǥ�� ���� ��ǥ�� ��ȯ
            float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;  // ��ź��Ʈ �Լ�, ����� = Atan2(local.x, local.z) * PI*2/360(������ �Ϲ� ������ ���� Rad2Deg)
            tr.Rotate(0f, angle * Time.deltaTime * rotSpeed, 0f);
        }
    }
}
