using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundDetector : MonoBehaviour
{
    private PlayerValues _playerValuesObject;
    private PlayerStatus _playerStatusObject;

    private Rigidbody2D _rigidbody;

    private float _hangtimer;
    private float _groundedTimer;

    private float _playerRadii;
    private float _playerPositiveCentreOffset;

    private void Start()
    {
        //init fields
        _playerValuesObject = DataManager.Instance.PlayerValuesObject;
        _playerStatusObject = DataManager.Instance.PlayerStatusObject;
        _rigidbody = _playerStatusObject.Player.GetComponent<Rigidbody2D>();
        _hangtimer = 0.0f;
        _groundedTimer = 0.0f;
        _playerRadii = _playerStatusObject.Player.GetComponent<CapsuleCollider2D>().size.y / 2.0f;
        _playerPositiveCentreOffset = (_playerStatusObject.Player.GetComponent<CapsuleCollider2D>().size.y - _playerStatusObject.Player.GetComponent<CapsuleCollider2D>().size.x) / 2.0f;
    }

    private void Update()
    {
        RaycastHit2D hitCentre = Physics2D.Raycast(transform.position + new Vector3(0.0f, -_playerRadii, 0.0f), Vector2.down, _playerValuesObject.GroundDetectionRange, LayerMask.GetMask("Ground"));
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position + new Vector3(-_playerPositiveCentreOffset, -_playerRadii, 0.0f), Vector2.down, _playerValuesObject.GroundDetectionRange, LayerMask.GetMask("Ground"));
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position + new Vector3(_playerPositiveCentreOffset, -_playerRadii, 0.0f), Vector2.down, _playerValuesObject.GroundDetectionRange, LayerMask.GetMask("Ground"));
        RaycastHit2D hitLeftSlope = Physics2D.CircleCast(transform.position + new Vector3(-_playerPositiveCentreOffset, 0.0f, 0.0f), _playerRadii, new Vector2(-1, -1), _playerValuesObject.GroundDetectionRange, LayerMask.GetMask("Slope"));
        RaycastHit2D hitRightSlope = Physics2D.CircleCast(transform.position + new Vector3(_playerPositiveCentreOffset, 0.0f, 0.0f), _playerRadii, new Vector2(1, -1), _playerValuesObject.GroundDetectionRange, LayerMask.GetMask("Slope"));

        if ((hitCentre.collider != null || hitLeft.collider != null || hitRight.collider != null || hitLeftSlope.collider != null || hitRightSlope.collider != null) && !_playerStatusObject.IsGrounded && _rigidbody.velocity.y <= 0.0f)
        {
            //start grounded
            _playerStatusObject.IsGrounded = true;
            _hangtimer = _playerValuesObject.Hangtime;
            _groundedTimer = 0.0f;
            _playerStatusObject.HasHighJumpToken = true;
            _playerStatusObject.HasDoubleJumpToken = false;
            _playerStatusObject.HasTripleJumpToken = false;
            _playerStatusObject.HasDartToken = true;

            //play effect
            if (_playerValuesObject.LandEffect != null)
            {
                PoolManager.Instance.Spawn(_playerValuesObject.LandEffect.name, transform.position, transform.rotation);
            }

            //interrupt pounce attack
            if (_playerStatusObject.IsPounceAttacking)
            {
                //TODO: change to event
                GetComponentInParent<PlayerAttackController>().InterruptAttack(_playerValuesObject.PounceAttackCooldown);
            }
        }
        else if (hitCentre.collider == null && hitLeft.collider == null && hitRight.collider == null && hitLeftSlope.collider == null && hitRightSlope.collider == null && _playerStatusObject.IsGrounded)
        {
            if (_hangtimer > 0.0f)
            {
                //hangtime grounded
                _hangtimer -= Time.deltaTime;
                _groundedTimer += Time.deltaTime;
            }
            else
            {
                //end grounded
                _playerStatusObject.IsGrounded = false;
                _playerStatusObject.HasSingleJumpToken = false;
                _playerStatusObject.HasHighJumpToken = false;
                _playerStatusObject.HasDoubleJumpToken = true;
                _playerStatusObject.HasDartToken = true;
            }
        }
        else if (_playerStatusObject.IsGrounded)
        {
            //grounded
            _groundedTimer += Time.deltaTime;
        }

        //switch jump tokens
        if (_playerStatusObject.HasHighJumpToken && _groundedTimer > _playerValuesObject.HighJumpWindow)
        {
            _playerStatusObject.HasHighJumpToken = false;
            _playerStatusObject.HasSingleJumpToken = true;
        }
    }
}
