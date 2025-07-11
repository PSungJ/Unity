using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class TankMoveAndRotate : MonoBehaviourPun, IPunObservable   // 로컬과 리모트 플레이어가 같이 움직이는 현상 제거
{
    private Rigidbody rb;
    private Transform tr;
    private TankInput input;
    Vector3 curPos = Vector3.zero;
    Quaternion curRot = Quaternion.identity;
    private float moveSpeed = 0f;
    private float rotSpeed = 0f;
    public Vector3 newFramingOffset = new Vector3(0, 8f, -15f);
    IEnumerator Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0f, -0.5f, 0f);
        tr = GetComponent<Transform>();
        input = GetComponent<TankInput>();
        moveSpeed = 10f;
        rotSpeed = 10f;
        curPos = tr.position;
        curRot = tr.rotation;
        yield return null;
        if (photonView.IsMine)
        {
            CinemachineVirtualCamera vcam = FindObjectOfType<CinemachineVirtualCamera>();
            CinemachineComponentBase body = vcam.GetCinemachineComponent<CinemachineComponentBase>();
            if (body is CinemachineTransposer)
            {
                CinemachineTransposer transposer = body as CinemachineTransposer;
                transposer.m_FollowOffset = newFramingOffset;
            }
            vcam.Follow = transform;
            vcam.LookAt = transform.GetChild(0).transform;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)  // 로컬의 이동 회전을 네트워크 상에 보이는 나에게 송신하고, 타인이 조작하는 리모트 조작을 수신해야한다.
    {
        if (stream.IsWriting)   // 로컬의 움직임 송신
        {
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
        }
        else /*if (stream.IsReading)*/  // 리모트의 움직임 수신
        {
            curPos = (Vector3)stream.ReceiveNext();
            curRot = (Quaternion)stream.ReceiveNext();
        }
    }

    void FixedUpdate()
    {
        if (photonView.IsMine)  // 포톤네트워크 상에 접속자가 자기자신이면 키보드 조작
        {
            // 이동
            Vector3 move = (Vector3.forward * input.v);
            tr.Translate(Vector3.forward * input.v * moveSpeed * Time.deltaTime);
            rb.MovePosition(rb.position + move * moveSpeed * Time.deltaTime);

            // 회전
            float turn = input.h * rotSpeed * Time.deltaTime;
            rb.rotation = rb.rotation * Quaternion.Euler(0f, turn, 0f);
        }
        else
        {
            tr.position = Vector3.Lerp(tr.position, curPos, Time.deltaTime * 3f);
            tr.rotation = Quaternion.Slerp(tr.rotation, curRot, Time.deltaTime * 3f);
        }
    }
}
