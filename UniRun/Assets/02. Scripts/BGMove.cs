using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMove : MonoBehaviour
{
    private Transform tr;
    [SerializeField] private BoxCollider2D box2D;
    private float width;
    private void Awake()
    {
        tr = GetComponent<Transform>();
        box2D = GetComponent<BoxCollider2D>();
        width = box2D.size.x;
    }
   
    private void Update()
    {
        if (tr.position.x <= -width)
        {
            Vector2 offset = new Vector2(width * 3f, 0);
            tr.position = (Vector2)tr.position + offset;
        }

    }
}
