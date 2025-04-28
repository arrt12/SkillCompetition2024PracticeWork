using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region 변수 선언
    int count = 0;
    [SerializeField] RectTransform startRect;
    [SerializeField] RectTransform PlayerstartRect;
    [SerializeField] RectTransform bossHpRect;
    [SerializeField] RectTransform stageRect;
    [SerializeField] RectTransform endRect;
    [SerializeField] RectTransform ClearRect;
    [SerializeField] Text text;
    [SerializeField] Text endText;
    [SerializeField] GameObject[] texts;
    [SerializeField] Text[] textValue;

    private bool isStart = false;
    private bool isPlayerStart = false;
    private bool isBossing = false;
    private bool isEnd = false;
    private bool isStage_1 = false;
    #endregion

    #region Unity 메소드
    void Update()
    {
        if (isStart)
        {
            StartCoroutine(Starting());
            isStart = false;
        }
        if (isPlayerStart)
        {
            StartCoroutine(PlayerStarting());
            isPlayerStart = false;
        }
        if (GameManager.gameManager.game == Game.midBoss || GameManager.gameManager.game == Game.endBoss)
        {
            if (!isBossing)
            {
                StartCoroutine(BossHp());
                isBossing = true;
            }
            if (MidBoss.midBoss.hp <= 0)
            {
                isStage_1 = false;
            }
        }
        if (GameManager.gameManager.game == Game.gaming)
        {
            if (isBossing)
            {
                StartCoroutine(BossHpUp());
                isBossing = false;
            }
            if (!isStage_1)
            {
                count++;
                text.text = count.ToString();
                StartCoroutine(Stage());
                isStage_1 = true;
            }
        }
        if (GameManager.gameManager.game == Game.end)
        {
            if (!isEnd)
            {
                print("End");
                count++;
                StartCoroutine(BossHpUp());
                isEnd = true;
            }
        }
        if (GameManager.gameManager.isDead)
        {
            StartCoroutine(GameOver());
            GameManager.gameManager.isDead = false;
        }
    }
    #endregion

    #region UI 동작
    public void UI_Start()
    {
        if (GameManager.gameManager.game != Game.start)
            return;
        isStart = true;
    }

    public void UI_Exit()
    {
        print("나가기");
        Application.Quit();
    }

    public void UI_Next()
    {
        SaveData.saveData.SaveScore = GameManager.gameManager.Score;
        SceneManager.LoadScene("RankingSceme");
    }
    #endregion

    #region 코루틴

    IEnumerator Starting()
    {
        float time = 0f;
        while (time <= 1)
        {
            time += Time.deltaTime;
            startRect.anchoredPosition = Vector2.Lerp(startRect.anchoredPosition, new Vector3(0, -810, 0), 3f * Time.deltaTime);
            yield return null;
        }
        startRect.anchoredPosition = new Vector3(0, -810, 0);
        GameManager.gameManager.game = Game.gaming;
        isPlayerStart = true;
        SpawnManager[] spawnPoint = FindObjectsOfType<SpawnManager>();
        foreach (var item in spawnPoint)
            item.isBool = IsBool.isTrue;
    }

    IEnumerator PlayerStarting()
    {
        float time = 0f;
        while (time <= 1)
        {
            time += Time.deltaTime;
            PlayerstartRect.anchoredPosition = Vector2.Lerp(PlayerstartRect.anchoredPosition, new Vector3(0, 0, 0), 4f * Time.deltaTime);
            yield return null;
        }
        PlayerstartRect.anchoredPosition = new Vector3(0, 0, 0);
    }

    IEnumerator Stage()
    {
        float t = 0f;
        while (t <= 2f)
        {
            t += Time.deltaTime;
            stageRect.anchoredPosition = Vector3.Lerp(stageRect.anchoredPosition, new Vector3(2100, 0, 0), 1.5f * Time.deltaTime);
            yield return null;
        }
        stageRect.anchoredPosition = new Vector3(2100, 0, 0);
        yield return new WaitForSecondsRealtime(0.1f);
        stageRect.anchoredPosition = new Vector3(-1920, 0, 0);

        if (count >= 2)
        {
            print("");
            SpawnManager[] spawnManager = FindObjectsOfType<SpawnManager>();
            foreach (var item in spawnManager)
            {
                if (item.isItem)
                    continue;
                item.spawnTime /= 2;
            }
            yield return MoveUI(endRect, new Vector3(0, -1, 0), 2f);
            endRect.anchoredPosition = Vector3.zero;

            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].SetActive(true);
                switch (i)
                {
                    case 0:
                        textValue[i].text = ((int)GameManager.gameManager.GameeTime).ToString();
                        break;
                    case 1:
                        textValue[i].text = GameManager.gameManager.BossKills.ToString();
                        break;
                    case 2:
                        textValue[i].text = GameManager.gameManager.kills.ToString();
                        break;
                    case 3:
                        textValue[i].text = GameManager.gameManager.Score.ToString();
                        break;
                }
                yield return new WaitForSecondsRealtime(0.5f);
            }

            yield return MoveUI(endRect, new Vector3(0, 850, 0), 2f);
            endRect.anchoredPosition = new Vector3(0, 850, 0);

            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].SetActive(false);
            }

            SpawnManager[] spawnManagers = FindObjectsOfType<SpawnManager>();
            foreach (var item in spawnManagers)
            {
                item.isBool = IsBool.isTrue;
            }
        }
    }

    IEnumerator BossHp()
    {
        yield return new WaitForSecondsRealtime(1f);
        yield return MoveUI(bossHpRect, new Vector3(0, 15, 0), 5f);
    }

    IEnumerator BossHpUp()
    {
        yield return new WaitForSecondsRealtime(1f);
        yield return MoveUI(bossHpRect, new Vector3(0, 200, 0), 5f);

        if (count >= 3)
        {
            GameManager.gameManager.isGameEnd = true;

            yield return MoveUI(endRect, new Vector3(0, -1, 0), 2f);
            endRect.anchoredPosition = Vector3.zero;

            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].SetActive(true);
                switch (i)
                {
                    case 0:
                        textValue[i].text = ((int)GameManager.gameManager.GameeTime).ToString();
                        break;
                    case 1:
                        textValue[i].text = GameManager.gameManager.BossKills.ToString();
                        break;
                    case 2:
                        textValue[i].text = GameManager.gameManager.kills.ToString();
                        break;
                    case 3:
                        textValue[i].text = ((((int)GameManager.gameManager.GameeTime * 10) + (GameManager.gameManager.BossKills * 100) + (GameManager.gameManager.kills * 10)) / 10).ToString();
                        GameManager.gameManager.Score = (((int)GameManager.gameManager.GameeTime * 10) + (GameManager.gameManager.BossKills * 100) + (GameManager.gameManager.kills * 10)) / 10;
                        break;
                }
                yield return new WaitForSecondsRealtime(0.5f);
            }

            yield return MoveUI(endRect, new Vector3(0, 850, 0), 2f);
            endRect.anchoredPosition = new Vector3(0, 850, 0);

            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].SetActive(false);
            }

            yield return MovePlayer(new Vector3(0, 0, -30), 2f);
            Player.player.transform.position = new Vector3(0, 0, -30);

            yield return MovePlayer(new Vector3(0, 0, 60), 1.5f);
            Player.player.transform.position = new Vector3(0, 0, 60);

            endText.text = "Game Clear";
            yield return MoveUI(ClearRect, new Vector2(0, -200f), 1.5f);
            ClearRect.anchoredPosition = Vector2.zero;
        }
    }

    IEnumerator GameOver()
    {
        endText.text = "Game Over";
        yield return MoveUI(ClearRect, new Vector2(0, -200f), 1.5f);
        ClearRect.anchoredPosition = Vector2.zero;
    }

    #endregion

    #region 유틸리티 메소드
    IEnumerator MoveUI(RectTransform rect, Vector3 target, float speed)
    {
        float t = 0f;
        while (t <= 1f)
        {
            t += Time.deltaTime;
            rect.anchoredPosition = Vector3.Lerp(rect.anchoredPosition, target, speed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator MovePlayer(Vector3 target, float speed)
    {
        float t = 0f;
        while (t <= 1f)
        {
            t += Time.deltaTime;
            Player.player.transform.position = Vector3.Lerp(Player.player.transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
    }
    #endregion
}
