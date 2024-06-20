using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatnipController : MonoBehaviour
{
    private PlayerValues _playerValuesObject;
    private PlayerStatus _playerStatusObject;

    [SerializeField] private ParticleSystem _catnipEffect;

    private void Start()
    {
        //init fields
        _playerValuesObject = DataManager.Instance.PlayerValuesObject;
        _playerStatusObject = DataManager.Instance.PlayerStatusObject;

        //start with particle system disabled
        _catnipEffect.Stop();
    }

    public void GainCatnip()
    {
        //allow dart if already used
        _playerStatusObject.HasDartToken = true;

        //allow one extra jump for the next X seconds
        StopAllCoroutines();
        StartCoroutine(OnGainCatnip());
    }

    private IEnumerator OnGainCatnip()
    {
        _playerStatusObject.HasCatnipToken = true;
        _catnipEffect.Play();

        yield return new WaitForSeconds(_playerValuesObject.MaxCatnipTime);

        _catnipEffect.Stop();
        _playerStatusObject.HasCatnipToken = false;
    }

    public void UseCatnip()
    {
        StopAllCoroutines();

        _catnipEffect.Stop();
        _playerStatusObject.HasCatnipToken = false;

        PoolManager.Instance.Spawn(_playerValuesObject.CatnipJumpEffect.name, transform.position, transform.rotation);
    }
}
