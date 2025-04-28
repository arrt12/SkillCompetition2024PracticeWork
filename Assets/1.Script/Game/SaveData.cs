using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    #region ���� ����
    public static SaveData Instance { get; private set; }
    public int saveScore { get; set; }
    #endregion

    #region �����ֱ� �Լ�
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    #endregion
}
