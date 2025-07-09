using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApacheCtrl : MonoBehaviour
{
    public float moveSpeed = 0f;
    public float rotSpeed = 0f;
    private float VerticalSpeed = 0f;
    Transform tr;

    void Start()
    {
        tr = GetComponent<Transform>();
    }

    void FixedUpdate()      // Ű���带 ������ ����ŭ �̵��ϱ� ���� ����
    {
        #region �¿�� ȸ�� �ϴ� ����
        if (Input.GetKey(KeyCode.A))
            rotSpeed += -0.05f;
        else if (Input.GetKey(KeyCode.D))
            rotSpeed += 0.05f;
        else
        {
            if (rotSpeed > 0f) rotSpeed += -0.05f;
            else if (rotSpeed < 0f) rotSpeed += 0.05f;
        }
        tr.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
        #endregion

        #region �յڷ� �̵� �ϴ� ����
        if (Input.GetKey(KeyCode.W))
            moveSpeed += 0.05f;
        else if (Input.GetKey(KeyCode.S))
            moveSpeed += -0.05f;
        else
        {
            if (moveSpeed > 0f) moveSpeed += -0.05f;
            else if (moveSpeed < 0f) moveSpeed += 0.05f;
        }
        tr.Translate (Vector3.forward * moveSpeed * Time.deltaTime, Space.Self);
        #endregion

        #region ���, �ϰ� ����
        if (Input.GetKey(KeyCode.Space))
            VerticalSpeed += 0.05f;
        else if (Input.GetKey(KeyCode.LeftControl))
            VerticalSpeed += -0.05f;
        else
            VerticalSpeed = 0;

        tr.Translate (Vector3.up * VerticalSpeed * Time.deltaTime, Space.Self);
        #endregion
    }
}
