using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "ScriptableObjects/GunData")]

public class GunData : ScriptableObject
{
    public AudioClip shotClip;
    public AudioClip reloadClip;
    public float damage = 10f;
}
