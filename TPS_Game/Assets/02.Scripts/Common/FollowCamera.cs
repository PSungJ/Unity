using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float moveDamping = 15.0f;
    [SerializeField] float rotateDamping = 10f;
    [SerializeField] float distance = 10f;
    [SerializeField] float height = 7.0f;
    [SerializeField] float targetOffset = 2.0f;
    private Transform tr;
    public float maxHeight = 20f;
    public float castOffset = 1f;
    public float originHeight;
    private readonly string playerTag = "Player";
    private float overDamping = 10f;
    CinemachineVirtualCamera virtualCamera;
    void Start()
    {
        virtualCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
        // 씬에 있는 오브젝트 중 CinemachineVirtualCamera 컴퍼넌트를 가진 오브젝트를 찾아서 할당
        tr = transform;
        originHeight = height;
    }
    void Update()
    {
        // 플레이어가 장애물에 가려졌는지 확인하는 Raycast 높낮이
        Vector3 castTarget = target.position + (Vector3.up * castOffset);
        Vector3 castDir = (castTarget - tr.position).normalized;    // 카메라에서 플레이어를 바라보는 방향벡터
        RaycastHit hit; // 충돌 정보를 가지고 있는 구조체
        if(Physics.Raycast(tr.position, castDir, out hit, Mathf.Infinity))
        {
            if (!hit.collider.CompareTag(playerTag)) // 플레이어가 카메라 Ray에 맞지 않았다면
            {
                height = Mathf.Lerp(height, maxHeight, Time.deltaTime * overDamping);
            }
            else
            {
                height = Mathf.Lerp(height, originHeight, Time.deltaTime * overDamping);
            }
        }
    }
    void LateUpdate()//Update나 FixedUpdate 먼저 이동이 되고 따라 가야 할때
    {
        var Campos = target.position -(Vector3.forward * distance)+(Vector3.up * height);
        tr.position = Vector3.Slerp(tr.position, Campos, Time.deltaTime * moveDamping);
        tr.rotation = Quaternion.Slerp(tr.rotation,target.rotation, Time.deltaTime * rotateDamping);
        tr.LookAt(target.position+ (Vector3.up *targetOffset));
    }
    
}
