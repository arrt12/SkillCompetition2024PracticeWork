using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class RankingData
{
    public List<string> names = new();
    public List<string> scores = new();
}

public class Ranking : MonoBehaviour
{
    RankingData data = new();
    [SerializeField] Text asdf;
    public static Ranking ranking;

    private void Awake()
    {
        if (ranking == null)
            ranking = this;
        else
            Destroy(ranking);
    }
    void Start()
    {
        LoadData();
    }

    private void Update()
    {
        LoadData();
    }

    void LoadData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "Ranking.json");//해당 경로에 파일 생성

        if (File.Exists(filePath))//해당 경로에 파일이 존재 하는 가?
        {
            string json = File.ReadAllText(filePath);// 전체 파일을 문자열로 읽음
            data = JsonUtility.FromJson<RankingData>(json);//파일 안에있는 데이터를 전환을 하고 data에 넣는다
        }
        else
        {
            data = new RankingData();//존재 하지 않으면 새로운 List 만든다
        }
        UIUpdate();
    }

    public void _SaveData()
    {
        data.names.Add(NameSave.nameSave.myName);//안녕이라는 데이터 추가
        data.scores.Add(SaveData.saveData.SaveScore.ToString());//122이라는 데이터 추가
        Data_Sort();

        if (data.scores.Count > 5)//5보다 커진다면?
        {
            data.names.RemoveAt(5);//인덱스 5번 삭제
            data.scores.RemoveAt(5);//인덱스 5번 삭제
        }

        string json = JsonUtility.ToJson(data, true); //string을 json으로 변환 해준다
        string filePath = Path.Combine(Application.persistentDataPath, "Ranking.json");//파일 경로 생성
        File.WriteAllText(filePath, json);//filePath에 json을 할당

        UIUpdate();
    }


    void UIUpdate()
    {
        asdf.text = string.Empty;//텍스트를 비우고

        for (int i = 0; i < 5; i++)
        {
            if (data.names.Count > i)//저장된 데이터가 있으면?
                asdf.text += $"{i + 1} .... {data.names[i]} : {data.scores[i]}\n";
            else //없다면?
                asdf.text += $"{i + 1} .... 등록된 정보 없음\n";
        }
    }

    void Data_Sort()
    {
        //정렬
        for (int i = 0; i < data.scores.Count; i++)
        {
            for (int j = 0; j < data.scores.Count; j++)
            {
                if (int.Parse(data.scores[i]) > int.Parse(data.scores[j]))
                {
                    string copy = data.scores[i];
                    data.scores[i] = data.scores[j];
                    data.scores[j] = copy;

                    string copy2 = data.names[i];
                    data.names[i] = data.names[j];
                    data.names[j] = copy2;
                }
            }
        }
    }
}
