using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLifeTime : MonoBehaviour
{
    [SerializeField]
    private float lifeTime;

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(LifeTime());
    }

    IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(lifeTime);
        BulletPool.Instance.ReleaseBullet(this.gameObject);
    }
}
