using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float moveDamping = 15.0f;
    [SerializeField] float rotateDamping = 10f;
    [SerializeField] float distance = 5f;
    [SerializeField] float height = 4.0f;
    [SerializeField] float targetOffset = 2.0f;
    private Transform tr;
    
    void Start()
    {
        tr = transform;
    }
    void LateUpdate()//Update나 FixedUpdate 먼저 이동이 되고 따라 가야 할때
    {
        var Campos = target.position -(Vector3.forward * distance)+(Vector3.up * height);
        tr.position = Vector3.Slerp(tr.position, Campos, Time.deltaTime * moveDamping);
        tr.rotation = Quaternion.Slerp(tr.rotation,target.rotation, Time.deltaTime * rotateDamping);
        tr.LookAt(target.position+ (Vector3.up *targetOffset));
    }
    
}
