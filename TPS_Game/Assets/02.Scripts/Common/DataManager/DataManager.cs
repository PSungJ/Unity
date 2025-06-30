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
        // 바이너리 포맷터 생성
        FileStream file = File.Create(dataPath);
        //데이터 저장을 위한 파일 스트림 생성
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
        if (File.Exists(dataPath)) //하드디스트에 저장된 파일이 존재 한다면
        {
            //파일이 존재 할 경우 데이터 불러오기 
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(dataPath, FileMode.Open);
            GameData data = (GameData)bf.Deserialize(file); //역직렬화 
                                                            //byte로 이진파일로 되어 있는 것을 다시 int나 float string으로 변환 한다.
            file.Close(); //이동통로 닫음 
            return data;
        }
        else
        {
            GameData data = new GameData();
            return data;
        }
    }
}
