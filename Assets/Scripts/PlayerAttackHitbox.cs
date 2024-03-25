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
        AttackableObject attackableObject = collider.GetComponent<AttackableObject>();
        if (attackableObject != null)
        {
            if (_playerStatusObject.IsSlapAttacking)
            {
                //TODO: deal damage
                Debug.Log(attackableObject.gameObject.name + " hit with slap attack");
                Destroy(attackableObject.gameObject);
            }
            else if (_playerStatusObject.IsSpinAttacking)
            {
                //TODO: deal damage
                Debug.Log(attackableObject.gameObject.name + " hit with spin attack");
                Destroy(attackableObject.gameObject);
            }
            if (_playerStatusObject.IsPounceAttacking)
            {
                //TODO: deal damage
                Debug.Log(attackableObject.gameObject.name + " hit with pounce attack");
                Destroy(attackableObject.gameObject);
            }
        }
    }
}
