using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WomanShooter : MonoBehaviour
{
    public Gun gun;
    public Transform gunPivot;      // ���� ȸ�� �߽� �Ǻ�
    public Transform leftHandleMount;   // ���� ���� ������, �޼��� ��ġ�� ����
    public Transform rightHandleMount;  // ���� ������ ������, �������� ��ġ�� ����

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
        // �Է��� �����ϰ� ���� �߻��ϰų� ������
        if (input.fire)
        {
            gun.Fire();
        }
        else if (input.reload)
        {
            if (gun.Reload()) // ���� ������ �����ϸ�
            {
                gun.Reload();
                ani.SetTrigger("Reload");
            } 
        }
        UpdateUI();
    }
    void UpdateUI()
    {
        // �Ѿ� UI ������Ʈ
    }
    private void OnAnimatorIK(int layerIndex)
    {
        // �ִϸ����� IK�� ����Ͽ� �� ��ġ ����
        // �ִϸ������� �ǽð� IK ������Ʈ
        gunPivot.position = ani.GetIKHintPosition(AvatarIKHint.RightElbow); // ������ �Ȳ�ġ ��ġ�� gunPivot��ġ�� ����
        ani.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);    // ������ IK ��ġ ����ġ ����
        ani.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);    // ������ IK ȸ�� ����ġ ����
        // IK�� ����Ͽ� ������ ��ġ�� ȸ���� ����
        ani.SetIKPosition(AvatarIKGoal.RightHand, rightHandleMount.position);
        ani.SetIKRotation(AvatarIKGoal.RightHand, rightHandleMount.rotation);

        ani.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);     // �޼� IK ��ġ ����ġ ����
        ani.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);     // �޼� IK ȸ�� ����ġ ����
        // IK�� ����Ͽ� �޼� ��ġ�� ȸ���� ����
        ani.SetIKPosition(AvatarIKGoal.LeftHand, leftHandleMount.position);
        ani.SetIKRotation(AvatarIKGoal.LeftHand, leftHandleMount.rotation);
    }
}
