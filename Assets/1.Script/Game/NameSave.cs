using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameSave : MonoBehaviour
{
    [SerializeField] InputField inputP;
    [SerializeField] GameObject PlayerName;
    public string myName = string.Empty;
    public static NameSave nameSave;

    private void Awake()
    {
        if (nameSave == null)
            nameSave = this;
        else
            Destroy(nameSave);
    }


    public void Button_Save()
    {
        if (inputP.text.Length != 3)
        {
            inputP.text = "글자수 초과";
        }
        else
        {
            myName = inputP.text;
            Ranking.ranking._SaveData();
            PlayerName.SetActive(false);
        }
    }
}
