using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

#region ������ Ŭ����
public class RankingData
{
    public List<string> names = new();
    public List<string> scores = new();
}
#endregion

public class Ranking : MonoBehaviour
{
    #region ���� ����
    private RankingData data = new();
    [SerializeField] private Text asdf;
    public static Ranking ranking;
    #endregion

    #region �����ֱ� �Լ�
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

    #region ������ �ε� / ����
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

    #region UI ����
    void UIUpdate()
    {
        asdf.text = string.Empty;

        for (int i = 0; i < 5; i++)
        {
            if (data.names.Count > i)
                asdf.text += $"{i + 1} .... {data.names[i]} : {data.scores[i]}\n";
            else
                asdf.text += $"{i + 1} .... ��ϵ� ���� ����\n";
        }
    }
    #endregion

    #region ������ ����
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
