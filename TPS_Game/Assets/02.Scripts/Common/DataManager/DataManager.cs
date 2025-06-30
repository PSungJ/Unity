using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using DataInfo;
public class DataManager : MonoBehaviour
{
    public string dataPath;
    public void Initalize()
    {
        dataPath = Application.persistentDataPath + "/TPSGameData.dat";
    }
    public void Save(GameData gameData)
    {
        BinaryFormatter bf = new BinaryFormatter();
        // ���̳ʸ� ������ ����
        FileStream file = File.Create(dataPath);
        //������ ������ ���� ���� ��Ʈ�� ����
        GameData data = new GameData();
        data.killCount = gameData.killCount;
        data.hp = gameData.hp;
        data.damage = gameData.damage;
        data.speed = gameData.speed;
        data.equipItems = gameData.equipItems;
        bf.Serialize(file, data);
        file.Close();
    }
    public GameData Load()
    {
        if (File.Exists(dataPath)) //�ϵ��Ʈ�� ����� ������ ���� �Ѵٸ�
        {
            //������ ���� �� ��� ������ �ҷ����� 
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(dataPath, FileMode.Open);
            GameData data = (GameData)bf.Deserialize(file); //������ȭ 
                                                            //byte�� �������Ϸ� �Ǿ� �ִ� ���� �ٽ� int�� float string���� ��ȯ �Ѵ�.
            file.Close(); //�̵���� ���� 
            return data;
        }
        else
        {
            GameData data = new GameData();
            return data;
        }
    }
}
