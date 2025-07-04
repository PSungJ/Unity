using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICar : MonoBehaviour
{
    [Header("WheelCollider")] public WheelCollider frontL, frontR, backL, backR;
    [Header("WheelModels")] public Transform frontL_M, frontR_M, backL_M, backR_M;
    [Header("CarSetting")]
    public Vector3 comPos = new Vector3(0f, -0.5f, 0f);
    [SerializeField] private float maxSteerAngle = 35f;     // 최대 조향각
    [SerializeField] private float maxMotorTorque = 500f;  // 가속력
    [SerializeField] private float emsBrakeForce;  // 비상브레이크 토크
    [Header("Car Status")] public float curSpeed = 0f;
    [Header("Path Line")]
    [SerializeField] private List<Transform> NodeList;
    [SerializeField] private int currentNodeIdx = 0;

    [SerializeField] private Rigidbody rb;
    public float maxSpeed = 80f;
    private float preTime = 0f;
    void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.centerOfMass = comPos;
        preTime = Time.time;

        var pathArray = GameObject.Find("PathPoint").transform;
        if (pathArray != null)
        {
            pathArray.GetComponentsInChildren<Transform>(NodeList);
        }
        NodeList.RemoveAt(0);   // 첫번째로 오는 부모오브젝트는 제외
    }
    void FixedUpdate()
    {
        if (Time.time - preTime >= 10f)
        {
            ApplySteer();
            Drive();
            CheckWayPointDistance();
        }
    }
    void ApplySteer()   // 앞바퀴가 경로를 따라 회전
    {
        // 월드 좌표를 차량의 로컬 좌표로 변환하여 상대적인 위치를 계산
        Vector3 relativeVector = transform.InverseTransformPoint(NodeList[currentNodeIdx].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle; // 상대위치의 x좌표를 사용하여 조향각 계산
        // (x좌표상의 경로라인 / 경로라인의 길이) * 최대 조향각
        frontL.steerAngle = newSteer;
        frontR.steerAngle = newSteer;   // 계산한 조향각을 양쪽 앞바퀴에 적용
    }
    void Drive()    // 경로를 따라 이동
    {
        curSpeed = 2f * Mathf.PI * frontL.radius * frontL.rpm * 60f / 1000f;    // 현재속도 계산하여 km/h로 변환
        //curSpeed = ((2f * Mathf.PI * frontL.radius) * frontL.rpm / 60f) * 3.6f;
        /*전체 계산 과정
         1. 바퀴의 둘레계산(m) : 2f * Mathf.PI * (WheelCollider).radius
         2. 분당 이동거리 계산(m/minute) : (2f * Mathf.PI * (WheelCollider).radius) * (WheelCollider).rpm
         3. 시간 단위 변환(m/minute -> km/h) : m/분 -> m/시 : (m/분) * 60
                                              m/시 -> km/시 : (m/시) / 1000*/

        if (curSpeed < maxSpeed)    // 현재 속도가 최고 속도보다 낮을 때만 가속
        {
            backL.motorTorque = maxMotorTorque;
            backR.motorTorque = maxMotorTorque;
        }
        else
        {
            backL.motorTorque = 0f;
            backR.motorTorque = 0f;
        }
    }
    void CheckWayPointDistance()    // 경로 체크 후 인덱스를 0으로 초기화
    {
        if (Vector3.Distance(transform.position, NodeList[currentNodeIdx].position) <= 2.5f)
        {
            if (currentNodeIdx == NodeList.Count - 1)
                currentNodeIdx = 0;
            else
                currentNodeIdx++;
        }
    }
}
