using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public GameObject[] obstacles = new GameObject[3];
    private bool isStep;    // 플레이어가 발판을 밟았는지 여부를 저장

    private void OnEnable() // 오브젝트가 활성화될 때 호출, 활성화 비활성화를 반복할 때 마다 호출
    {
        for (int i = 0; i < obstacles.Length; i++)
        {
            obstacles[i] = transform.GetChild(i).gameObject;
        }
        isStep = false;
        for (int i = 0; i < obstacles.Length; i++)
        {
            if (Random.Range(0, 3) == 0)
            {
                obstacles[i].SetActive(true);   // 0~2사이의 랜덤값이 0일때만 활성화
            }
            else
            {
                obstacles[i].SetActive(false);
            }
        }
        Debug.Log("Platform 활성화");
    }
    private void Start()
    {
        GameManager.instance.isGameOver = false;
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && !isStep)
        {
            isStep = true;
            GameManager.instance.AddScore(1);   // 점수 추가
        }
        Debug.Log("충돌");
    }
    private void OnDisable()
    {
        Debug.Log("Platform 비활성화");
    }
}
