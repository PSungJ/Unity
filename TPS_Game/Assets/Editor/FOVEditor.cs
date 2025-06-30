using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyFOV))]
public class FOVEditor : Editor
{
    private void OnSceneGUI()   // 씬 뷰에서 핸들을 그리기 위한 메서드
    {
        EnemyFOV fov = (EnemyFOV)target;    //EnemyFOV스크립트의 인스턴스를 가져옵니다.
        Vector3 fromAnglePos = fov.CirclePoint(-fov.viewAngle * 0.5f);
        // 원주 위 시작점의 좌표를 계산(주어진 각도의 1/2를 사용하여 시작점과 끝점을 계산)
        Handles.color = Color.white;       //핸들의 색상을 설정합니다.
        // 채워진 호를 그리기 위해 Handles.DrawSolidArc를 사용합니다.
        Handles.DrawSolidArc(
            fov.transform.position, //원의 중심 좌표
            Vector3.up,             //y축을 기준으로 회전
            fromAnglePos,           //부채꼴의 시작점 좌표
            fov.viewAngle,          //사야각 부채꼴의 각도
            fov.viewRange          //부채꼴의 반지름
        );  //씬 뷰에서 시야각을 나타내는 호를 그립니다.

        //시야각을 텍스트로 표시하기 위해 Handles.Label을 사용
        Handles.Label(fov.transform.position + (fov.transform.forward * 2f), fov.viewAngle.ToString());
        //씬 뷰에 라벨을 추가하여 시야각과 범위를 표시합니다.
    }
}
