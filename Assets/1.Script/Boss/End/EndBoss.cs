using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#region Class: EndBoss
/// <summary>
/// 최종 보스 로직 관리
/// </summary>
public class EndBoss : MonoBehaviour
{
    #region Serialized Fields
    [Header("UI Elements")]
    [SerializeField] private Image HpBar;

    [Header("Boss Settings")]
    public float hp = 100;
    [SerializeField] private float speed;
    public static EndBoss endBoss;
    [SerializeField] private EndPatterns endPatterns;
    [SerializeField] private bool IsTwoAttack;
    [SerializeField] private bool IsRo;
    [SerializeField] private bool IsMove;
    [SerializeField] private bool[] isPattern;

    [Header("Game Objects")]
    [SerializeField] private GameObject[] TwogameObjects;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject bullet2;
    [SerializeField] private GameObject bullet3;
    [SerializeField] private GameObject Gun;
    [SerializeField] private GameObject GunSpawn;
    [SerializeField] private GameObject Spawn;
    [SerializeField] private GameObject axis;
    [SerializeField] private GameObject DeadEfact;
    #endregion

    #region Private Variables
    private float Attacktime = 0f;
    private bool hitIng = false;
    private bool isDead = false;
    private bool isDeadIng = false;
    private bool isStart = false;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        if (endBoss == null)
            endBoss = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        IsTwoAttack = true;
        StartCoroutine(Starting());
    }

    private void Update()
    {
        UpdateHpBar();

        if (!isStart || endPatterns == EndPatterns.none)
            return;

        HandlePatterns();
        HandleDead();
    }
    #endregion

    #region UI Methods
    private void UpdateHpBar()
    {
        HpBar.fillAmount = hp / 100f;
    }
    #endregion

    #region Pattern Handling
    private void HandlePatterns()
    {
        switch (endPatterns)
        {
            case EndPatterns.pattern_1:
                Pattern_1();
                TwoAttack();
                break;
            case EndPatterns.pattern_2:
                Pattern_2();
                break;
            case EndPatterns.pattern_3:
                Pattern_3();
                break;
        }
    }
    #endregion

    #region Pattern 1
    private void Pattern_1()
    {
        if (!isPattern[0])
        {
            StartCoroutine(Pattering1());
            IsMove = true;
            isPattern[0] = true;
            isPattern[1] = false;
            isPattern[2] = false;
        }

        if (IsRo)
            Gun.transform.Rotate(0, 0, 700 * Time.deltaTime);

        if (IsMove)
            Move();
    }

    private IEnumerator Pattering1()
    {
        yield return new WaitForSecondsRealtime(2f);

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                IsRo = true;
                Instantiate(bullet2, GunSpawn.transform.position, Quaternion.Euler(GunSpawn.transform.eulerAngles));
                Camera.main.GetComponent<CameraShake>().SetUp(0.1f, 0.2f);
                yield return new WaitForSecondsRealtime(0.1f);
            }
            IsRo = false;
            yield return new WaitForSecondsRealtime(1f);
        }

        IsMove = false;
        yield return MoveToPosition(new Vector3(0, 0, 8f), 0.7f);

        yield return ResetTwinCannons();

        endPatterns = EndPatterns.pattern_2;
    }
    #endregion

    #region Pattern 2
    private void Pattern_2()
    {
        if (!isPattern[1])
        {
            StartCoroutine(Pattering2());
            isPattern[1] = true;
            isPattern[0] = false;
            isPattern[2] = false;
        }
    }

    private IEnumerator Pattering2()
    {
        yield return new WaitForSecondsRealtime(1f);

        for (int i = 0; i < 10; i++)
        {
            yield return RotateOverTime(Quaternion.Euler(40, 0, 0), 0.1f);
            yield return RotateOverTime(Quaternion.identity, 0.1f);

            for (int e = 0; e < 18; e++)
            {
                Instantiate(bullet, GunSpawn.transform.position, Quaternion.Euler(0, e * 20, 0));
            }

            Camera.main.GetComponent<CameraShake>().SetUp(0.1f, 0.7f);
            yield return new WaitForSecondsRealtime(0.1f);
        }

        yield return new WaitForSecondsRealtime(1f);
        PatternRand();
    }
    #endregion

    #region Pattern 3
    private void Pattern_3()
    {
        if (!isPattern[2])
        {
            StartCoroutine(Pattering3());
            isPattern[2] = true;
            isPattern[0] = false;
            isPattern[1] = false;
        }
    }

    private IEnumerator Pattering3()
    {
        yield return new WaitForSecondsRealtime(1f);

        float Rand = Random.Range(-20f, 20f);
        yield return MoveToPosition(new Vector3(Rand, transform.position.y, transform.position.z), 1f);

        yield return RotateOverTime(Quaternion.Euler(0, 40, 0), 1f);
        yield return RotateOverTime(Quaternion.Euler(0, -150, 0), 0.5f);

        Instantiate(bullet3, Spawn.transform.position, Quaternion.Euler(Spawn.transform.eulerAngles));

        for (int i = 0; i < 18; i++)
        {
            Instantiate(bullet2, Spawn.transform.position, Quaternion.Euler(0, i * 20, 0));
        }

        Camera.main.GetComponent<CameraShake>().SetUp(0.1f, 2f);

        yield return RotateOverTime(Quaternion.identity, 1f);
        yield return MoveToPosition(new Vector3(0, 0, 8), 1f);

        endPatterns = EndPatterns.pattern_2;
    }
    #endregion

    #region Attack Logic
    private void TwoAttack()
    {
        if (!IsTwoAttack)
            return;

        LookAt(TwogameObjects[0]);
        LookAt(TwogameObjects[1]);

        Attacktime += Time.deltaTime;
        if (Attacktime >= 1f)
        {
            foreach (var obj in TwogameObjects)
            {
                Instantiate(bullet, obj.transform.position, Quaternion.Euler(obj.transform.eulerAngles));
            }
            Attacktime = 0f;
        }
    }

    private void LookAt(GameObject axis)
    {
        Quaternion targetRotation = Quaternion.LookRotation(Player.player.transform.position - axis.transform.position);
        axis.transform.rotation = targetRotation;
    }
    #endregion

    #region Movement Logic
    private void Move()
    {
        transform.Translate(speed * Time.deltaTime, 0, 0);
        var viewPos = Camera.main.WorldToViewportPoint(transform.position);

        if (viewPos.x >= 0.6f || viewPos.x <= 0.4f)
            speed *= -1f;
    }

    private IEnumerator MoveToPosition(Vector3 target, float duration)
    {
        float t = 0f;
        Vector3 startPos = transform.position;

        while (t < duration)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, target, t / duration);
            yield return null;
        }
        transform.position = target;
    }

    private IEnumerator RotateOverTime(Quaternion targetRotation, float duration)
    {
        float t = 0f;
        Quaternion startRotation = transform.rotation;

        while (t < duration)
        {
            t += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t / duration);
            yield return null;
        }
        transform.rotation = targetRotation;
    }

    private IEnumerator ResetTwinCannons()
    {
        float c = 0f;
        while (c <= 1f)
        {
            c += Time.deltaTime;
            foreach (var obj in TwogameObjects)
            {
                obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, Quaternion.identity, 2f * Time.deltaTime);
            }
            yield return null;
        }

        foreach (var obj in TwogameObjects)
        {
            obj.transform.rotation = Quaternion.identity;
        }
    }
    #endregion

    #region Dead Logic
    private void HandleDead()
    {
        if (hp <= 0)
        {
            isDead = true;
            isDeadIng = true;
        }

        if (isDead)
        {
            StartCoroutine(Deading());
            isDead = false;
        }
    }

    private IEnumerator Deading()
    {
        axis.SetActive(false);
        DeadEfact.SetActive(true);
        Camera.main.GetComponent<CameraShake>().SetUp(0.1f, 2f);
        yield return new WaitForSecondsRealtime(0.5f);

        GameManager.gameManager.game = Game.end;
        GameManager.gameManager.BossKills++;

        Destroy(gameObject);
    }
    #endregion

    #region Damage Logic
    private IEnumerator Hit(float damage)
    {
        if (!isDeadIng)
        {
            this.hp -= damage;
            hitIng = true;
            axis.SetActive(false);

            yield return new WaitForSecondsRealtime(0.1f);

            axis.SetActive(true);
            hitIng = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hitIng || isDeadIng)
            return;

        if (other.CompareTag("PlayerBt"))
            StartCoroutine(Hit(0.1f));
    }
    #endregion

    #region Pattern Random
    private void PatternRand()
    {
        int rand = Random.Range(0, 3);
        switch (rand)
        {
            case 0:
                endPatterns = EndPatterns.pattern_1;
                break;
            case 1:
            case 2:
                endPatterns = EndPatterns.pattern_3;
                break;
        }
    }
    #endregion

    #region Start Animation
    private IEnumerator Starting()
    {
        float t = 0f;
        while (t <= 2f)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, new Vector3(0, 0, 6), 1.5f * Time.deltaTime);
            yield return null;
        }
        transform.position = new Vector3(0, 0, 8);
        isStart = true;
    }
    #endregion
}
#endregion

public enum EndPatterns
{
    pattern_1,
    pattern_2,
    pattern_3,
    none
}
