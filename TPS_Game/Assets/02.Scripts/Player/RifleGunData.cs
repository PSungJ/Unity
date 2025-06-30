using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "RifleGunData", menuName = "ScriptableObjects/RifleGundata", order = 1)]
 // 어튜리뷰트      파일명                   메뉴이름                                    순서
public class RifleGunData : ScriptableObject
{
    public AudioClip shotClip; // 총알 발사 사운드
    public AudioClip reloadClip; // 총알 재장전 사운드
    public float fireRate = 0.1f; // 발사 속도
    public float reloadTime = 2.0f; // 재장전 시간
}
