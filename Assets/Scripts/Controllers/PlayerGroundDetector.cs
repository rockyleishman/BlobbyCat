using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundDetector : MonoBehaviour
{
    private PlayerValues _playerValuesObject;
    private PlayerStatus _playerStatusObject;

    private float _hangtimer;
    private float _groundedTimer;

    private void Start()
    {
        //init fields
        _playerValuesObject = DataManager.Instance.PlayerValuesObject;
        _playerStatusObject = DataManager.Instance.PlayerStatusObject;
        _hangtimer = 0.0f;
        _groundedTimer = 0.0f;
    }

    private void Update()
    {
        RaycastHit2D hitCentre = Physics2D.Raycast(transform.position, Vector2.down, _playerValuesObject.GroundDetectionRange, LayerMask.GetMask("Ground"));
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position + new Vector3(_playerValuesObject.GroundDetectionSpan * -0.5f, 0.0f, 0.0f), Vector2.down, _playerValuesObject.GroundDetectionRange, LayerMask.GetMask("Ground"));
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position + new Vector3(_playerValuesObject.GroundDetectionSpan * 0.5f, 0.0f, 0.0f), Vector2.down, _playerValuesObject.GroundDetectionRange, LayerMask.GetMask("Ground"));

        if ((hitCentre.collider != null || hitLeft.collider != null || hitRight.collider != null) && !_playerStatusObject.IsGrounded)
        {
            //start grounded
            _playerStatusObject.IsGrounded = true;
            _hangtimer = _playerValuesObject.Hangtime;
            _groundedTimer = 0.0f;
            _playerStatusObject.HasHighJumpToken = true;
            _playerStatusObject.HasDoubleJumpToken = false;
            _playerStatusObject.HasTripleJumpToken = false;

            //interrupt pounce attack
            if (_playerStatusObject.IsPounceAttacking)
            {
                //TODO: change to event
                GetComponentInParent<PlayerAttackController>().InterruptAttack(_playerValuesObject.PounceAttackCooldown);
            }
        }
        else if (hitCentre.collider == null && hitLeft.collider == null && hitRight.collider == null && _playerStatusObject.IsGrounded)
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
