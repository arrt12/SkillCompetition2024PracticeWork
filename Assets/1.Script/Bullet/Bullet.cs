using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Bullets
{
    player,
    monseter
}

public class Bullet : MonoBehaviour
{
    #region ���� ����
    public Bullets bullets;
    [SerializeField] float speed;
    #endregion

    #region �����ֱ�
    void Update()
    {
        Destroy(gameObject, 7f);
        Move();
        BulletDestory();
    }
    #endregion

    #region �̵� ����
    void Move()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);
    }
    #endregion

    #region �Ѿ� ����
    void BulletDestory()
    {
        var viewPos = Camera.main.WorldToViewportPoint(transform.position);

        if (viewPos.x > 1f) Destroy(gameObject);
        if (viewPos.x < 0f) Destroy(gameObject);
        if (viewPos.y > 1f) Destroy(gameObject);
        if (viewPos.y < 0f) Destroy(gameObject);
    }
    #endregion
}
