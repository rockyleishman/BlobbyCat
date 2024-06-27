using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    private PlayerValues _playerValuesObject;
    private PlayerStatus _playerStatusObject;

    private Collider2D _collider;

    private void Start()
    {
        //init fields
        _playerValuesObject = DataManager.Instance.PlayerValuesObject;
        _playerStatusObject = DataManager.Instance.PlayerStatusObject;
        _collider = GetComponent<Collider2D>();

        //disable collider
        _collider.enabled = false;
    }

    //call with event
    public void EnableCollider()
    {
        _collider.enabled = true;
    }

    //call with event
    public void DisableCollider()
    {
        _collider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //deal damage
        HitPointController attackableObject = collider.GetComponent<HitPointController>();
        if (attackableObject != null)
        {
            if (_playerStatusObject.IsSlapAttacking)
            {
                attackableObject.Damage(_playerValuesObject.SlapAttackDamage);
            }
            else if (_playerStatusObject.IsSlamAttacking)
            {
                attackableObject.Damage(_playerValuesObject.SlamAttackDamage);
            }
            else if (_playerStatusObject.IsSpinAttacking)
            {
                attackableObject.Damage(_playerValuesObject.SpinAttackDamage);
            }
            if (_playerStatusObject.IsPounceAttacking)
            {
                attackableObject.Damage(_playerValuesObject.PounceAttackDamage);
            }
        }

        //roll yarn
        YarnBall yarnBall = collider.GetComponent<YarnBall>();
        if (yarnBall != null)
        {
            if (_playerStatusObject.IsSlapAttacking)
            {
                yarnBall.AttackHit(transform.position, _playerValuesObject.SlapAttackDamage * _playerValuesObject.YarnDamageVelocityMultiplier);
            }
            else if (_playerStatusObject.IsSlamAttacking)
            {
                yarnBall.AttackHit(transform.position, _playerValuesObject.SlamAttackDamage * _playerValuesObject.YarnDamageVelocityMultiplier);
            }
            else if (_playerStatusObject.IsSpinAttacking)
            {
                yarnBall.AttackHit(transform.position, _playerValuesObject.SpinAttackDamage * _playerValuesObject.YarnDamageVelocityMultiplier);
            }
            if (_playerStatusObject.IsPounceAttacking)
            {
                yarnBall.AttackHit(transform.position, _playerValuesObject.PounceAttackDamage * _playerValuesObject.YarnDamageVelocityMultiplier);
            }
        }

        //TODO: knockback

    }
}
