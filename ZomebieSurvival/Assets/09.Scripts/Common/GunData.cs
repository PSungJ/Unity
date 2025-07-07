using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "ScriptableObjects/GunData")]
public class GunData : ScriptableObject
{
    public AudioClip shotClip;
    public AudioClip reloadClip;
    public float damage = 25f;
    public int magCapacity = 25;        // źâ �뷮
    public int startAmmoRemain = 100;   // ó�� �־����� ��ü ź��
    public float timeBetTime = 0.1f;    // �߻� ����
    public float reloadTime = 1.8f;     // ������ �ð�
}
