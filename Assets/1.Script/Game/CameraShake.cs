using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    #region 변수 선언
    private Vector3 originPos;
    private float strength;
    #endregion

    #region 생명주기
    private void Awake()
    {
        originPos = transform.position;
    }
    #endregion

    #region 외부 호출 함수
    public void SetUp(float time, float strength)
    {
        this.strength = strength;
        StopCoroutine(nameof(Shake));
        StartCoroutine(nameof(Shake), time);
    }
    #endregion

    #region 내부 기능
    private IEnumerator Shake(float time)
    {
        float t = 0f;
        while (t <= time)
        {
            t += Time.deltaTime;
            var newPos = transform.position + Random.insideUnitSphere * strength;
            transform.position = new Vector3(newPos.x, newPos.y, originPos.z);
            yield return null;
        }

        float y = 0f;
        while (y <= 1f)
        {
            y += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, originPos, y);
            yield return null;
        }

        transform.position = originPos;
    }
    #endregion
}
