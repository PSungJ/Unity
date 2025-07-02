using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCollider : MonoBehaviour
{
    [SerializeField] private BoxCollider boxCol;    // 박스 콜라이더
    [SerializeField] private MeshRenderer mesh;     // 메쉬 렌더러
    [SerializeField] int swordLayer;
    [SerializeField] int shieldLayer;
    float swordDamage, shieldDamage;
    void Start()
    {
        boxCol = GetComponentInChildren<BoxCollider>();
        mesh = boxCol.GetComponent<MeshRenderer>();
        swordLayer = LayerMask.NameToLayer("SWORD");
        shieldLayer = LayerMask.NameToLayer("SHIELD");
        swordDamage = 0f;
        shieldDamage = 0f;
    }
    public void AttackHitEnable()
    {
        boxCol.enabled = true;
        mesh.enabled = true;
        swordDamage = 20f;
        shieldDamage = 10f;
    }
    public void AttackHitDisable()
    {
        boxCol.enabled = false;
        mesh.enabled = false;
        swordDamage = 0f;
        shieldDamage = 0f;
    }
}
