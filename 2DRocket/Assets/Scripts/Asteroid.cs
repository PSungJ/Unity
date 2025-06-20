using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float Speed;
    private Transform tr;
    private readonly string coinTag = "Coin";

    void Start()
    {
        Speed = Random.Range(20f, 35f);
        tr = GetComponent<Transform>();   
    }

     void Update()
    {   
        //방향 X 속도 = Velocity
        tr.Translate(Vector3.left * Speed * Time.deltaTime);

        if (tr.position.x <= -10f)
            Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == coinTag)
        {
            Destroy(col.gameObject);
            Destroy(this.gameObject, 0.1f);
        }
    }
}
