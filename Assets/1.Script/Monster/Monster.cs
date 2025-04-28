using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Monsters
{
    monster1,  // 앞으로 이동하면서 플레이어 방향으로 공격
    monster2,  // 자폭하는 몬스터
    monster3,  // 여러 마리 무리지어 다니는 몬스터
    meteor     // 별똥별 타입
}

public class Monster : MonoBehaviour
{
    #region 변수
    private int count = 0;
    private bool isMeteor = true;
    private bool hitIng = false;
    private bool isDead = false;
    private bool isDeadIng = false;
    private float attackTime = 0f;

    [Header("설정")]
    public float hp = 100f;
    [SerializeField] private float speed = 3f;
    [SerializeField] private Monsters monsters;

    [Header("오브젝트")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject axis;
    [SerializeField] private GameObject attackRang;
    [SerializeField] private GameObject deadEffect;
    [SerializeField] private GameObject[] items;
    [SerializeField] private Transform bulletSpawn;
    #endregion

    #region Unity 생명주기
    private void Start()
    {
        if (monsters == Monsters.meteor)
        {
            Quaternion targetRotation = Quaternion.LookRotation(Player.player.transform.position - axis.transform.position);
            transform.rotation = targetRotation;
        }
    }

    private void Update()
    {
        if (isDead) return;

        MonsterBehavior();
        Dead();
        Destroy(gameObject, 15f); // 15초 후 삭제
    }
    #endregion

    #region 몬스터 동작
    private void MonsterBehavior()
    {
        switch (monsters)
        {
            case Monsters.monster1:
                Monster1();
                MonsterMove();
                LookAt();
                break;

            case Monsters.monster2:
                Monster2();
                Rotate(Vector3.up * 500f * Time.deltaTime);
                MonsterMove();
                break;

            case Monsters.monster3:
                Monster3();
                LookAt();
                break;

            case Monsters.meteor:
                Meteor();
                Rotate(Vector3.right * 300f * Time.deltaTime);
                break;
        }
    }

    private void Monster1()
    {
        attackTime += Time.deltaTime;
        if (attackTime >= 1f)
        {
            Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.Euler(axis.transform.eulerAngles));
            attackTime = 0f;
        }
    }

    private void Monster2()
    {
        attackTime += Time.deltaTime;
        if (attackTime >= 1.5f)
        {
            for (int i = 0; i < 10; i++)
            {
                Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, i * 36, 0));
            }
            attackTime = 0f;
        }
    }

    private void Monster3() => MonsterMove();

    private void Meteor()
    {
        if (isMeteor)
        {
            StartCoroutine(MeteorAttack());
            isMeteor = false;
        }
    }
    #endregion

    #region 이동 및 회전
    private void MonsterMove() => transform.Translate(0, 0, -speed * Time.deltaTime);

    private void Rotate(Vector3 vector) => axis.transform.Rotate(vector);

    private void LookAt()
    {
        Quaternion targetRotation = Quaternion.LookRotation(Player.player.transform.position - axis.transform.position);
        axis.transform.rotation = targetRotation;
    }
    #endregion

    #region 데미지 및 죽음
    private void Dead()
    {
        if (hp > 0) return;

        isDead = true;

        if (count <= 0)
        {
            StartCoroutine(Deading());
            isDead = false;
            isDeadIng = true;
            count++;
        }
    }

    private IEnumerator Deading()
    {
        GameManager.gameManager.kills++;
        yield return null;

        axis.SetActive(false);
        Instantiate(deadEffect, transform.position, Quaternion.identity);
        RandItem();
        Destroy(gameObject);
    }

    private IEnumerator Hit(float damage)
    {
        if (!isDeadIng)
        {
            hp -= damage;
            hitIng = true;
            axis.SetActive(false);

            yield return new WaitForSecondsRealtime(0.1f);

            axis.SetActive(true);
            hitIng = false;
        }
    }
    #endregion

    #region 기타
    private IEnumerator MeteorAttack()
    {
        attackRang.SetActive(true);
        for (int i = 0; i < 10; i++)
        {
            attackRang.SetActive(true);
            yield return new WaitForSecondsRealtime(0.1f);
            attackRang.SetActive(false);
            yield return new WaitForSecondsRealtime(0.1f);
        }
        axis.SetActive(true);

        float t = 0f;
        while (t < 10f)
        {
            t += Time.deltaTime;
            MonsterMove();
            yield return null;
        }
    }

    private void RandItem()
    {
        if (monsters == Monsters.meteor) return;

        int randValue = Random.Range(0, 50);

        switch (randValue)
        {
            case 0:
                Instantiate(items[0], transform.position, Quaternion.identity);
                break;
            case 10:
                Instantiate(items[1], transform.position, Quaternion.identity);
                break;
            case 20:
                Instantiate(items[2], transform.position, Quaternion.identity);
                break;
            case 40:
                Instantiate(items[3], transform.position, Quaternion.identity);
                break;
        }
    }
    #endregion

    #region 충돌 처리
    private void OnTriggerEnter(Collider other)
    {
        if (hitIng || isDeadIng || monsters == Monsters.meteor)
            return;

        if (other.CompareTag("PlayerBt"))
            StartCoroutine(Hit(30f));
    }
    #endregion
}
