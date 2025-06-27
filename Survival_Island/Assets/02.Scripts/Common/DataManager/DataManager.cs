using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;    // �ϵ��ũ�� ���� ��� ��Ʈ������ ��/����� �ؾ��Ѵ�.
using System.Runtime.Serialization.Formatters.Binary;   // ���̳ʸ� �������� ����ȭ�� ������ȭ�� �ϱ� ����
using DataInfo;
public class DataManager : MonoBehaviour
{
    public string dataPath;
    public void Initialize()        // ���� ��� �� ���� ����
    {
        dataPath = Application.persistentDataPath + "/Survial_Island.dat";     // ���ӵ����� ���� ��� ����
    }
    public void Save(GameData gameData)
    {
        BinaryFormatter bf = new BinaryFormatter(); // ���̳ʸ� ���� ����
        FileStream file = File.Create(dataPath);    // ������ ������ ���� ���� ��Ʈ�� ����

        GameData data = new GameData();
        //data.killCount = gameData.killCount;
        data.hp = gameData.hp;
        data.damage = gameData.damage;
        data.speed = gameData.speed;
        data.equipItems = gameData.equipItems;
        bf.Serialize(file, data);   // ����ȭ�Ͽ� ���Ͽ� ����
        file.Close();   // ���� ��Ʈ�� �ݱ�
    }
    public GameData Load()
    {
        if (File.Exists(dataPath))  // ������ �����Ѵٸ�
        {
            BinaryFormatter bf = new BinaryFormatter(); // ���̳ʸ� ������ ����
            FileStream file = File.Open(dataPath, FileMode.Open);   // ���� ��Ʈ�� ����
            GameData data = (GameData)bf.Deserialize(file); // ������ȭ�Ͽ� ������ �б�
            file.Close();   //���� ��Ʈ�� �ݱ�
            return data;    // ���� ������ ��ȯ
        }
        else
        {
            GameData data = new GameData(); // ������ ������ �� ������ ����
            return data;
        }
    }
}
