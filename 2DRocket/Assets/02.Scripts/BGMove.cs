using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMove : MonoBehaviour
{
    //변수 지정 : 어디로 이동할지, 속도, 방향
    private float x = 0f, y = 0f;
    public float speed = 0.5f;

    public MeshRenderer meshRenderer;
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        BackGroundmove();
    }

    private void BackGroundmove()
    {
        x += Time.deltaTime * speed;
        meshRenderer.material.mainTextureOffset = new Vector2(x, y);
        //메쉬렌더러의 머테리얼 안에 메인텍스쳐 오프셋 = new 벡터2(x, y);
    }
}
