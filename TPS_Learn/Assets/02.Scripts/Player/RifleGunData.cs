using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//어트리뷰트 : 파일명/메뉴이름/순서
[CreateAssetMenu(fileName = "RifleGunData", menuName = "ScriptableObject/RifleGunData", order = 1)]
public class RifleGunData : ScriptableObject
{
    public AudioClip shotClip;      // 총알 사운드
    public AudioClip reloadClip;    // 재장전 사운드
    public float fireRate = 0.1f;   // 발사 속도
    public float reloadTime = 2.0f; // 재장전 시간
}
