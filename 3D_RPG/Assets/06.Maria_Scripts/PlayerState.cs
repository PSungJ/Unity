using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public enum State
    {
        IDLE, ATTACK, SHIELD_ATTACK, HIT, DIE
    }
    public State state = State.IDLE;

    [SerializeField] private PlayerInput input; // PlayerInput 스크립트
    [SerializeField] private CameraCtrl _camera;    // CameraCtrl 스크립트
    void Start()
    {
        input = GetComponent<PlayerInput>();
        _camera = GetComponent<CameraCtrl>();
    }

    void Update()
    {
        FreezeXZ();

        switch (state)
        {
            case State.IDLE:
                input.PlayerIdleAndMove();
                break;
            case State.ATTACK:
                input.AttackTimeState();
                break;
            case State.SHIELD_ATTACK:
                input.AttackTimeState();
                break;
            case State.HIT:

                break;
            case State.DIE:

                break;
        }
        _camera.CameraDistanceCtrl();
    }
    void FreezeXZ()
    {
        transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
    }
}
