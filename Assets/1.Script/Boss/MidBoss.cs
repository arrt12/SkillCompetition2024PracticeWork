using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum Patterns
{
    pattern_1,
    pattern_2,
    pattern_3,
    none
}
public class MidBoss : MonoBehaviour
{
    [SerializeField] Image HpBar;
    private bool isStart = false;
    public float hp = 100;
    private int attackCount;
    public static MidBoss midBoss;
    [SerializeField] Patterns patterns;
    [SerializeField] bool[] isPattern;
    private bool isLook = true;
    private bool isRo = false;
    public bool isAttack = false;
    [SerializeField] GameObject axis;
    [SerializeField] GameObject axis_2;
    [SerializeField] GameObject Laser;
    [SerializeField] GameObject BulletPrefab;
    [SerializeField] GameObject AttackRang;
    [SerializeField] GameObject DeadEfact;
    [SerializeField] Transform SpawnPoint;
    private float laserTime = 0f;
    private bool hitIng = false;
    private bool isDead = false;
    private bool isDeadIng = false;
    private void Awake()
    {
        if (midBoss == null)
            midBoss = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        StartCoroutine(Starting());
    }
    IEnumerator Starting()
    { 
        float t = 0f;
        while (t <= 2f)
        {
            t+= Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, new Vector3(0, 0 ,3), 1.5f* Time.deltaTime);
            yield return null;
        }
        transform.position = new Vector3(0,0,5);
        isStart = true;
    }
    void Update()
    {
        HpBar.fillAmount = hp / 100;
        if (!isStart)
            return;
        switch (patterns)
        {
            case Patterns.pattern_1:
                Pattern1();
                break;
            case Patterns.pattern_2:
                Pattern2();
                break;
            case Patterns.pattern_3:
                Pattern3();
                break;
        }
        Dead();
    }


    void Pattern1()//레이저 발사
    {
        if (isLook)
        {
            LookAt();
        }
        laserTime += Time.deltaTime;
        if (!isPattern[0])
        {
            print("");
            StartCoroutine(Pattering1());
            isPattern[0] = true;
            isPattern[1] = false;
            isPattern[2] = false;
        }
    }

    IEnumerator Pattering1()
    {
        for (int i = 0; i <= 3; i++)
        {
            isLook = false;
            float t = 0;
            while (t <= 0.4f)
            {
                t += Time.deltaTime;
                axis.transform.Rotate(0, 0, 100 * Time.deltaTime);
                yield return null;
            }
            yield return new WaitForSecondsRealtime(0.1f);
            float f = 0;
            while (f <= 0.3f)
            {
                f += Time.deltaTime;
                axis.transform.Rotate(0, 0, -700 * Time.deltaTime);
                yield return null;
            }
            axis.transform.rotation = Quaternion.Euler(0, axis.transform.eulerAngles.y, 0);
            yield return new WaitForSecondsRealtime(0.1f);
            Laser.SetActive(true);
            for (int j = 0; j < 18; j++)
            {
                Instantiate(BulletPrefab, transform.position, Quaternion.Euler(0, j * 20, 0));
            }
            Camera.main.GetComponent<CameraShake>().SetUp(0.1f, 1f);
            yield return new WaitForSecondsRealtime(1f);

            float c = 0f;

            while (c <= 1f)
            {
                c += Time.deltaTime;
                Quaternion targetRotation = Quaternion.LookRotation(Player.player.transform.position - axis.transform.position);
                axis.transform.rotation = Quaternion.Lerp(axis.transform.rotation, targetRotation, 10 * Time.deltaTime);
                yield return null;
            }
            isLook = true;
        }
        yield return new WaitForSecondsRealtime(1f);
        PatternRand();
    }

    void LookAt()
    {
        Quaternion targetRotation = Quaternion.LookRotation(Player.player.transform.position - axis.transform.position);
        axis.transform.rotation = targetRotation;
    }

    void Pattern2()//돌면서 퍼지는 공격
    {
        if (!isPattern[1])
        {
            StartCoroutine(Pattering2());
            isPattern[0] = false;
            isPattern[1] = true;
            isPattern[2] = false;
        }
        if (isRo)
        {
            axis.transform.Rotate(0,0,1000 * Time.deltaTime);
        }
    }


    IEnumerator Pattering2()
    {
        float t = 0f;
        while (t <= 1f)
        {
            t+= Time.deltaTime;
            axis.transform.rotation = Quaternion.Lerp(axis.transform.rotation, Quaternion.Euler(-90, 180 ,0),5 * Time.deltaTime);
            yield return null;
        }
        axis.transform.rotation = Quaternion.Euler(-90, 180, 0);

        isRo = true;
        Camera.main.GetComponent<CameraShake>().SetUp(0.2f, 1f);
        for (int i = 0; i <= 10; i++)
        {
            for (int j = 0; j < 25; j++)
            {
                Instantiate(BulletPrefab, SpawnPoint.position, Quaternion.Euler(SpawnPoint.eulerAngles));
                yield return new WaitForSecondsRealtime(0.02f);
            }
        }
        isRo = false;

        float f = 0f;
        while (f <= 1f)
        {
            f += Time.deltaTime;
            axis.transform.rotation = Quaternion.Lerp(axis.transform.rotation, Quaternion.Euler(0, 180, 0), 5 * Time.deltaTime);
            yield return null;
        }
        axis.transform.rotation = Quaternion.Euler(0, 180, 0);
        yield return new WaitForSecondsRealtime(1f);
        patterns = Patterns.pattern_1;
    }

    void Pattern3()
    {
        if (!isPattern[2])
        {
            StartCoroutine(Pattering3());
            isPattern[0] = false;
            isPattern[1] = false;
            isPattern[2] = true;
        }
        if (isRo)
        {
            axis.transform.Rotate(0, 0, 1000 * Time.deltaTime);
        }
        if (isAttack)
        {
            transform.Translate(0, 0, -100 * Time.deltaTime);
        }
    }

    IEnumerator Pattering3()
    {
        float t = 0f;
        while (t <= 1f)
        {
            t += Time.deltaTime;
            axis.transform.rotation = Quaternion.Lerp(axis.transform.rotation, Quaternion.Euler(90, 180, 0), 5 * Time.deltaTime);
            yield return null;
        }
        axis.transform.rotation = Quaternion.Euler(90, 180, 0);
        isRo = true;
        AttackRang.SetActive(true);
        BossAttackRang.instance.isStart = true;
        yield return new WaitForSecondsRealtime(2f);
        isAttack = false;
        Attack(1, -12);
        yield return new WaitForSecondsRealtime(3f);
        isAttack = false;
        Attack(-1, -3);
        yield return new WaitForSecondsRealtime(3f);
        isAttack = false;
        Attack(1, -3);
        yield return new WaitForSecondsRealtime(3f);
        isAttack = false;
        Attack(-1, -12);
        yield return new WaitForSecondsRealtime(3f);
        isAttack = false;
        yield return new WaitForSecondsRealtime(1f);
        transform.position = new Vector3(0, 0, 40);
        transform.rotation = Quaternion.identity;
        axis.transform.rotation = Quaternion.Euler(0, 180, 0);
        isRo = false;
        float c = 0;
        while (c < 2)
        {
            c += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position,new Vector3(0, 0, 5),2f * Time.deltaTime);
            yield return null;
        }
        transform.position = new Vector3(0, 0, 5);
        yield return new WaitForSecondsRealtime(1f);
        //패턴 끝
        patterns = Patterns.pattern_1;
    }
    void PatternRand()
    {
        int RandP = Random.Range(0,2);
        switch (RandP)
        {
            case 0:
                patterns = Patterns.pattern_2;
                break;
            case 1:
                patterns = Patterns.pattern_3;
                break;
        }
    }

    void Attack(float a, float b)
    {
        transform.rotation = Quaternion.Euler(0, 90 * a, 0);
        transform.position = new Vector3(35 * a, 0, b);
        AttackRang.SetActive(true);
        BossAttackRang.instance.isStart = true;
    }
    void Dead()
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
    IEnumerator Deading()
    {
        axis.SetActive(false);
        DeadEfact.SetActive(true);
        Camera.main.GetComponent<CameraShake>().SetUp(0.1f,2);
        yield return new WaitForSecondsRealtime(0.5f);
        GameManager.gameManager.game = Game.gaming;
        GameManager.gameManager.BossKills++;
        Destroy(gameObject);
    }
    IEnumerator Hit(float hp)
    {
        if (!isDeadIng)
        {
            this.hp -= hp;
            hitIng = true;
            axis_2.SetActive(false);
            yield return new WaitForSecondsRealtime(0.1f);
            axis_2.SetActive(true);
            hitIng = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (hitIng)
            return;
        if (isDeadIng)
            return;
        if (other.gameObject.CompareTag("PlayerBt"))
        {
            StartCoroutine(Hit(0.5f));
        }
    }
}
