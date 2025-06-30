using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using PlasticGui.WorkspaceWindow.Locks;

public class ObstacleAvoidCam : MonoBehaviour
{
    private readonly string playerTag = "Player";
    public CinemachineVirtualCamera virtualCamera;
    public CinemachineTransposer transposer;
    private Transform tr;
    private Transform playerTr;
    public float curOffsetY = 5f;
    public float maxOffsetY = 15f;

    void Start()
    {
        tr = transform;
        playerTr = GameObject.FindGameObjectWithTag(playerTag).GetComponent<Transform>();
        transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }
    void Update()
    {
        
    }
}
