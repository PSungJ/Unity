using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] private ParticleSystem blood;
    private readonly string E_bulletTag = "E_BULLET";
    private float initHp = 100f;
    private float curHp = 0f;
    void Start()
    {
        curHp = initHp;
        blood.Stop();
    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(E_bulletTag))
        {
            col.gameObject.SetActive(false);
            curHp -= 5f;
            blood.Play();

            if (curHp <= 0f)
            {
                PlayerDie();
            }
        }
    }
    void PlayerDie()
    {

    }
}

