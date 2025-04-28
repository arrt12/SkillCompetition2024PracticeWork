using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Game
{
    start,
    gaming,
    midBoss,
    endBoss,
    end,
    Dead
}

public class GameManager : MonoBehaviour
{
    public Game game;
    public static GameManager gameManager;
    [SerializeField] GameObject midBoss;
    [SerializeField] GameObject EndBossObject;
    public int kills = 0;
    public int BossKills = 0;
    public float GameeTime = 0;
    public int Score = 0;
    public float time = 0;
    int count;
    private bool isEnd = false;
    public bool isGameEnd = false;
    public bool isDead = false;

    private void Awake()
    {
        if (gameManager == null)
            gameManager = this;
        else
            Destroy(gameObject);

        Screen.SetResolution(1920, 1080, false);
    }

    private void Update()
    {
        Score = (((int)GameeTime * 10) + (BossKills * 100) + (kills * 10)) / 10;
        GameFlow();
        if (!isGameEnd)
            GameeTime += Time.deltaTime;
    }

    void GameFlow()
    {
        if (game == Game.gaming)
            time += Time.deltaTime;

        if (time >= 90 && count <= 0)
        {
            game = Game.midBoss;
            midBoss.SetActive(true);
            StageEnd();
            count++;
        }
        else if (time >= 180 && !isEnd)
        {
            game = Game.endBoss;
            EndBossObject.SetActive(true);
            StageEnd();
            isEnd = true;
        }
    }

    void StageEnd()
    {
        CleanupEntities();
    }

    public void CleanupEntities()
    {
        // 몬스터들 전부 삭제
        foreach (var monster in FindObjectsOfType<Monster>())
            Destroy(monster.gameObject);

        // 총알 전부 삭제
        foreach (var bullet in FindObjectsOfType<Bullet>())
            Destroy(bullet.gameObject);

        // 스폰 멈추기
        foreach (var spawner in FindObjectsOfType<SpawnManager>())
            spawner.isBool = IsBool.isFalse;

        // MidBoss랑 EndBoss도 예외처리
        if (MidBoss.Instance != null)
            Destroy(MidBoss.Instance.gameObject);

        if (EndBoss.endBoss != null)
            Destroy(EndBoss.endBoss.gameObject);
    }
}
