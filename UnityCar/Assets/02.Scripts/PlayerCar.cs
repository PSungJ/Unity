using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 1. Wheel Collider : 바닥 마찰로 이동, 2. Wheel Models : 모델링은 WheelCollider와 같이 회전
public class PlayerCar : MonoBehaviour
{
    [Header("WheelCollider")] public WheelCollider frontL, frontR, backL, backR;
    [Header("WheelModels")] public Transform frontL_M, frontR_M, backL_M, backR_M;
    [Header("CarSetting")]
    public Vector3 comPos = new Vector3(0f, -0.5f, 0f);     // vichicle은 무조건 중심이 있어야함
    [SerializeField] private float maxSteerAngle = 35f;     // 최대 조향각
    [SerializeField] private float maxMotorTorque = 2500f;  // 가속력
    [SerializeField] private float maxSpeed = 200f;         // 최대속도
    [SerializeField] private float emsBrakeForce = 8000f;  // 비상브레이크 토크

    [Header("Car CurrnetSpeed")]
    [SerializeField] public float curSpeed = 0f;           // 현재속도

    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject fpsPlayer;
    [SerializeField] private bool isInCar = false;
    [SerializeField] private Transform playerTr;
    [SerializeField] private Transform tr;
    [SerializeField] private Transform getOutPos;
    private readonly string hori = "Horizontal";
    private readonly string ver = "Vertical";

    private float SteerInput = 0f;       //A,D 키값을 받기 위한 변수
    public float MotorInput = 0f;       //W,S 키값을 받기 위한 변수
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
        // 비상 브레이크를 밟고 있다면 모든 구동력과 브레이크를 통제
        if (isEmsBraking)
        {
            // 모든 바퀴 구동력을 0으로
            frontL.motorTorque = 0f;
            frontR.motorTorque = 0f;
            backL.motorTorque = 0f;
            backR.motorTorque = 0f;
            // 모든 바퀴에 강력한 브레이크를 건다.
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
            // 일반적인 주행이라면 0으로 초기화
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
        curSpeed = rb.velocity.magnitude * 3.6f;    // m/s를 km/h로 변환
    }
    private void CarInput()
    {
        SteerInput = Input.GetAxis(hori);   // A,D값
        MotorInput = Input.GetAxis(ver);    // W,S값
        isEmsBraking = Input.GetKey(KeyCode.Space);
    }
    private void LateUpdate()
    {
        UpdateWheelModel(frontL, frontL_M);
        UpdateWheelModel(frontR, frontR_M);
        UpdateWheelModel(backL, backL_M);
        UpdateWheelModel(backR, backR_M);
    }
    private void UpdateWheelModel(WheelCollider col, Transform model)   // 휠 콜라이더에 따른 휠 모델 움직임
    {
        Vector3 pos;
        Quaternion rot;
        col.GetWorldPose(out pos, out rot); // 휠 콜라이더의 위치와 회전값을 가져옴
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
