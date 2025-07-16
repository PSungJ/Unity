using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

public class ZombieSpawner : MonoBehaviourPun, IPunObservable
{
    public Zombie zombiePrefab;         // ������ ���� ������
    public ZombieData[] zombieDatas;    // ����� ���� �¾� ������
    public List<Transform> spawnPointList;     // ���� ��ȯ�� ��ġ
    
    private List<Zombie> zombieList = new List<Zombie>();  // ������ ���� ��� ����Ʈ
    private int zombieCount = 0;   // ���� �����
    private int wave;   // ���� ���̺� �ܰ�

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(zombieList.Count);      // ���� ������� ���� ���̺긦 ��Ʈ��ũ�� ���� ����Ʈ�� ������
            stream.SendNext(wave);
        }
        else
        {
            zombieCount = (int)stream.ReceiveNext();   // ���� ������� ���� ���̺긦 ��Ʈ��ũ�� ���� ���ÿ��� �ޱ�
            wave = (int)stream.ReceiveNext();
        }
    }
    
    void Awake()
    {
        // ���� ������ �÷��� ����ȭ, ������ȭ �ϴ� �޼���
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
            // ���ӿ��� ���¶�� �������� ����
            if (GameManager.instance != null && GameManager.instance.isGameOver)
                return;

            // ���� ��� ����ģ ��� ���� ���� ����
            if (zombieList.Count <= 0)
                SpawnWave();
        }
        UpdateUI();
    }

    private void UpdateUI() // ���̺� ������ UI�� ǥ��
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // ȣ��Ʈ�� ���� ������ ���� ����Ʈ�� ���� ���� ������� ǥ��
            UIManager.UI_instance.UpdateWaveText(wave, zombieList.Count);
        }
        else
        {
            // ����Ʈ Ŭ���̾�Ʈ�� ���� ����Ʈ�� ������ �� �����Ƿ�, ȣ��Ʈ�� ������ zombieCount�� ���� ������ ���� ǥ����
            UIManager.UI_instance.UpdateWaveText(wave, zombieCount);
        }
    }

    private void SpawnWave()    // ���� ���̺꿡 ���� ���� ����
    {
        wave++;
        // ���� ���̺꿡 * 1.5�� �ݿø��� ���� ŭ ���� ����
        int spawnCount = Mathf.RoundToInt(wave * 1.5f);

        // ���̺꿡 ���� ������ ���� �� ����
        for (int i = 0; i < spawnCount; i++)
        {
            CreateZombie();
        }
    }

    private void CreateZombie() // ���� �����ϰ� ������ ���񿡰� ������� �Ҵ�
    {
        ZombieData zombieData = zombieDatas[Random.Range(0, zombieDatas.Length)];       // ���� ���� ����
        Transform spawnPoint = spawnPointList[Random.Range(0, spawnPointList.Count)];   // ���� ��ġ ����
        GameObject createZombie = PhotonNetwork.Instantiate(zombiePrefab.gameObject.name, spawnPoint.position, spawnPoint.rotation);
        Zombie zombie = createZombie.GetComponent<Zombie>();

        zombie.photonView.RPC("SetUp", RpcTarget.All, zombieData.health, zombieData.damage, zombieData.speed, zombieData.skinColor);   // ���� ���� ����

        zombieList.Add(zombie);     // ������ ���� ����Ʈ�� �߰�
        // ���ٽ����� �͸� �޼��� ���
        zombie.onDeath += () => zombieList.Remove(zombie);  // ���� ������ ����Ʈ���� ����
        zombie.onDeath += () => StartCoroutine(DestroyAfter(zombie.gameObject, 10f));    // ���� ������ ���� ������Ʈ �ı�
        zombie.onDeath += () => GameManager.instance.AddScore(100); // ���� ������ ���� �߰�
    }

    IEnumerator DestroyAfter(GameObject target, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (target != null)
            PhotonNetwork.Destroy(target);
    }
}
