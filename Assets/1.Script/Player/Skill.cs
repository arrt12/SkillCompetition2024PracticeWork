using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��ų ��� �� ��Ÿ�� ����
/// </summary>
public class Skill : MonoBehaviour
{
    #region Singleton
    public static Skill skill;

    private void Awake()
    {
        if (skill == null)
            skill = this;
        else
            Destroy(gameObject);
    }
    #endregion

    #region ���� �� ����
    [Header("��ų ������Ʈ")]
    [SerializeField] private GameObject bomb;

    [Header("��ų UI")]
    [SerializeField] private Image skill1Image;
    [SerializeField] private Image skill2Image;

    [Header("��ų ��Ÿ��")]
    public float skill1Cooldown = 20f;
    public float skill2Cooldown = 30f;
    private const float MaxSkill1Cooldown = 20f;
    private const float MaxSkill2Cooldown = 30f;
    #endregion

    #region Unity Life Cycle
    private void Start()
    {
        skill1Cooldown = MaxSkill1Cooldown;
        skill2Cooldown = MaxSkill2Cooldown;
    }

    private void Update()
    {
        if (GameManager.gameManager.game == Game.start || GameManager.gameManager.game == Game.end)
            return;

        UpdateSkillCooldowns();
        UpdateSkillUI();
    }
    #endregion

    #region ��ų ó��
    private void UpdateSkillCooldowns()
    {
        // ��ų 1
        if (skill1Cooldown < MaxSkill1Cooldown)
        {
            skill1Cooldown += Time.deltaTime;
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            UseSkill1();
            skill1Cooldown = 0f;
        }

        // ��ų 2
        if (skill2Cooldown < MaxSkill2Cooldown)
            skill2Cooldown += Time.deltaTime;
        else if (Input.GetKeyDown(KeyCode.C))
        {
            UseSkill2();
            skill2Cooldown = 0f;
        }
    }

    private void UseSkill1()
    {
        Vector3 spawnPosition = transform.position + Vector3.up;
        Instantiate(bomb, spawnPosition, Quaternion.identity);

        if (Bomb.bomb != null)
            Bomb.bomb.rig.AddForce(Vector3.up * 6 + Vector3.back * 2, ForceMode.Impulse);
    }

    private void UseSkill2()
    {
        Debug.Log("ü�� ȸ��");
        Player.player.hp = 100;
    }
    #endregion

    #region UI ó��
    private void UpdateSkillUI()
    {
        skill1Image.fillAmount = skill1Cooldown / MaxSkill1Cooldown;
        skill2Image.fillAmount = skill2Cooldown / MaxSkill2Cooldown;
    }
    #endregion
}
