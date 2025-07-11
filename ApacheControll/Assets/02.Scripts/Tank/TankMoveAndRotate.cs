using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class TankMoveAndRotate : MonoBehaviourPun, IPunObservable   // ���ð� ����Ʈ �÷��̾ ���� �����̴� ���� ����
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)  // ������ �̵� ȸ���� ��Ʈ��ũ �� ���̴� ������ �۽��ϰ�, Ÿ���� �����ϴ� ����Ʈ ������ �����ؾ��Ѵ�.
    {
        if (stream.IsWriting)   // ������ ������ �۽�
        {
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
        }
        else /*if (stream.IsReading)*/  // ����Ʈ�� ������ ����
        {
            curPos = (Vector3)stream.ReceiveNext();
            curRot = (Quaternion)stream.ReceiveNext();
        }
    }

    void FixedUpdate()
    {
        if (photonView.IsMine)  // �����Ʈ��ũ �� �����ڰ� �ڱ��ڽ��̸� Ű���� ����
        {
            // �̵�
            Vector3 move = (Vector3.forward * input.v);
            tr.Translate(Vector3.forward * input.v * moveSpeed * Time.deltaTime);
            rb.MovePosition(rb.position + move * moveSpeed * Time.deltaTime);

            // ȸ��
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
