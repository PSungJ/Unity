using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float health = 100f;
        bool isDead = false;
        
        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0);
            print(health);
            if (health <= 0)
            {
                bool isDead = Dead();
            }
        }
        public bool Dead()
        {
            print("»ç¸Á");
            return true;
        }
    }
}
