using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public static SaveData saveData;
    public int SaveScore;
    private void Awake()
    {
        if (saveData == null)
            saveData = this;
        else
            Destroy(saveData);
        DontDestroyOnLoad(gameObject);
    }
}
