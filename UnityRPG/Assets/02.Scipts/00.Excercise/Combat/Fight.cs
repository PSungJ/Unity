using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement; // Movements.cs �� �����Ͽ� ������ ����Ǿ���Ѵ�.
using RPG.Core;

namespace RPG.Combat
{
    public class Fight : MonoBehaviour, IAction
    {
        [SerializeField] float attackRange = 2f; // ���� ��Ÿ� ����
        float timeBetweenAttack = 1f;   // ���� ����
        float timeSinceLastAttack;  // �ð��� ��� ����
        float rotationSpeed = 10f;  // ȸ�� �ӵ� ����
        private Transform target;   // ���� ���
        private Movements movement;
        private Actions _action;

        void Start()
        {
            movement = GetComponent<Movements>();
            _action = GetComponent<Actions>();
        }

        void Update()
        {
            if (target == null) // ���� ����� ���ٸ� ����x
            { 
                Debug.Log("Target is null in Update."); 
                return; 
            }
            if (target.GetComponent<Health>().Dead())
            {
                target = null;
                return;
            }

            if (!InRange())
            {
                movement.OnMove(target.position, 5f);
            }
            else
            {
                movement.Cancel();
                movement.Stop();
                AttackBehavior();
            }
        }

        private void AttackBehavior()
        {
            if (timeSinceLastAttack > timeBetweenAttack)
            {
                GetComponent<Animator>().SetTrigger("Attack");
                timeSinceLastAttack = 0;
                CombatTarget target = GetComponent<CombatTarget>();
                Vector3 normal = (target.transform.position - transform.position).normalized;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(normal), Time.deltaTime * rotationSpeed);
            }
        }

        public bool InRange()   // ���� ��Ÿ� ���Կ��θ� �Ǵ��ϱ� ���� bool�� ��ȯ
        {
            // ���ΰ� ���ݴ�� ������ �Ÿ��� ���ݻ�Ÿ��� ���Ͽ� ��ȯ
            return Vector3.Distance(transform.position, target.position) < attackRange;
        }

        public void Attack(CombatTarget combatTarget)    // CombatTarget Ŭ������ ���� ����� ����
        {
            _action.ActionStart(this);
            target = combatTarget.transform;
        }

        public void Cancel()    // ���� ����� ���� �� ���
        {
            target = null;
        }
    }
}
