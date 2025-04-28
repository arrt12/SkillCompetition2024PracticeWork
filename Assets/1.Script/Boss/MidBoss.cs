using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#region Enum
public enum Patterns
{
    Pattern1,
    Pattern2,
    Pattern3,
    None
}
#endregion

public class MidBoss : MonoBehaviour
{
    #region Variables

    [Header("UI")]
    [SerializeField] private Image hpBar;

    [Header("Pattern Settings")]
    [SerializeField] private Patterns currentPattern = Patterns.None;
    [SerializeField] private bool[] isPatternActive;

    [Header("Boss Components")]
    [SerializeField] private GameObject axis;
    [SerializeField] private GameObject axis_2;
    [SerializeField] private GameObject laser;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject attackRange;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private Transform spawnPoint;

    private bool isStart = false;
    private bool isLooking = true;
    private bool isRotating = false;
    public bool isAttacking = false;

    private bool isDead = false;
    private bool isDeadProcessStarted = false;
    private bool isHitProcessing = false;

    private float laserTime = 0f;
    public float hp = 100f;

    public static MidBoss Instance;

    #endregion

    #region Unity Functions

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        StartCoroutine(StartRoutine());
    }

    private void Update()
    {
        hpBar.fillAmount = hp / 100f;

        if (!isStart) return;

        switch (currentPattern)
        {
            case Patterns.Pattern1:
                HandlePattern1();
                break;
            case Patterns.Pattern2:
                HandlePattern2();
                break;
            case Patterns.Pattern3:
                HandlePattern3();
                break;
        }

        CheckDead();
    }

    #endregion

    #region Start Routine

    private IEnumerator StartRoutine()
    {
        float timer = 0f;

        while (timer <= 2f)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, new Vector3(0, 0, 3), 1.5f * Time.deltaTime);
            yield return null;
        }

        transform.position = new Vector3(0, 0, 5);
        isStart = true;
    }

    #endregion

    #region Pattern 1 - Laser Attack

    private void HandlePattern1()
    {
        if (isLooking)
            LookAtPlayer();

        laserTime += Time.deltaTime;

        if (!isPatternActive[0])
        {
            StartCoroutine(Pattern1Routine());
            SetPatternActive(0);
        }
    }

    private IEnumerator Pattern1Routine()
    {
        for (int i = 0; i <= 3; i++)
        {
            isLooking = false;

            yield return RotateAxis(100f, 0.4f);
            yield return new WaitForSecondsRealtime(0.1f);

            yield return RotateAxis(-700f, 0.3f);
            axis.transform.rotation = Quaternion.Euler(0, axis.transform.eulerAngles.y, 0);

            yield return new WaitForSecondsRealtime(0.1f);

            laser.SetActive(true);
            FireRadialBullets(18, 20);

            Camera.main.GetComponent<CameraShake>().SetUp(0.1f, 1f);

            yield return new WaitForSecondsRealtime(1f);

            yield return RotateToPlayer(1f);

            isLooking = true;
        }

        yield return new WaitForSecondsRealtime(1f);

        RandomizeNextPattern();
    }

    private void LookAtPlayer()
    {
        Quaternion targetRotation = Quaternion.LookRotation(Player.player.transform.position - axis.transform.position);
        axis.transform.rotation = targetRotation;
    }

    private void FireRadialBullets(int bulletCount, float angleStep)
    {
        for (int i = 0; i < bulletCount; i++)
        {
            Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, i * angleStep, 0));
        }
    }

    private IEnumerator RotateAxis(float speed, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            axis.transform.Rotate(0, 0, speed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator RotateToPlayer(float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            Quaternion targetRotation = Quaternion.LookRotation(Player.player.transform.position - axis.transform.position);
            axis.transform.rotation = Quaternion.Lerp(axis.transform.rotation, targetRotation, 10 * Time.deltaTime);
            yield return null;
        }
    }

    #endregion

    #region Pattern 2 - Rotating Bullet Spray

    private void HandlePattern2()
    {
        if (!isPatternActive[1])
        {
            StartCoroutine(Pattern2Routine());
            SetPatternActive(1);
        }

        if (isRotating)
        {
            axis.transform.Rotate(0, 0, 1000f * Time.deltaTime);
        }
    }

    private IEnumerator Pattern2Routine()
    {
        yield return RotateAxisTo(new Vector3(-90, 180, 0), 1f);

        isRotating = true;
        Camera.main.GetComponent<CameraShake>().SetUp(0.2f, 1f);

        for (int i = 0; i <= 10; i++)
        {
            for (int j = 0; j < 25; j++)
            {
                Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
                yield return new WaitForSecondsRealtime(0.02f);
            }
        }

        isRotating = false;

        yield return RotateAxisTo(new Vector3(0, 180, 0), 1f);

        yield return new WaitForSecondsRealtime(1f);

        currentPattern = Patterns.Pattern1;
    }

    private IEnumerator RotateAxisTo(Vector3 targetEulerAngles, float duration)
    {
        float elapsed = 0f;
        Quaternion startRotation = axis.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(targetEulerAngles);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            axis.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsed / duration);
            yield return null;
        }

        axis.transform.rotation = targetRotation;
    }

    #endregion

    #region Pattern 3 - Charge Attack

    private void HandlePattern3()
    {
        if (!isPatternActive[2])
        {
            StartCoroutine(Pattern3Routine());
            SetPatternActive(2);
        }

        if (isRotating)
            axis.transform.Rotate(0, 0, 1000f * Time.deltaTime);

        if (isAttacking)
            transform.Translate(0, 0, -100f * Time.deltaTime);
    }

    private IEnumerator Pattern3Routine()
    {
        yield return RotateAxisTo(new Vector3(90, 180, 0), 1f);

        isRotating = true;
        attackRange.SetActive(true);
        BossAttackRang.instance.isStart = true;

        yield return new WaitForSecondsRealtime(2f);

        yield return ExecuteChargeAttack(1, -12);
        yield return ExecuteChargeAttack(-1, -3);
        yield return ExecuteChargeAttack(1, -3);
        yield return ExecuteChargeAttack(-1, -12);

        ResetBossPosition();
        yield return MoveBossBack(2f);

        yield return new WaitForSecondsRealtime(1f);

        currentPattern = Patterns.Pattern1;
    }

    private IEnumerator ExecuteChargeAttack(float direction, float zPosition)
    {
        isAttacking = false;
        SetupCharge(direction, zPosition);
        yield return new WaitForSecondsRealtime(3f);
        isAttacking = false;
    }

    private void SetupCharge(float direction, float zPosition)
    {
        transform.rotation = Quaternion.Euler(0, 90f * direction, 0);
        transform.position = new Vector3(35f * direction, 0, zPosition);
        attackRange.SetActive(true);
        BossAttackRang.instance.isStart = true;
    }

    private void ResetBossPosition()
    {
        transform.position = new Vector3(0, 0, 40);
        transform.rotation = Quaternion.identity;
        axis.transform.rotation = Quaternion.Euler(0, 180, 0);
        isRotating = false;
    }

    private IEnumerator MoveBossBack(float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, new Vector3(0, 0, 5), 2f * Time.deltaTime);
            yield return null;
        }

        transform.position = new Vector3(0, 0, 5);
    }

    #endregion

    #region Death Handling

    private void CheckDead()
    {
        if (hp <= 0)
        {
            isDead = true;
            isDeadProcessStarted = true;
        }

        if (isDead)
        {
            StartCoroutine(DeathRoutine());
            isDead = false;
        }
    }

    private IEnumerator DeathRoutine()
    {
        axis.SetActive(false);
        deathEffect.SetActive(true);
        Camera.main.GetComponent<CameraShake>().SetUp(0.1f, 2f);

        yield return new WaitForSecondsRealtime(0.5f);

        GameManager.gameManager.game = Game.gaming;
        GameManager.gameManager.BossKills++;
        Destroy(gameObject);
    }

    #endregion

    #region Hit Handling

    private void OnTriggerEnter(Collider other)
    {
        if (isHitProcessing || isDeadProcessStarted)
            return;

        if (other.CompareTag("PlayerBt"))
            StartCoroutine(HitRoutine(0.5f));
    }

    private IEnumerator HitRoutine(float damage)
    {
        if (!isDeadProcessStarted)
        {
            hp -= damage;
            isHitProcessing = true;
            axis_2.SetActive(false);

            yield return new WaitForSecondsRealtime(0.1f);

            axis_2.SetActive(true);
            isHitProcessing = false;
        }
    }

    #endregion

    #region Utility

    private void SetPatternActive(int index)
    {
        for (int i = 0; i < isPatternActive.Length; i++)
            isPatternActive[i] = i == index;
    }

    private void RandomizeNextPattern()
    {
        currentPattern = (Patterns)Random.Range(1, 3);
    }

    #endregion
}
