using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement; // Movements.cs 를 참조하여 공격이 연계되어야한다.
using RPG.Core;

namespace RPG.Combat
{
    public class Fight : MonoBehaviour, IAction
    {
        [SerializeField] float attackRange = 2f; // 공격 사거리 지정
        float timeBetweenAttack = 1f;   // 공격 간격
        float timeSinceLastAttack;  // 시간을 재는 변수
        float rotationSpeed = 10f;  // 회전 속도 제어
        private Transform target;   // 공격 대상
        private Movements movement;
        private Actions _action;

        void Start()
        {
            movement = GetComponent<Movements>();
            _action = GetComponent<Actions>();
        }

        void Update()
        {
            if (target == null) // 공격 대상이 없다면 실행x
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

        public bool InRange()   // 공격 사거리 진입여부를 판단하기 위해 bool로 반환
        {
            // 본인과 공격대상 사이의 거리와 공격사거리를 비교하여 반환
            return Vector3.Distance(transform.position, target.position) < attackRange;
        }

        public void Attack(CombatTarget combatTarget)    // CombatTarget 클래스를 가진 대상을 공격
        {
            _action.ActionStart(this);
            target = combatTarget.transform;
        }

        public void Cancel()    // 공격 대상이 없을 때 취소
        {
            target = null;
        }
    }
}
