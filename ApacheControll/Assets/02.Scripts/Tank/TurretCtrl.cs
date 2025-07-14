using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TurretCtrl : MonoBehaviourPun, IPunObservable
{
    Ray ray;
    RaycastHit hit;
    private Transform tr;
    private int terrainLayer;
    private float rotSpeed = 3f;
    Quaternion curRot = Quaternion.identity;

    void Start()
    {
        tr = transform;
        terrainLayer = LayerMask.GetMask("TERRAIN");
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
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, terrainLayer))
            {
                Vector3 relative = tr.InverseTransformPoint(hit.point);         // 광선이 맞은 지점의 월드좌표를 로컬 좌표로 변환
                float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;  // 역탄젠트 함수, 결과값 = Atan2(local.x, local.z) * PI*2/360(라디안을 일반 각도로 변경 Rad2Deg)
                tr.Rotate(0f, angle * Time.deltaTime * rotSpeed, 0f);
            }
        }
        else
        {
            tr.localRotation = Quaternion.Slerp(tr.localRotation, curRot, Time.deltaTime * 3f);
        }
    }
}
