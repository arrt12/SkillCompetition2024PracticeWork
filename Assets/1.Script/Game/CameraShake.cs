using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 originPos;

    private float strength;

    private void Awake()
    {
        originPos = transform.position;
    }
    public void SetUp(float time, float strength)
    {
        this.strength = strength;
        StopCoroutine(nameof(Shake));
        StartCoroutine(nameof(Shake),time);
    }
    private IEnumerator Shake(float time)
    {
        float t = 0f;
        while (t <= time) 
        {
            t+= Time.deltaTime;

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
}
