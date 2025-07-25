using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Combat;

namespace RPG.Movement  
{
    public class Movements : MonoBehaviour, IAction   // Player, Enemy�� ������ �������� ���
    {
        private float moveSpeed = 1.5f;
        private NavMeshAgent navi;
        private Animator ani;
        private Fight fight;
        private Actions _action;

        void Start()
        {
            navi = GetComponent<NavMeshAgent>();
            ani = GetComponent<Animator>();
            navi.speed = moveSpeed; // �⺻ �̵��ӵ� �ʱ�ȭ
            fight = GetComponent<Fight>();
            _action = GetComponent<Actions>();
        }

        void Update()
        {
            AnimaitorUpdate();
        }

        public void StartMove(Vector3 destination)
        {
            _action.ActionStart(this);
            OnMove(destination, this.moveSpeed);
        }

        public void OnMove(Vector3 destination, float speed)    // ������ Vector3�������� ���� ������ speed������ �̵�
        {
            navi.destination = destination; // NavMesh�� ������ �������� ������
            navi.isStopped = false; // �̵��� �ؾ��ϴ� isStopped�� false
            navi.speed = speed; // �ִϸ��̼ǿ� walk, run �ΰ����� ������ ��Ȳ�� ���缭 ��������
        }

        public void Cancel()
        {
            fight.Cancel(); // �������̽� ������� Move��� ����
        }

        public void Stop()
        {
            navi.isStopped = true;  // �̵��� ������ NavMesh ���߱�
        }

        public void AnimaitorUpdate()   // �̵� ���϶� �̵� Ani, ������ �ؾ��ϸ� ���� Ani�� �ҷ��´�
        {
            Vector3 worldVelocity = navi.velocity;  // NavMesh�� ���� �ӵ� ����
            // ���� �ӵ� > ĳ���� ���� �ӵ��� ��ȯ InversTransformDirection
            Vector3 localVelocity = transform.InverseTransformDirection(worldVelocity);

            float fowardSpeed = localVelocity.z; // ���� �ӵ��� ��ȯ�� ����z���� �ӵ��� ����
            // �̵� Ani�� Float���� �����Ͽ� �ڵ����� �Ȱ� �� �� �ӵ��� ��ȯ�Ͽ� ���
            ani.SetFloat("Move", fowardSpeed, 0.01f, Time.deltaTime);
        }
    }
}
