using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IsBool
{
    isTrue,
    isFalse
}

public class SpawnManager : MonoBehaviour
{
    #region 변수 선언
    [SerializeField] private GameObject spawnPrefab;
    [SerializeField] public float spawnTime = 5f;
    [SerializeField] public bool isItem = false;

    [SerializeField] private float radius = 50f; // 원의 반지름
    [SerializeField] private Vector2 circleCenter = Vector2.zero;
    [SerializeField] private float angleIncrement = 20f;

    private float timer = 0f;
    public IsBool isBool;
    #endregion

    #region Unity 메소드
    private void Update()
    {
        if (isBool != IsBool.isTrue && !isItem)
            return;

        HandleSpawn();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(circleCenter, radius);
    }
    #endregion

    #region 스폰 관련
    private void HandleSpawn()
    {
        timer += Time.deltaTime;
        if (timer >= spawnTime)
        {
            SpawnObject();
            timer = 0f;
        }
    }

    private void SpawnObject()
    {
        float angle = Random.Range(0f, 180f) * Mathf.Deg2Rad;
        float x = circleCenter.x + radius * Mathf.Cos(angle);
        float y = circleCenter.y + radius * Mathf.Sin(angle);
        Vector3 spawnPosition = new Vector3(x, 0, y);

        Instantiate(spawnPrefab, spawnPosition, Quaternion.Euler(0, angle * Mathf.Rad2Deg, 0));
    }
    #endregion
}
