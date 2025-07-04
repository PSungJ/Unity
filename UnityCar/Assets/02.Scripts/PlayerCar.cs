using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 1. Wheel Collider : �ٴ� ������ �̵�, 2. Wheel Models : �𵨸��� WheelCollider�� ���� ȸ��
public class PlayerCar : MonoBehaviour
{
    [Header("WheelCollider")] public WheelCollider frontL, frontR, backL, backR;
    [Header("WheelModels")] public Transform frontL_M, frontR_M, backL_M, backR_M;
    [Header("CarSetting")]
    public Vector3 comPos = new Vector3(0f, -0.5f, 0f);     // vichicle�� ������ �߽��� �־����
    [SerializeField] private float maxSteerAngle = 35f;     // �ִ� ���Ⱒ
    [SerializeField] private float maxMotorTorque = 2500f;  // ���ӷ�
    [SerializeField] private float maxSpeed = 200f;         // �ִ�ӵ�
    [SerializeField] private float emsBrakeForce = 8000f;  // ���극��ũ ��ũ

    [Header("Car CurrnetSpeed")]
    [SerializeField] public float curSpeed = 0f;           // ����ӵ�

    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject fpsPlayer;
    [SerializeField] private bool isInCar = false;
    [SerializeField] private Transform playerTr;
    [SerializeField] private Transform tr;
    [SerializeField] private Transform getOutPos;
    private readonly string hori = "Horizontal";
    private readonly string ver = "Vertical";

    private float SteerInput = 0f;       //A,D Ű���� �ޱ� ���� ����
    public float MotorInput = 0f;       //W,S Ű���� �ޱ� ���� ����
    public bool isEmsBraking = false;
    void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.centerOfMass = comPos;
        isInCar = false;
    }
    private void FixedUpdate()
    {
        if (isInCar)
        {
            HandleMotor();
            HandleSteering();
        }
    }
    private void HandleMotor()
    {
        // ��� �극��ũ�� ��� �ִٸ� ��� �����°� �극��ũ�� ����
        if (isEmsBraking)
        {
            // ��� ���� �������� 0����
            frontL.motorTorque = 0f;
            frontR.motorTorque = 0f;
            backL.motorTorque = 0f;
            backR.motorTorque = 0f;
            // ��� ������ ������ �극��ũ�� �Ǵ�.
            frontL.brakeTorque = emsBrakeForce;
            frontR.brakeTorque = emsBrakeForce;
            backL.brakeTorque = emsBrakeForce;
            backR.brakeTorque = emsBrakeForce;
        }
        else
        {
            frontL.motorTorque = maxMotorTorque * MotorInput;
            frontR.motorTorque = maxMotorTorque * MotorInput;
            backL.motorTorque = maxMotorTorque * MotorInput;
            backR.motorTorque = maxMotorTorque * MotorInput;
            // �Ϲ����� �����̶�� 0���� �ʱ�ȭ
            frontL.brakeTorque = 0f;
            frontR.brakeTorque = 0f;
            backL.brakeTorque = 0f;
            backR.brakeTorque = 0f;
        }
    }
    private void HandleSteering()
    {
        float steerAngle = maxSteerAngle * SteerInput;
        frontL.steerAngle = steerAngle;
        frontR.steerAngle = steerAngle;
    }
    private void Update()
    {
        GetInCar();
        CarInput();
        curSpeed = rb.velocity.magnitude * 3.6f;    // m/s�� km/h�� ��ȯ
    }
    private void CarInput()
    {
        SteerInput = Input.GetAxis(hori);   // A,D��
        MotorInput = Input.GetAxis(ver);    // W,S��
        isEmsBraking = Input.GetKey(KeyCode.Space);
    }
    private void LateUpdate()
    {
        UpdateWheelModel(frontL, frontL_M);
        UpdateWheelModel(frontR, frontR_M);
        UpdateWheelModel(backL, backL_M);
        UpdateWheelModel(backR, backR_M);
    }
    private void UpdateWheelModel(WheelCollider col, Transform model)   // �� �ݶ��̴��� ���� �� �� ������
    {
        Vector3 pos;
        Quaternion rot;
        col.GetWorldPose(out pos, out rot); // �� �ݶ��̴��� ��ġ�� ȸ������ ������
        model.position = pos;
        model.rotation = rot;
    }
    void GetInCar()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            float dist = Vector3.Distance(tr.transform.position, playerTr.transform.position);
            if (!isInCar && dist <= 3)
            {
                isInCar = true;
                fpsPlayer.SetActive(false);
                playerTr.position = tr.position;
            }
            else if (isInCar)
            {
                isInCar = false;
                fpsPlayer.SetActive(true);
                playerTr.position = getOutPos.position;
            }
        }
    }
}
