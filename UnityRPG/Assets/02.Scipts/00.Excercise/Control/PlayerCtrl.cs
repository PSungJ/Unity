using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Movement;

namespace RPG.Control
{
    public class PlayerCtrl : MonoBehaviour
    {
        // ���� ī�޶󿡼� Ray�� �߻��� Ctrl�Ѵ�
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

        private Ray GetPointRay()   // ī�޶󿡼� �� Ray�� �ҷ��� �޼���
        {
            return mainCamera.ScreenPointToRay(Input.mousePosition);
        }

        private bool Combat()   // ���� ����
        {
            // Ray�� Hit�� ��� ���� hits �迭�� ������
            RaycastHit[] hits = Physics.RaycastAll(GetPointRay());
            foreach (RaycastHit hit in hits)
            {
                // Hit�� �͵� �� CombatTarget Ŭ������ ���� object�� target���� ����
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;   // target�� ���ٸ� ��� ����

                if (Input.GetMouseButton(0))
                {
                    // Fight Ŭ�������� Attack�޼��� ȣ���Ͽ� ����
                    GetComponent<Fight>().Attack(target);
                }
                return true;
            }
            return false;
        }

        private bool Movement() // �̵� ����
        {
            RaycastHit hit;
            // moveLayer�� enemyLayer�� hit �ϸ� True�� ��ȯ
            bool isHit = Physics.Raycast(GetPointRay(), out hit, Mathf.Infinity, moveLayer | enemyLayer);
            if (isHit)
            {
                if (Input.GetMouseButton(0))
                {
                    // Movements Ŭ������ �̵����� ȣ��
                    GetComponent<Movements>().StartMove(hit.point);
                }
                return true;
            }
            return false;
        }
    }
}