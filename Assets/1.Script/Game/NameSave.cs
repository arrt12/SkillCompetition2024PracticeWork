using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameSave : MonoBehaviour
{
    #region ���� ����
    [SerializeField] InputField inputP;
    [SerializeField] GameObject PlayerName;
    public string myName = string.Empty;
    public static NameSave nameSave;
    #endregion

    #region �����ֱ�
    private void Awake()
    {
        if (nameSave == null)
            nameSave = this;
        else
            Destroy(gameObject);
    }
    #endregion

    #region ��ư �Լ�
    public void Button_Save()
    {
        if (inputP.text.Length != 3)
        {
            inputP.text = "���ڼ� �ʰ�";
        }
        else
        {
            myName = inputP.text;
            Ranking.ranking._SaveData();
            PlayerName.SetActive(false);
        }
    }
    #endregion
}
