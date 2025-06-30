using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "RifleGunData", menuName = "ScriptableObjects/RifleGundata", order = 1)]
 // ��Ʃ����Ʈ      ���ϸ�                   �޴��̸�                                    ����
public class RifleGunData : ScriptableObject
{
    public AudioClip shotClip; // �Ѿ� �߻� ����
    public AudioClip reloadClip; // �Ѿ� ������ ����
    public float fireRate = 0.1f; // �߻� �ӵ�
    public float reloadTime = 2.0f; // ������ �ð�
}
