using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ShotGunData", menuName = "ScriptableObjects/ShotGundata", order = 2)]
// ��Ʃ����Ʈ      ���ϸ�                   �޴��̸�                                    ����
public class ShotGunData : ScriptableObject
{
    public AudioClip shotClip; // �Ѿ� �߻� ����
    public AudioClip reloadClip; // �Ѿ� ������ ����
    public float fireRate = 0.1f; // �߻� �ӵ�
    public float reloadTime = 2.0f; // ������ �ð�
}
