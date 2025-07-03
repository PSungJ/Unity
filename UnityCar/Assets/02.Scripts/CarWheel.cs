using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarWheel : MonoBehaviour
{
    public WheelCollider targetWheel;
    public Vector3 WheelPos = Vector3.zero;
    public Quaternion WheelRot = Quaternion.identity;

    void Update()
    {
        targetWheel.GetWorldPose(out WheelPos, out WheelRot);
        transform.position = WheelPos;
        transform.rotation = WheelRot;
    }
}
