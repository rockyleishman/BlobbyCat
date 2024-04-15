using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackController : MonoBehaviour
{
    private PlayerValues _playerValuesObject;
    private PlayerStatus _playerStatusObject;

    private Animator _animator;

    private bool _crouchInput;
    private bool _specialInput;

    private float _cooldownTimer;

    private void Start()
    {
        //init fields
        _playerValuesObject = DataManager.Instance.PlayerValuesObject;
        _playerStatusObject = DataManager.Instance.PlayerStatusObject;
        _animator = GetComponent<Animator>();
        _crouchInput = false;
        _specialInput = false;
        _cooldownTimer = 0.0f;
    }

    private void Update()
    {
        _cooldownTimer -= Time.deltaTime;
    }

    private void OnAttack()
    {
        if (!_playerStatusObject.IsAttacking && _cooldownTimer <= 0.0f)
        {
            if (_crouchInput && !_specialInput && _playerStatusObject.IsGrounded)
            {
                StartPounceAttack();
            }
            else if (_specialInput)
            {
                StartCoroutine(SpinAttackCoroutine());
            }
            else if (_playerStatusObject.IsGrounded)
            {
                StartCoroutine(SlapAttackCoroutine());
            }
        }        
    }

    IEnumerator SlapAttackCoroutine()
    {
        //is attacking
        _playerStatusObject.IsAttacking = true;
        _playerStatusObject.IsSlapAttacking = true;

        //enable attack hitbox
        if (_playerStatusObject.IsFacingRight)
        {
            EventManager.Instance.OnEnableRightSlapHitbox.TriggerEvent(transform.position);
        }
        else
        {
            EventManager.Instance.OnEnableLeftSlapHitbox.TriggerEvent(transform.position);
        }

        //start animation
        _animator.SetTrigger("SlapAttack");

        yield return new WaitForSeconds(_playerValuesObject.SlapAttackTime);

        //disable attack hitboxes
        EventManager.Instance.OnDisableRightSlapHitbox.TriggerEvent(transform.position);
        EventManager.Instance.OnDisableLeftSlapHitbox.TriggerEvent(transform.position);

        //set cooldown before next attack can be made
        _cooldownTimer = _playerValuesObject.SlapAttackCooldown;

        //is not attacking
        _playerStatusObject.IsAttacking = false;
        _playerStatusObject.IsSlapAttacking = false;
    }

    IEnumerator SpinAttackCoroutine()
    {
        //is attacking
        _playerStatusObject.IsAttacking = true;
        _playerStatusObject.IsSpinAttacking = true;

        //enable attack hitboxes
        EventManager.Instance.OnEnableRightSpinHitbox.TriggerEvent(transform.position);
        EventManager.Instance.OnEnableLeftSpinHitbox.TriggerEvent(transform.position);

        //start animation
        _animator.SetTrigger("SpinAttack");

        yield return new WaitForSeconds(_playerValuesObject.SpinAttackTime);

        //disable attack hitboxes
        EventManager.Instance.OnDisableRightSpinHitbox.TriggerEvent(transform.position);
        EventManager.Instance.OnDisableLeftSpinHitbox.TriggerEvent(transform.position);

        //set cooldown before next attack can be made
        _cooldownTimer = _playerValuesObject.SpinAttackCooldown;

        //is not attacking
        _playerStatusObject.IsAttacking = false;
        _playerStatusObject.IsSpinAttacking = false;
    }

    private void StartPounceAttack()
    {
        //is attacking
        _playerStatusObject.IsAttacking = true;
        _playerStatusObject.IsPounceAttacking = true;

        //enable attack hitbox
        if (_playerStatusObject.IsFacingRight)
        {
            EventManager.Instance.OnEnableRightPounceHitbox.TriggerEvent(transform.position);
        }
        else
        {
            EventManager.Instance.OnEnableLeftPounceHitbox.TriggerEvent(transform.position);
        }

        //start animation
        _animator.SetBool("IsPounceAttacking", true);
    }

    public void InterruptAttack(float cooldown)
    {
        //interrupt attack coroutines
        StopAllCoroutines();

        //disable attack hitboxes
        EventManager.Instance.OnDisableRightSlapHitbox.TriggerEvent(transform.position);
        EventManager.Instance.OnDisableLeftSlapHitbox.TriggerEvent(transform.position);
        EventManager.Instance.OnDisableRightSpinHitbox.TriggerEvent(transform.position);
        EventManager.Instance.OnDisableLeftSpinHitbox.TriggerEvent(transform.position);
        EventManager.Instance.OnDisableRightPounceHitbox.TriggerEvent(transform.position);
        EventManager.Instance.OnDisableLeftPounceHitbox.TriggerEvent(transform.position);

        //end animation
        _animator.SetBool("IsPounceAttacking", false);

        //set cooldown before next attack can be made
        _cooldownTimer = cooldown;

        //stop auto jump if was pounce attacking
        if (_playerStatusObject.IsPounceAttacking)
        {
            _playerStatusObject.IsJumping = false;
        }

        //is not attacking
        _playerStatusObject.IsAttacking = false;
        _playerStatusObject.IsSlapAttacking = false;
        _playerStatusObject.IsSpinAttacking = false;
        _playerStatusObject.IsPounceAttacking = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_playerStatusObject.IsPounceAttacking)
        {
            InterruptAttack(_playerValuesObject.PounceAttackCooldown);
        }
    }

    private void OnCrouch(InputValue value)
    {
        _crouchInput = value.Get<float>() != 0.0f;
    }

    private void OnSpecial(InputValue value)
    {
        _specialInput = value.Get<float>() != 0.0f;
    }
}
