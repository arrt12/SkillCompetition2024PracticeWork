using System.Collections;
using UnityEngine;

/// <summary>
/// �÷��̾� ���� �� ���� ����
/// </summary>
public class Player : MonoBehaviour
{
    #region Singleton
    public static Player player;

    private void Awake()
    {
        if (player == null) player = this;
        else Destroy(gameObject);

        rb = GetComponent<Rigidbody>();
    }
    #endregion

    #region ���� �� ����
    [Header("����")]
    public int LevelCount = 0;
    public float hp = 100, fuel = 1000;
    private int deadCount = 0;

    [Header("����")]
    [SerializeField] private float speed = 5f, limitXAngle = 30f, attackCooldown = 0.1f;
    [SerializeField] private GameObject bulletPrefab, axis, invincibilityEffect;

    private Rigidbody rb;
    private Vector3 moveInput;
    private float attackTimer, rotateSmoothDamp;
    private Coroutine invincibilityCoroutine;
    private bool isHit = false;
    public bool isInvincible = false, triggerInvincibility = false;

    private const float DAMAGE_NORMAL = 10f, DAMAGE_STRONG = 20f, DAMAGE_BOSS = 30f;
    #endregion

    #region Unity Life Cycle
    private void Update()
    {
        if (GameManager.gameManager.game == Game.start) return;
        ResetRigidbody();

        if (triggerInvincibility)
        {
            if (invincibilityCoroutine != null) StopCoroutine(invincibilityCoroutine);
            invincibilityCoroutine = StartCoroutine(HandleInvincibility());
            triggerInvincibility = false;
        }

        if (!GameManager.gameManager.isGameEnd)
        {
            HandleInput();
            HandleAttack();
        }

        CheckDead();
    }

    private void FixedUpdate()
    {
        if (GameManager.gameManager.isGameEnd)
        {
            axis.transform.rotation = Quaternion.identity;
            return;
        }

        Move();
        Rotate(moveInput.x);
    }
    #endregion

    #region �Է� ó��
    private void HandleInput()
    {
        moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        ClampPosition();
    }
    #endregion

    #region ���� ó��
    private void HandleAttack()
    {
        if ((attackTimer -= Time.deltaTime) > 0) return;
        if (!Input.GetKey(KeyCode.X)) return;

        Vector3 pos = transform.position + Vector3.forward * 2;

        for (int i = -LevelCount; i <= LevelCount; i++)
        {
            float offset = 0.5f * i;
            float angle = 10 * Mathf.Sign(i) * (Mathf.Abs(i) > 0 ? 1 : 0);
            SpawnBullet(pos + Vector3.right * offset, angle);
        }

        attackTimer = attackCooldown;
    }

    private void SpawnBullet(Vector3 pos, float angleZ)
    {
        Instantiate(bulletPrefab, pos, Quaternion.Euler(0, 0, 180 + angleZ));
    }
    #endregion

    #region �ǰ� �� ���� ó��
    private IEnumerator HandleHit(float damage)
    {
        hp -= damage;
        isHit = true;
        Camera.main.GetComponent<CameraShake>().SetUp(0.1f, 1f);

        for (int i = 0; i < 7; i++)
        {
            axis.SetActive(!axis.activeSelf);
            yield return new WaitForSecondsRealtime(0.1f);
        }

        axis.SetActive(true);
        isHit = false;
    }

    private IEnumerator HandleInvincibility()
    {
        isInvincible = true;

        for (int i = 0; i < 40; i++)
        {
            invincibilityEffect.SetActive(!invincibilityEffect.activeSelf);
            yield return new WaitForSecondsRealtime(0.05f);
        }

        invincibilityEffect.SetActive(false);
        isInvincible = false;
    }
    #endregion

    #region ��� ó��
    private void CheckDead()
    {
        if ((hp <= 0 || fuel <= 0) && deadCount == 0)
            GameOver();
    }

    private void GameOver()
    {
        deadCount++;

        Camera.main.GetComponent<CameraShake>().SetUp(0.2f, 2f);

        GameManager.gameManager.isGameEnd = true;
        GameManager.gameManager.isDead = true;
        GameManager.gameManager.CleanupEntities(); // ���� ������Ʈ ����

        GameManager.gameManager.game = Game.Dead;
    }
    #endregion

    #region �浹 ó��
    private void OnTriggerEnter(Collider other)
    {
        if (isHit || isInvincible) return;

        float damage = other.tag switch
        {
            "MosterBt" => DAMAGE_NORMAL,
            "Moster" => DAMAGE_NORMAL,
            "Mt" => DAMAGE_STRONG,
            "Boss" or "Pt3" or "Laser" => DAMAGE_BOSS,
            _ => 0
        };

        if (damage > 0) StartCoroutine(HandleHit(damage));
        if (other.tag == "MosterBt") Destroy(other.gameObject);
    }
    #endregion

    #region �̵� �� ȸ��
    private void Move()
    {
        transform.position += moveInput * speed * Time.deltaTime;
    }

    private void Rotate(float xInput)
    {
        float targetZ = xInput * limitXAngle;
        targetZ = Mathf.SmoothDampAngle(axis.transform.eulerAngles.z, targetZ, ref rotateSmoothDamp, 0.1f);
        axis.transform.rotation = Quaternion.Euler(0, 0, targetZ);
    }

    private void ClampPosition()
    {
        var viewPos = Camera.main.WorldToViewportPoint(transform.position);
        viewPos.x = Mathf.Clamp(viewPos.x, 0.05f, 0.95f);
        viewPos.y = Mathf.Clamp(viewPos.y, 0.05f, 0.95f);
        var clamped = Camera.main.ViewportToWorldPoint(viewPos);
        transform.position = new Vector3(clamped.x, transform.position.y, clamped.z);
    }

    private void ResetRigidbody()
    {
        rb.velocity = Vector3.zero;
        rb.rotation = Quaternion.identity;
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }
    #endregion
}
