using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Rocket : MonoBehaviour
{
    private float h;   //수평 이동 A,D
    private float v;   //수직 이동 W,S
    //public float speed = 5f;
    private Transform tr;
    public GameObject effect;
    public AudioSource source;  //오디오 소스
    public AudioClip hitclip;   //오디오 클립
    private Vector2 StartPos = Vector2.zero;    //시작위치
    public Transform firePos;   //총알 발사 위치
    public GameObject coinPrefab;   //총알
    private Rigidbody2D rb;
    void Start()
    {
        source = GetComponent<AudioSource>();   //오디오 소스 컴퍼넌트 불러오기
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();

    }
    public void OnStickPos(Vector2 stickPos)
    {
        h = stickPos.x;
        v = stickPos.y;
    }
    void Update()
    {
        if (GameManager.instance.isGameOver == true)
        {
            return; // 여기서부터 하위 로직으로 진행하지 않음
        }
        #region 기존 코드
        //if (Application.platform == RuntimePlatform.WindowsEditor) //윈도우에서 실행 시
        //{
        //    h = Input.GetAxis("Horizontal");
        //    v = Input.GetAxis("Vertical");
        //    Vector3 Normal = (h * Vector3.right) + (v * Vector3.up);
        //    tr.Translate(Normal.normalized * speed * Time.deltaTime);
        //    //tr.Translate(Vector3.right * h * speed * Time.deltaTime);
        //    //tr.Translate(Vector3.up * v * speed * Time.deltaTime);
        //}
        //if (Application.platform == RuntimePlatform.Android) //안드로이드에서 실행 시
        //{
        //    if (Input.touchCount > 0) //한 번이라도 터치가 되었다면
        //    {
        //        Touch touch = Input.GetTouch(0);
        //                    //GetTouch(0) : 터치한 위치값을 배열에 저장하고 첫번째 터치한 위치를 가져옴
        //        switch (touch.phase)
        //        {
        //            case TouchPhase.Began:  //터치 시작
        //                StartPos = transform.position;  //터치한 위치 저장
        //                break;
        //            case TouchPhase.Moved:  //터치가 이동
        //                Vector2 touchDelta = touch.position - StartPos; //터치한 위치 - 시작위치 거리를 구함
        //                Vector2 moveDir = new Vector3(touchDelta.x, touchDelta.y);
        //                tr.Translate(moveDir.normalized * speed * Time.deltaTime);
        //                StartPos = touch.position;
        //                break;

        //        }
        //    }
        #endregion
        if (rb != null)
        {
            Vector2 speed = rb.velocity;    // 속력
            speed.x = 4 * h;
            speed.y = 4 * v;
            rb.velocity = speed;
        }


    #region 초보자 로직
    //if (tr.position.x >= 7.6f)
    //    tr.position = new Vector3(7.6f, tr.position.y, tr.position.z);
    //else if (tr.position.x <= -7.8f)
    //    tr.position = new Vector3(-7.8f, tr.position.y, -tr.position.z);

    //if (tr.position.y >= 4.3f)
    //    tr.position = new Vector3(tr.position.x, 4.3f, tr.position.z);
    //else if (tr.position.y <= -4.3)
    //    tr.position = new Vector3(tr.position.x, -4.3f, -tr.position.z);
    #endregion

    #region 중급자 로직
    tr.position = new Vector3(Mathf.Clamp(tr.position.x, -7f, 7f), Mathf.Clamp(tr.position.y, -4.3f, 4.3f), tr.position.z);
                                //Mathf.Clamp : 값을 제한하는 함수 (무엇을, min, max)
        #endregion
    }
    //충돌 트리거 콜백 함수
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Asteroid")
        {
            col.gameObject.SetActive(false);
            //Destroy(col.gameObject); //충돌한 오브젝트 삭제 (운석)
            Debug.Log($"충돌");
            var eff = Instantiate(effect, col.transform.position, Quaternion.identity); //이펙트 생성
            Destroy(eff ,1f); //이펙트 삭제
            GameManager.instance.TurnOn();  // GameManger의 카메라 흔들림 함수 호출
            source.PlayOneShot(hitclip, 1f);// 소리를 한번만 재생
            //PlayOneShot:소리를 한번만 재생(무엇을, 볼륨크기(최대1f))
            GameManager.instance.RocketHealthPoint(50);
        }
    }
    public void Fire()
    {
        //프리팹 생성 함수(What?, Where?, How Rotation)
        Instantiate(coinPrefab, firePos.position, Quaternion.identity); //총알 생성
        Destroy(coinPrefab, 1f);    //1초 후 총알 삭제
    }
}
