using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;    // 하드디스크로 가는 통로 스트림으로 입/출력을 해야한다.
using System.Runtime.Serialization.Formatters.Binary;   // 바이너리 포맷으로 직렬화와 역직렬화를 하기 위해
using DataInfo;
public class DataManager : MonoBehaviour
{
    public string dataPath;
    public void Initialize()        // 파일 경로 및 파일 저장
    {
        dataPath = Application.persistentDataPath + "/Survial_Island.dat";     // 게임데이터 파일 경로 설정
    }
    public void Save(GameData gameData)
    {
        BinaryFormatter bf = new BinaryFormatter(); // 바이너리 포맷 생성
        FileStream file = File.Create(dataPath);    // 데이터 저장을 위한 파일 스트림 생성

        GameData data = new GameData();
        //data.killCount = gameData.killCount;
        data.hp = gameData.hp;
        data.damage = gameData.damage;
        data.speed = gameData.speed;
        data.equipItems = gameData.equipItems;
        bf.Serialize(file, data);   // 직렬화하여 파일에 저장
        file.Close();   // 파일 스트림 닫기
    }
    public GameData Load()
    {
        if (File.Exists(dataPath))  // 파일이 존재한다면
        {
            BinaryFormatter bf = new BinaryFormatter(); // 바이너리 포맷터 생성
            FileStream file = File.Open(dataPath, FileMode.Open);   // 파일 스트림 열기
            GameData data = (GameData)bf.Deserialize(file); // 역직렬화하여 데이터 읽기
            file.Close();   //파일 스트림 닫기
            return data;    // 읽은 데이터 반환
        }
        else
        {
            GameData data = new GameData(); // 파일이 없으면 새 데이터 생성
            return data;
        }
    }
}
