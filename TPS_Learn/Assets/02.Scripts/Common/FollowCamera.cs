using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] private float moveDamping = 15f;
    [SerializeField] private float rotateDamping = 10f;
    [SerializeField] private float distance = 5f;
    [SerializeField] private float height = 4f;
    [SerializeField] private float targetOffset = 2f;
    private Transform tr;
    void Start()
    {
        tr = transform;
    }

    void LateUpdate()   // Update나 FixedUpdate가 먼저 호출된 이후 호출된다.
    {
        var camPos = target.position - (Vector3.forward * distance) + (Vector3.up * height);
        tr.position = Vector3.Slerp(tr.position, camPos, Time.deltaTime * moveDamping);
        tr.rotation = Quaternion.Slerp(tr.rotation, target.rotation, Time.deltaTime * rotateDamping);
        tr.LookAt(target.position + (Vector3.up * targetOffset));
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;
    //    // 추적 및 시야를 맞출 위치를 표시
    //    Gizmos.DrawWireSphere(target.position + (Vector3.up * targetOffset), 0.1f);
    //    // 씬화면에서 선을 그린다.
    //    Gizmos.DrawLine(target.position + (Vector3.up * targetOffset), tr.position);
    //}
}
