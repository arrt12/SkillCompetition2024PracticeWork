using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

#region 데이터 클래스
public class RankingData
{
    public List<string> names = new();
    public List<string> scores = new();
}
#endregion

public class Ranking : MonoBehaviour
{
    #region 변수 선언
    private RankingData data = new();
    [SerializeField] private Text asdf;
    public static Ranking ranking;
    #endregion

    #region 생명주기 함수
    private void Awake()
    {
        if (ranking == null)
            ranking = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        LoadData();
    }

    private void Update()
    {
        LoadData();
    }
    #endregion

    #region 데이터 로드 / 저장
    void LoadData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "Ranking.json");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            data = JsonUtility.FromJson<RankingData>(json);
        }
        else
            data = new RankingData();
        UIUpdate();
    }

    public void _SaveData()
    {
        data.names.Add(NameSave.nameSave.myName);
        data.scores.Add(SaveData.Instance.saveScore.ToString());
        Data_Sort();

        if (data.scores.Count > 5)
        {
            data.names.RemoveAt(5);
            data.scores.RemoveAt(5);
        }

        string json = JsonUtility.ToJson(data, true);
        string filePath = Path.Combine(Application.persistentDataPath, "Ranking.json");
        File.WriteAllText(filePath, json);

        UIUpdate();
    }
    #endregion

    #region UI 갱신
    void UIUpdate()
    {
        asdf.text = string.Empty;

        for (int i = 0; i < 5; i++)
        {
            if (data.names.Count > i)
                asdf.text += $"{i + 1} .... {data.names[i]} : {data.scores[i]}\n";
            else
                asdf.text += $"{i + 1} .... 등록된 정보 없음\n";
        }
    }
    #endregion

    #region 데이터 정렬
    void Data_Sort()
    {
        for (int i = 0; i < data.scores.Count; i++)
        {
            for (int j = 0; j < data.scores.Count; j++)
            {
                if (int.Parse(data.scores[i]) > int.Parse(data.scores[j]))
                {
                    (data.scores[i], data.scores[j]) = (data.scores[j], data.scores[i]);
                    (data.names[i], data.names[j]) = (data.names[j], data.names[i]);
                }
            }
        }
    }
    #endregion
}
