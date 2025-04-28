using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 치트 코드 기능
/// </summary>
public class Cheat : MonoBehaviour
{
    #region Unity Life Cycle
    private void Update()
    {
        if (GameManager.gameManager.game == Game.start)
            return;

        CheckCheatInputs();
    }
    #endregion

    #region 치트 키 입력
    private void CheckCheatInputs()
    {
        if (Input.GetKeyDown(KeyCode.F1)) KillAllEnemies();
        if (Input.GetKeyDown(KeyCode.F2)) SetPlayerLevel();
        if (Input.GetKeyDown(KeyCode.F3)) ResetSkillCooldowns();
        if (Input.GetKeyDown(KeyCode.F4)) HealPlayer();
        if (Input.GetKeyDown(KeyCode.F5)) RefillFuel();
        if (Input.GetKeyDown(KeyCode.F6)) SkipTime();
    }
    #endregion

    #region 치트 기능
    private void KillAllEnemies()
    {
        Monster[] monsters = FindObjectsOfType<Monster>();
        MidBoss midBoss = FindObjectOfType<MidBoss>();
        EndBoss endBoss = FindObjectOfType<EndBoss>();

        if (monsters != null)
        {
            foreach (var monster in monsters)
                monster.hp = 0;
        }
        if (midBoss != null)
            midBoss.hp = 0;
        if (endBoss != null)
            endBoss.hp = 0;
    }

    private void SetPlayerLevel() => Player.player.LevelCount = 3;

    private void ResetSkillCooldowns()
    {
        Skill.skill.skill1Cooldown = 20f;
        Skill.skill.skill2Cooldown = 30f;
    }

    private void HealPlayer() => Player.player.hp = 100;

    private void RefillFuel() => Player.player.fuel = 1000;

    private void SkipTime()
    {
        if (GameManager.gameManager.game == Game.gaming)
        {
            if (GameManager.gameManager.time < 90)
                GameManager.gameManager.time = 90;
            else if (GameManager.gameManager.time < 180)
                GameManager.gameManager.time = 180;
        }
        else if (GameManager.gameManager.game == Game.midBoss)
        {
            MidBoss.midBoss.hp = 0;
        }
        else if (GameManager.gameManager.game == Game.endBoss)
        {
            EndBoss.endBoss.hp = 0;
        }
    }
    #endregion
}
