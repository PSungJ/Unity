using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WomanShooter : MonoBehaviour
{
    public Gun gun;
    public Transform gunPivot;      // 총의 회전 중심 피봇
    public Transform leftHandleMount;   // 총의 왼쪽 손잡이, 왼손이 위치할 지점
    public Transform rightHandleMount;  // 총의 오른쪽 손잡이, 오른손이 위치할 지점

    public WomanInput input;
    public Animator ani;
    void Start()
    {
        input = GetComponent<WomanInput>();
        ani = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        gun.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        gun.gameObject.SetActive(false);
    }

    void Update()
    {
        // 입력을 감지하고 총을 발사하거나 재장전
        if (input.fire)
        {
            gun.Fire();
        }
        else if (input.reload)
        {
            if (gun.Reload()) // 총이 재장전 가능하면
            {
                gun.Reload();
                ani.SetTrigger("Reload");
            } 
        }
        UpdateUI();
    }
    void UpdateUI()
    {
        // 총알 UI 업데이트
    }
    private void OnAnimatorIK(int layerIndex)
    {
        // 애니메이터 IK를 사용하여 손 위치 조정
        // 애니메이터의 실시간 IK 업데이트
        gunPivot.position = ani.GetIKHintPosition(AvatarIKHint.RightElbow); // 오른팔 팔꿈치 위치를 gunPivot위치로 설정
        ani.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);    // 오른손 IK 위치 가중치 설정
        ani.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);    // 오른손 IK 회전 가중치 설정
        // IK를 사용하여 오른손 위치와 회전을 설정
        ani.SetIKPosition(AvatarIKGoal.RightHand, rightHandleMount.position);
        ani.SetIKRotation(AvatarIKGoal.RightHand, rightHandleMount.rotation);

        ani.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);     // 왼손 IK 위치 가중치 설정
        ani.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);     // 왼손 IK 회전 가중치 설정
        // IK를 사용하여 왼손 위치와 회전을 설정
        ani.SetIKPosition(AvatarIKGoal.LeftHand, leftHandleMount.position);
        ani.SetIKRotation(AvatarIKGoal.LeftHand, leftHandleMount.rotation);
    }
}
