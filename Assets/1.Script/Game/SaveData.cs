using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    #region 변수 선언
    public static SaveData Instance { get; private set; }
    public int saveScore { get; set; }
    #endregion

    #region 생명주기 함수
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
