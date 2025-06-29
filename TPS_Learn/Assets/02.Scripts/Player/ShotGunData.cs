using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��Ʈ����Ʈ : ���ϸ�/�޴��̸�/����
[CreateAssetMenu(fileName = "ShotGunData", menuName = "ScriptableObject/ShotGunData", order = 2)]
public class ShotGunData : ScriptableObject
{
    public AudioClip shotClip;      // �Ѿ� ����
    public AudioClip reloadClip;    // ������ ����
    public float fireRate = 0.1f;   // �߻� �ӵ�
    public float reloadTime = 2.0f; // ������ �ð�
}
