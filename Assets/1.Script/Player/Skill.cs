using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 스킬 사용 및 쿨타임 관리
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

    #region 상태 및 설정
    [Header("스킬 오브젝트")]
    [SerializeField] private GameObject bomb;

    [Header("스킬 UI")]
    [SerializeField] private Image skill1Image;
    [SerializeField] private Image skill2Image;

    [Header("스킬 쿨타임")]
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

    #region 스킬 처리
    private void UpdateSkillCooldowns()
    {
        // 스킬 1
        if (skill1Cooldown < MaxSkill1Cooldown)
        {
            skill1Cooldown += Time.deltaTime;
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            UseSkill1();
            skill1Cooldown = 0f;
        }

        // 스킬 2
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
        Debug.Log("체력 회복");
        Player.player.hp = 100;
    }
    #endregion

    #region UI 처리
    private void UpdateSkillUI()
    {
        skill1Image.fillAmount = skill1Cooldown / MaxSkill1Cooldown;
        skill2Image.fillAmount = skill2Cooldown / MaxSkill2Cooldown;
    }
    #endregion
}
