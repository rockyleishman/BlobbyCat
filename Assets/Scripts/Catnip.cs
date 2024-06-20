using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catnip : Collectable
{
    [SerializeField] private float _respawnTime = 10.0f;
    private Vector3 _initialPosition;

    protected override void Start()
    {
        base.Start();

        //disable suction
        _isBeingSucked = true;

        //find initial position
        _initialPosition = transform.position;

        //randomize animation
        _animator.Play("Catnip_Floating", -1, Random.value);
    }

    protected override void Collect()
    {
        _playerStatusObject.Player.GetComponent<PlayerCatnipController>().GainCatnip();

        //effects
        PoolManager.Instance.Spawn(CollectEffect.name, transform.position, transform.rotation);

        //stop coroutines
        StopAllCoroutines();

        //despawn & respawn
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        //send to limbo
        transform.position = GameManager.Instance.ObjectLimboPosition;

        yield return new WaitForSeconds(_respawnTime);

        //return to initial position
        transform.position = _initialPosition;
    }
}
