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
        string filePath = Path.Combine(Application.persistentDataPath, "Ranking.json");//�ش� ��ο� ���� ����

        if (File.Exists(filePath))//�ش� ��ο� ������ ���� �ϴ� ��?
        {
            string json = File.ReadAllText(filePath);// ��ü ������ ���ڿ��� ����
            data = JsonUtility.FromJson<RankingData>(json);//���� �ȿ��ִ� �����͸� ��ȯ�� �ϰ� data�� �ִ´�
        }
        else
        {
            data = new RankingData();//���� ���� ������ ���ο� List �����
        }
        UIUpdate();
    }

    public void _SaveData()
    {
        data.names.Add(NameSave.nameSave.myName);//�ȳ��̶�� ������ �߰�
        data.scores.Add(SaveData.saveData.SaveScore.ToString());//122�̶�� ������ �߰�
        Data_Sort();

        if (data.scores.Count > 5)//5���� Ŀ���ٸ�?
        {
            data.names.RemoveAt(5);//�ε��� 5�� ����
            data.scores.RemoveAt(5);//�ε��� 5�� ����
        }

        string json = JsonUtility.ToJson(data, true); //string�� json���� ��ȯ ���ش�
        string filePath = Path.Combine(Application.persistentDataPath, "Ranking.json");//���� ��� ����
        File.WriteAllText(filePath, json);//filePath�� json�� �Ҵ�

        UIUpdate();
    }


    void UIUpdate()
    {
        asdf.text = string.Empty;//�ؽ�Ʈ�� ����

        for (int i = 0; i < 5; i++)
        {
            if (data.names.Count > i)//����� �����Ͱ� ������?
                asdf.text += $"{i + 1} .... {data.names[i]} : {data.scores[i]}\n";
            else //���ٸ�?
                asdf.text += $"{i + 1} .... ��ϵ� ���� ����\n";
        }
    }

    void Data_Sort()
    {
        //����
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
