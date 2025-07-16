using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

public class ZombieSpawner : MonoBehaviourPun, IPunObservable
{
    public Zombie zombiePrefab;         // 생성할 좀비 프리팹
    public ZombieData[] zombieDatas;    // 사용할 좀비 셋업 데이터
    public List<Transform> spawnPointList;     // 좀비를 소환할 위치
    
    private List<Zombie> zombieList = new List<Zombie>();  // 생성된 좀비를 담는 리스트
    private int zombieCount = 0;   // 남은 좀비수
    private int wave;   // 현재 웨이브 단계

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(zombieList.Count);      // 남은 좀비수와 현재 웨이브를 네트워크를 통해 리모트에 보내기
            stream.SendNext(wave);
        }
        else
        {
            zombieCount = (int)stream.ReceiveNext();   // 남은 좀비수와 현재 웨이브를 네트워크를 통해 로컬에게 받기
            wave = (int)stream.ReceiveNext();
        }
    }
    
    void Awake()
    {
        // 좀비가 렌더러 컬러를 직렬화, 역직렬화 하는 메서드
        PhotonPeer.RegisterType(typeof(Color), 128, ColorSerialization.SerializeColor, ColorSerialization.DeserializeColor);
    }

    private void Start()
    {
        //var spawnPoints = GameObject.Find("Spawn Points");
        //if (spawnPoints != null)
        //{
        //    spawnPoints.GetComponentsInChildren<Transform>(spawnPointList);
        //}
        //spawnPointList.RemoveAt(0);

        zombieDatas = Resources.LoadAll<ZombieData>("ZombieData");
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // 게임오버 상태라면 실행하지 않음
            if (GameManager.instance != null && GameManager.instance.isGameOver)
                return;

            // 좀비를 모두 물리친 경우 다음 스폰 실행
            if (zombieList.Count <= 0)
                SpawnWave();
        }
        UpdateUI();
    }

    private void UpdateUI() // 웨이브 정보를 UI에 표시
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // 호스트는 직접 갱신한 좀비 리스트를 통해 남은 좀비수를 표시
            UIManager.UI_instance.UpdateWaveText(wave, zombieList.Count);
        }
        else
        {
            // 리모트 클라이언트는 좀비 리스트를 갱신할 수 없으므로, 호스트가 보내준 zombieCount를 통해 좀비의 수를 표시함
            UIManager.UI_instance.UpdateWaveText(wave, zombieCount);
        }
    }

    private void SpawnWave()    // 현재 웨이브에 맞춰 좀비 생성
    {
        wave++;
        // 현재 웨이브에 * 1.5를 반올림한 수만 큼 좀비 생성
        int spawnCount = Mathf.RoundToInt(wave * 1.5f);

        // 웨이브에 따라 생성할 좀비 수 증가
        for (int i = 0; i < spawnCount; i++)
        {
            CreateZombie();
        }
    }

    private void CreateZombie() // 좀비를 생성하고 생성한 좀비에게 추적대상 할당
    {
        ZombieData zombieData = zombieDatas[Random.Range(0, zombieDatas.Length)];       // 좀비 랜덤 생성
        Transform spawnPoint = spawnPointList[Random.Range(0, spawnPointList.Count)];   // 스폰 위치 랜덤
        GameObject createZombie = PhotonNetwork.Instantiate(zombiePrefab.gameObject.name, spawnPoint.position, spawnPoint.rotation);
        Zombie zombie = createZombie.GetComponent<Zombie>();

        zombie.photonView.RPC("SetUp", RpcTarget.All, zombieData.health, zombieData.damage, zombieData.speed, zombieData.skinColor);   // 좀비 스펙 설정

        zombieList.Add(zombie);     // 생성한 좀비를 리스트에 추가
        // 람다식으로 익명 메서드 등록
        zombie.onDeath += () => zombieList.Remove(zombie);  // 좀비가 죽으면 리스트에서 제거
        zombie.onDeath += () => StartCoroutine(DestroyAfter(zombie.gameObject, 10f));    // 좀비가 죽으면 게임 오브젝트 파괴
        zombie.onDeath += () => GameManager.instance.AddScore(100); // 좀비가 죽으면 점수 추가
    }

    IEnumerator DestroyAfter(GameObject target, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (target != null)
            PhotonNetwork.Destroy(target);
    }
}
