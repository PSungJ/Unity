using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Movement;

namespace RPG.Control
{
    public class PlayerCtrl : MonoBehaviour
    {
        // 메인 카메라에서 Ray를 발사해 Ctrl한다
        private Camera mainCamera;
        [SerializeField] private LayerMask moveLayer;
        [SerializeField] private LayerMask enemyLayer;

        void Awake()
        {
            mainCamera = Camera.main;
        }

        void Update()
        {
            if (Combat()) return;
            if (Movement()) return;
        }

        private Ray GetPointRay()   // 카메라에서 쏠 Ray를 불러올 메서드
        {
            return mainCamera.ScreenPointToRay(Input.mousePosition);
        }

        private bool Combat()   // 공격 로직
        {
            // Ray로 Hit된 모든 것을 hits 배열로 가져옴
            RaycastHit[] hits = Physics.RaycastAll(GetPointRay());
            foreach (RaycastHit hit in hits)
            {
                // Hit된 것들 중 CombatTarget 클래스를 가진 object를 target으로 지정
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;   // target이 없다면 계속 진행

                if (Input.GetMouseButton(0))
                {
                    // Fight 클래스에서 Attack메서드 호출하여 공격
                    GetComponent<Fight>().Attack(target);
                }
                return true;
            }
            return false;
        }

        private bool Movement() // 이동 로직
        {
            RaycastHit hit;
            // moveLayer와 enemyLayer를 hit 하면 True로 반환
            bool isHit = Physics.Raycast(GetPointRay(), out hit, Mathf.Infinity, moveLayer | enemyLayer);
            if (isHit)
            {
                if (Input.GetMouseButton(0))
                {
                    // Movements 클래스의 이동로직 호출
                    GetComponent<Movements>().StartMove(hit.point);
                }
                return true;
            }
            return false;
        }
    }
}