using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum EndPatterns
{
    pattern_1,
    pattern_2,
    pattern_3,
    none
}
public class EndBoss : MonoBehaviour
{
    [SerializeField] Image HpBar;
    public float hp = 100;
    [SerializeField] float speed;
    public static EndBoss endBoss;
    [SerializeField] EndPatterns endPatterns;
    [SerializeField] bool IsTwoAttack;
    [SerializeField] bool IsRo;
    [SerializeField] bool IsMove;
    [SerializeField] bool[] isPattern;
    [SerializeField] GameObject[] TwogameObjects;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject bullet2;
    [SerializeField] GameObject bullet3;
    [SerializeField] GameObject Gun;
    [SerializeField] GameObject GunSpawn;
    [SerializeField] GameObject Spawn;
    [SerializeField] GameObject axis;
    [SerializeField] GameObject DeadEfact;
    float Attacktime = 0f;
    private bool hitIng = false;
    private bool isDead = false;
    private bool isDeadIng = false;
    private bool isStart = false;

    private void Awake()
    {
        if (endBoss == null)
                endBoss = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        IsTwoAttack = true;
        StartCoroutine(Starting());
    }

    IEnumerator Starting()
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
    void Update()
    {
        HpBar.fillAmount = hp / 100;
        if (endPatterns == EndPatterns.none)
            return;
        if (!isStart)
            return;
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
        Dead();
    }
    void Pattern_1()
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
    IEnumerator Pattering1()//양옆으로 움직이면서 게틀린건 발싸
    {
        yield return new WaitForSecondsRealtime(2f);
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                IsRo = true;
                Instantiate(bullet2,GunSpawn.transform.position,Quaternion.Euler(GunSpawn.transform.eulerAngles));
                Camera.main.GetComponent<CameraShake>().SetUp(0.1f, 0.2f);
                yield return new WaitForSecondsRealtime(0.1f);
            }
            IsRo = false;
            yield return new WaitForSecondsRealtime(1f);
        }
        yield return new WaitForSecondsRealtime(1f);
        IsMove = false;
        float t = 0f;
        while (t <= 0.7f)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, new Vector3(0, 0, 8f), 5f * Time.deltaTime);
            yield return null;
        }
        transform.position = new Vector3(0, 0, 8f);
        float c = 0f;
        while (c <= 1)
        {
            c += Time.deltaTime; 
            foreach (var item in TwogameObjects)
            {
                item.transform.rotation = Quaternion.Lerp(item.transform.rotation, Quaternion.identity, 2 * Time.deltaTime);
            }
        }
        foreach (var item in TwogameObjects)
        {
            item.transform.rotation = Quaternion.identity;
        }
        endPatterns = EndPatterns.pattern_2;
    }
    void Pattern_2()
    {
        if (!isPattern[1])
        {
            StartCoroutine(Pattering2());
            isPattern[1] = true;
            isPattern[0] = false;
            isPattern[2] = false;
        }
    }
    IEnumerator Pattering2()
    {
        yield return new WaitForSecondsRealtime(1f);
        for (int i = 0; i < 10; i++)
        {
            float t = 0f;
            while (t <= 0.1f)
            {
                t += Time.deltaTime;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(40, 0, 0), 80f * Time.deltaTime);
                yield return null;
            }
            transform.rotation = Quaternion.Euler(40, 0, 0);
            yield return new WaitForSecondsRealtime(0.1f);
            float f = 0f;
            while (f <= 0.1f)
            {
                f += Time.deltaTime;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, 80f * Time.deltaTime);
                yield return null;
            }
            transform.rotation = Quaternion.identity;
            for (int e = 0; e < 18; e++)
            {
                Instantiate(bullet, GunSpawn.transform.position, Quaternion.Euler(0,e * 20,0));
            }
            Camera.main.GetComponent<CameraShake>().SetUp(0.1f,0.7f);
            yield return new WaitForSecondsRealtime(0.1f);
        }
        yield return new WaitForSecondsRealtime(1f);
        PatternRand();
    }
    void Pattern_3()
    {
        if (!isPattern[2])
        {
            StartCoroutine(Pattering3());
            isPattern[2] = true;
            isPattern[1] = false;
            isPattern[0] = false;
        }
    }
    IEnumerator Pattering3()
    {
        yield return new WaitForSecondsRealtime(1f);
        float c = 0f;
        float Rand = Random.Range(-20,20f);
        while (c <= 1)
        {
            c += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, new Vector3(Rand,transform.position.y,transform.position.z), 5 * Time.deltaTime);
            yield return null;
        }
        transform.position = new Vector3(Rand, transform.position.y, transform.position.z);

        float t = 0f;
        while (t <= 1)
        {
            t += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 40, 0), 5f * Time.deltaTime);
            yield return null;
        }
        transform.rotation = Quaternion.Euler(0, 40 ,0);
        float f = 0f;
        while (f <= 0.5f)
        {
            f += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, -150, 0), 30f * Time.deltaTime);
            yield return null;
        }
        Instantiate(bullet3, Spawn.transform.position, Quaternion.Euler(Spawn.transform.eulerAngles));
        for (int i = 0; i < 18; i++)
        {
            Instantiate(bullet2, Spawn.transform.position, Quaternion.Euler(0, i * 20, 0));
        }
        Camera.main.GetComponent<CameraShake>().SetUp(0.1f, 2f);
        transform.rotation = Quaternion.Euler(0, -150, 0);
        yield return new WaitForSecondsRealtime(1f);
        
        float e = 0f;
        while (e <= 1)
        {
            e += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, 5f * Time.deltaTime);
            yield return null;
        }
        transform.rotation = Quaternion.identity;

        float d = 0f;
        while (d <= 1)
        {
            d += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, new Vector3(0,0,8), 5 * Time.deltaTime);
            yield return null;
        }
        transform.position = new Vector3(0, 0, 8);
        endPatterns = EndPatterns.pattern_2;
    }
    void TwoAttack()
    {
        if (IsTwoAttack != true)
            return;
        LookAt(TwogameObjects[0]);
        LookAt(TwogameObjects[1]); 
        Attacktime += Time.deltaTime;
        if (Attacktime >= 1)
        {
            foreach (var item in TwogameObjects)
            {
                Instantiate(bullet,item.transform.position,Quaternion.Euler(item.transform.eulerAngles));
            }
            Attacktime = 0;
        }
    }

    void LookAt(GameObject axis)
    {
        Quaternion targetRotation = Quaternion.LookRotation(Player.player.transform.position - axis.transform.position);
        axis.transform.rotation = targetRotation;
    }

    void Move()
    {
        transform.Translate(speed* Time.deltaTime,0,0);
        var viewPos = Camera.main.WorldToViewportPoint(transform.position);
        if (viewPos.x >= 0.6f) speed *= -1f;
        if (viewPos.x <= 0.4f) speed *= -1f;
    }

    void PatternRand()
    {
        int RandP = Random.Range(0, 3);
        switch (RandP)
        {
            case 0:
                endPatterns = EndPatterns.pattern_1;
                break;
            case 1:
                endPatterns = EndPatterns.pattern_3;
                break;
            case 2:
                endPatterns = EndPatterns.pattern_3;
                break;
        }
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
        Camera.main.GetComponent<CameraShake>().SetUp(0.1f, 2);
        yield return new WaitForSecondsRealtime(0.5f);
        GameManager.gameManager.game = Game.end;
        GameManager.gameManager.BossKills++;
        Destroy(gameObject);
    }

    IEnumerator Hit(float hp)
    {
        if (!isDeadIng)
        {
            this.hp -= hp;
            hitIng = true;
            axis.SetActive(false);
            yield return new WaitForSecondsRealtime(0.1f);
            axis.SetActive(true);
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
            StartCoroutine(Hit(0.1f));
    }
}
