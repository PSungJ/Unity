using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICar : MonoBehaviour
{
    [Header("WheelCollider")] public WheelCollider frontL, frontR, backL, backR;
    [Header("WheelModels")] public Transform frontL_M, frontR_M, backL_M, backR_M;
    [Header("CarSetting")]
    public Vector3 comPos = new Vector3(0f, -0.5f, 0f);
    [SerializeField] private float maxSteerAngle = 35f;     // �ִ� ���Ⱒ
    [SerializeField] private float maxMotorTorque = 500f;  // ���ӷ�
    [SerializeField] private float emsBrakeForce;  // ���극��ũ ��ũ
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
        NodeList.RemoveAt(0);   // ù��°�� ���� �θ������Ʈ�� ����
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
    void ApplySteer()   // �չ����� ��θ� ���� ȸ��
    {
        // ���� ��ǥ�� ������ ���� ��ǥ�� ��ȯ�Ͽ� ������� ��ġ�� ���
        Vector3 relativeVector = transform.InverseTransformPoint(NodeList[currentNodeIdx].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle; // �����ġ�� x��ǥ�� ����Ͽ� ���Ⱒ ���
        // (x��ǥ���� ��ζ��� / ��ζ����� ����) * �ִ� ���Ⱒ
        frontL.steerAngle = newSteer;
        frontR.steerAngle = newSteer;   // ����� ���Ⱒ�� ���� �չ����� ����
    }
    void Drive()    // ��θ� ���� �̵�
    {
        curSpeed = 2f * Mathf.PI * frontL.radius * frontL.rpm * 60f / 1000f;    // ����ӵ� ����Ͽ� km/h�� ��ȯ
        //curSpeed = ((2f * Mathf.PI * frontL.radius) * frontL.rpm / 60f) * 3.6f;
        /*��ü ��� ����
         1. ������ �ѷ����(m) : 2f * Mathf.PI * (WheelCollider).radius
         2. �д� �̵��Ÿ� ���(m/minute) : (2f * Mathf.PI * (WheelCollider).radius) * (WheelCollider).rpm
         3. �ð� ���� ��ȯ(m/minute -> km/h) : m/�� -> m/�� : (m/��) * 60
                                              m/�� -> km/�� : (m/��) / 1000*/

        if (curSpeed < maxSpeed)    // ���� �ӵ��� �ְ� �ӵ����� ���� ���� ����
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
    void CheckWayPointDistance()    // ��� üũ �� �ε����� 0���� �ʱ�ȭ
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
