using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    #region 변수
    public static Bomb bomb;

    [Header("컴포넌트")]
    public Rigidbody rig;
    [SerializeField] private GameObject axis;
    [SerializeField] private GameObject boomEffect;
    [SerializeField] private GameObject boomEffect2;

    [Header("설정")]
    [SerializeField] private float speed = 5f;

    private bool isAttacking = true;
    #endregion

    #region Unity Life Cycle
    private void Awake()
    {
        if (bomb == null)
            bomb = this;
        else
            Destroy(gameObject);

        rig = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        HandleBoom();
    }
    #endregion

    #region 폭발 관련
    private void HandleBoom()
    {
        axis.transform.Rotate(0, 0, 500f * Time.deltaTime);

        if (isAttacking)
        {
            StartCoroutine(AttackRoutine());
            isAttacking = false;
        }
    }

    private IEnumerator AttackRoutine()
    {
        yield return new WaitForSecondsRealtime(0.1f);

        float timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime;
            rig.useGravity = false;
            Move();
            yield return null;
        }

        rig.velocity = Vector3.zero;
        axis.SetActive(false);
        boomEffect.SetActive(true);

        yield return new WaitForSecondsRealtime(0.2f);

        Instantiate(boomEffect2, transform.position, Quaternion.identity);

        KillMonsters();
        DestroyEnemyBullets();

        Camera.main.GetComponent<CameraShake>().SetUp(0.2f, 1.5f);

        yield return new WaitForSecondsRealtime(1f);

        Destroy(gameObject);
    }

    private void Move()
    {
        Vector3 targetPos = transform.position + Vector3.forward * 5f;
        Quaternion targetRotation = Quaternion.LookRotation(targetPos - transform.position);
        axis.transform.rotation = Quaternion.Euler(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, axis.transform.eulerAngles.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
    }

    private void KillMonsters()
    {
        Monster[] monsters = FindObjectsOfType<Monster>();
        foreach (var monster in monsters)
        {
            monster.hp = 0;
        }
    }

    private void DestroyEnemyBullets()
    {
        Bullet[] bullets = FindObjectsOfType<Bullet>();
        foreach (var bullet in bullets)
        {
            if (bullet.bullets == Bullets.player)
                continue;

            Destroy(bullet.gameObject);
        }
    }
    #endregion
}
