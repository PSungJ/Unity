using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class CannonCtrl : MonoBehaviourPun, IPunObservable
{
    private TankInput input;
    private Transform tr;
    public float rotSpeed = 150f;
    [Header("포신 각도제한")]
    public float upperAngle = -30f; // 최대 각도 제한
    public float downAngle = 0;
    public float curRotate = 0f;    // 현재 회전 각도
    Quaternion curRot = Quaternion.identity;

    void Start()
    {
        input = GetComponentInParent<TankInput>();
        tr = GetComponent<Transform>();
        curRot = tr.localRotation;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(tr.localRotation);
        }
        else
        {
            curRot = (Quaternion)stream.ReceiveNext();
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            float wheel = -input.m_scrollWheel;
            float angle = Time.deltaTime * rotSpeed * wheel;
            if (wheel <= -0.01f)    // 포신을 올릴 때
            {
                curRotate += angle;
                if (curRotate > upperAngle)
                    tr.Rotate(angle, 0f, 0f);
                else
                    curRotate = upperAngle;
            }
            else
            {
                curRotate += angle;
                if (curRotate < downAngle)
                    tr.Rotate(angle, 0f, 0f);
                else
                    curRotate = downAngle;
            }
        }
        else
        {
            tr.localRotation = Quaternion.Slerp(tr.localRotation, curRot, Time.deltaTime * 3f);
        }
    }
}
