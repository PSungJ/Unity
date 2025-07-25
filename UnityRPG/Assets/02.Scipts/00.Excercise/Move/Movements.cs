using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Combat;

namespace RPG.Movement  
{
    public class Movements : MonoBehaviour, IAction   // Player, Enemy의 움직임 로직만을 담당
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
            navi.speed = moveSpeed; // 기본 이동속도 초기화
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

        public void OnMove(Vector3 destination, float speed)    // 지정한 Vector3목적지를 향해 지정한 speed값으로 이동
        {
            navi.destination = destination; // NavMesh가 지정한 목적지를 가져옴
            navi.isStopped = false; // 이동을 해야하니 isStopped는 false
            navi.speed = speed; // 애니메이션에 walk, run 두가지가 있으니 상황에 맞춰서 변동가능
        }

        public void Cancel()
        {
            fight.Cancel(); // 인터페이스 상속으로 Move취소 구현
        }

        public void Stop()
        {
            navi.isStopped = true;  // 이동이 끝나면 NavMesh 멈추기
        }

        public void AnimaitorUpdate()   // 이동 중일땐 이동 Ani, 공격을 해야하면 공격 Ani를 불러온다
        {
            Vector3 worldVelocity = navi.velocity;  // NavMesh의 월드 속도 추출
            // 월드 속도 > 캐릭터 로컬 속도로 변환 InversTransformDirection
            Vector3 localVelocity = transform.InverseTransformDirection(worldVelocity);

            float fowardSpeed = localVelocity.z; // 전진 속도를 변환된 로컬z축의 속도로 지정
            // 이동 Ani를 Float으로 지정하여 자동으로 걷고 뛸 때 속도를 변환하여 재생
            ani.SetFloat("Move", fowardSpeed, 0.01f, Time.deltaTime);
        }
    }
}
