using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : PoolObject
{
    [SerializeField] public float Lifetime = 1.0f;

    private void OnEnable()
    {
        StartCoroutine(AutoDespawn());
    }

    private IEnumerator AutoDespawn()
    {
        yield return new WaitForSeconds(Lifetime);

        OnDespawn();
    }
}
