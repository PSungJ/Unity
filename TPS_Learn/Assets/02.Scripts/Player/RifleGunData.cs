using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��Ʈ����Ʈ : ���ϸ�/�޴��̸�/����
[CreateAssetMenu(fileName = "RifleGunData", menuName = "ScriptableObject/RifleGunData", order = 1)]
public class RifleGunData : ScriptableObject
{
    public AudioClip shotClip;      // �Ѿ� ����
    public AudioClip reloadClip;    // ������ ����
    public float fireRate = 0.1f;   // �߻� �ӵ�
    public float reloadTime = 2.0f; // ������ �ð�
}
