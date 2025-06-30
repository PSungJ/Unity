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
        // ���� �ִ� ������Ʈ �� CinemachineVirtualCamera ���۳�Ʈ�� ���� ������Ʈ�� ã�Ƽ� �Ҵ�
        tr = transform;
        originHeight = height;
    }
    void Update()
    {
        // �÷��̾ ��ֹ��� ���������� Ȯ���ϴ� Raycast ������
        Vector3 castTarget = target.position + (Vector3.up * castOffset);
        Vector3 castDir = (castTarget - tr.position).normalized;    // ī�޶󿡼� �÷��̾ �ٶ󺸴� ���⺤��
        RaycastHit hit; // �浹 ������ ������ �ִ� ����ü
        if(Physics.Raycast(tr.position, castDir, out hit, Mathf.Infinity))
        {
            if (!hit.collider.CompareTag(playerTag)) // �÷��̾ ī�޶� Ray�� ���� �ʾҴٸ�
            {
                height = Mathf.Lerp(height, maxHeight, Time.deltaTime * overDamping);
            }
            else
            {
                height = Mathf.Lerp(height, originHeight, Time.deltaTime * overDamping);
            }
        }
    }
    void LateUpdate()//Update�� FixedUpdate ���� �̵��� �ǰ� ���� ���� �Ҷ�
    {
        var Campos = target.position -(Vector3.forward * distance)+(Vector3.up * height);
        tr.position = Vector3.Slerp(tr.position, Campos, Time.deltaTime * moveDamping);
        tr.rotation = Quaternion.Slerp(tr.rotation,target.rotation, Time.deltaTime * rotateDamping);
        tr.LookAt(target.position+ (Vector3.up *targetOffset));
    }
    
}
