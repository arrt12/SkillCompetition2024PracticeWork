using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackRang : MonoBehaviour
{
    public static BossAttackRang instance;
    [SerializeField] GameObject attackRang;
    public bool isStart;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (isStart)
        {
            StartCoroutine(Attacking());
            isStart = false;
        }
    }
    IEnumerator Attacking()
    {
        for (int i = 0; i < 7; i++)
        {
            attackRang.SetActive(false);
            yield return new WaitForSecondsRealtime(0.1f);
            attackRang.SetActive(true);
            yield return new WaitForSecondsRealtime(0.1f);
        }
        MidBoss.Instance.isAttacking = true;
        Camera.main.GetComponent<CameraShake>().SetUp(0.5f, 0.2f);
        gameObject.SetActive(false);;
    }
}
