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
    #region 변수 선언
    public Bullets bullets;
    [SerializeField] float speed;
    #endregion

    #region 생명주기
    void Update()
    {
        Destroy(gameObject, 7f);
        Move();
        BulletDestory();
    }
    #endregion

    #region 이동 관련
    void Move()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);
    }
    #endregion

    #region 총알 제거
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
