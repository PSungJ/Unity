using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGFarMove : MonoBehaviour
{
    public float speed;
    public Transform tr;
    public BoxCollider2D box2D;
    private float width;

    void Start()
    {
        tr = GetComponent<Transform>();
        box2D = GetComponent<BoxCollider2D>();
        speed = 5f;
        width = box2D.size.x; //BoxCollider의 x 사이즈 값을 너비로 지정
    }

    void Update()
    {
        //if (GameManager.instance.isGameOver == true)
        //{
        //    return; // 여기서부터 하위 로직으로 진행하지 않음
        //}

        if (tr.position.x <= -width*1.8f)
        {
            RePosition();
        }
        tr.Translate(Vector3.left * speed * Time.deltaTime);   
    }

    private void RePosition()
    {
        Vector2 offset = new Vector2(width * 2.8f, 0f);
        tr.position = (Vector2)tr.position + offset;
    }
}
