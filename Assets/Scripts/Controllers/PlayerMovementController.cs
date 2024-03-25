using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    private PlayerValues _playerValuesObject;
    private PlayerStatus _playerStatusObject;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private Vector2 _velocity;
    private Vector2 _movementInput;

    private bool _runInput;
    private bool _crouchInput;
    private bool _specialInput;
    private bool _jumpInput;

    private float _jumpTimer;
    private float _jumpCooldownTimer;

    private void Start()
    {
        //init fields
        _playerValuesObject = DataManager.Instance.PlayerValuesObject;
        _playerStatusObject = DataManager.Instance.PlayerStatusObject;
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _velocity = Vector2.zero;
        _movementInput = Vector2.zero;
        _runInput = false;
        _crouchInput = false;
        _specialInput = false;
        _jumpInput = false;
        _jumpTimer = 0.0f;
        _jumpCooldownTimer = 0.0f;
    }

    private void Update()
    {
        //match velocity to rigidbody
        _velocity = _rigidbody.velocity;

        if (_playerStatusObject.IsPounceAttacking)
        {
            //pounce attack forces movement
            PounceMove();
            PounceJump();
        }
        else
        {
            //horizontal movement
            Move();
        }

        //jump movement
        Jump();

        //apply gravity
        Gravity();

        //clmap velocity
        ClampVelocity();

        //update rigidbody velocity
        _rigidbody.velocity = _velocity;

        //animate
        if (!_playerStatusObject.IsPounceAttacking)
        {
            OrientSprite();
        }            
        Animate();
    }

    private void Move()
    {
        if (_movementInput.x != 0.0f)
        {
            Accelerate();
        }
        else if (_velocity.x != 0.0f)
        {
            Decelerate();
        }        
    }

    private void Accelerate()
    {
        if (_playerStatusObject.IsLiquid)
        {
            _velocity.x += _movementInput.x * _playerValuesObject.LiquidAcceleration * Time.deltaTime;
        }
        else if (_playerStatusObject.IsGrounded && _crouchInput && _runInput)
        {
            //forced deceleration
            float lostSpeed = _playerValuesObject.SlideDeceleration * Time.deltaTime;

            if (_velocity.x > lostSpeed)
            {
                _velocity.x -= lostSpeed;
            }
            else if (_velocity.x < -lostSpeed)
            {
                _velocity.x += lostSpeed;
            }
            else
            {
                _velocity.x = 0.0f;
            }
        }
        else if (_playerStatusObject.IsGrounded && _crouchInput)
        {
            _velocity.x += _movementInput.x * _playerValuesObject.CrouchAcceleration * Time.deltaTime;
        }
        else if (_playerStatusObject.IsGrounded && _runInput)
        {
            _velocity.x += _movementInput.x * _playerValuesObject.GroundRunAcceleration * Time.deltaTime;
        }
        else if (_playerStatusObject.IsGrounded)
        {
            _velocity.x += _movementInput.x * _playerValuesObject.GroundAcceleration * Time.deltaTime;
        }
        else if (_runInput)
        {
            _velocity.x += _movementInput.x * _playerValuesObject.AirRunAcceleration * Time.deltaTime;
        }
        else
        {
            _velocity.x += _movementInput.x * _playerValuesObject.AirAcceleration * Time.deltaTime;
        }
    }

    private void Decelerate()
    {
        float lostSpeed;

        if (_playerStatusObject.IsLiquid)
        {
            lostSpeed = _playerValuesObject.LiquidDeceleration * Time.deltaTime;
        }
        else if (_playerStatusObject.IsGrounded && _crouchInput && _runInput)
        {
            lostSpeed = _playerValuesObject.SlideDeceleration * Time.deltaTime;
        }
        else if (_playerStatusObject.IsGrounded && _crouchInput)
        {
            lostSpeed = _playerValuesObject.CrouchDeceleration * Time.deltaTime;
        }
        else if (_playerStatusObject.IsGrounded && _runInput)
        {
            lostSpeed = _playerValuesObject.GroundRunDeceleration * Time.deltaTime;
        }
        else if (_playerStatusObject.IsGrounded)
        {
            lostSpeed = _playerValuesObject.GroundDeceleration * Time.deltaTime;
        }
        else if (_runInput)
        {
            lostSpeed = _playerValuesObject.AirRunDeceleration * Time.deltaTime;
        }
        else
        {
            lostSpeed = _playerValuesObject.AirDeceleration * Time.deltaTime;            
        }

        if (_velocity.x > lostSpeed)
        {
            _velocity.x -= lostSpeed;
        }
        else if (_velocity.x < -lostSpeed)
        {
            _velocity.x += lostSpeed;
        }
        else
        {
            _velocity.x = 0.0f;
        }
    }

    private void PounceMove()
    {
        if (_playerStatusObject.IsFacingRight)
        {
            _velocity.x = _playerValuesObject.PounceSpeed;
        }
        else
        {
            _velocity.x = -_playerValuesObject.PounceSpeed;
        }
    }

    private void Jump()
    {
        //cooldown between jumps
        _jumpCooldownTimer -= Time.deltaTime;

        if (!_jumpInput)
        {
            //allow new jump input after release
            _playerStatusObject.HasGeneralJumpToken = true;
        }
        
        //possible jump input
        HighJump();
        SingleJump();
        DoubleJump();
        TripleJump();

        //allow new pounce jumps if not currently pounce attacking
        if (!_playerStatusObject.IsPounceAttacking)
        {
            _playerStatusObject.IsPounceJumping = false;
        }
    }

    private void HighJump()
    {
        if (_playerStatusObject.HasGeneralJumpToken && _playerStatusObject.HasHighJumpToken && _jumpInput && _jumpCooldownTimer <= 0.0f)
        {
            //start jump
            _playerStatusObject.IsJumping = true;
            _playerStatusObject.IsHighJumping = true;
            _playerStatusObject.HasGeneralJumpToken = false;
            _playerStatusObject.HasHighJumpToken = false;
            _playerStatusObject.HasDoubleJumpToken = true;
            _jumpTimer = 0.0f;
            _jumpCooldownTimer = _playerValuesObject.JumpCooldown;
        }

        if (_playerStatusObject.IsHighJumping && _jumpInput && _jumpTimer < _playerValuesObject.HighJumpTime)
        {
            //jumping
            _velocity.y = _playerValuesObject.HighJumpHeight * Mathf.PI / 2.0f / _playerValuesObject.HighJumpTime * Mathf.Sin(Mathf.Lerp(Mathf.PI / 2.0f, Mathf.PI, _jumpTimer / _playerValuesObject.HighJumpTime));
            _jumpTimer += Time.deltaTime;
        }
        else if (_playerStatusObject.IsHighJumping)
        {
            //end of jump
            _playerStatusObject.IsHighJumping = false;
            _playerStatusObject.IsJumping = false;
        }
    }

    private void SingleJump()
    {
        if (_playerStatusObject.HasGeneralJumpToken && _playerStatusObject.HasSingleJumpToken && _jumpInput && _jumpCooldownTimer <= 0.0f)
        {
            //start jump
            _playerStatusObject.IsJumping = true;
            _playerStatusObject.IsSingleJumping = true;
            _playerStatusObject.HasGeneralJumpToken = false;
            _playerStatusObject.HasSingleJumpToken = false;
            _playerStatusObject.HasDoubleJumpToken = true;
            _jumpTimer = 0.0f;
            _jumpCooldownTimer = _playerValuesObject.JumpCooldown;
        }

        if (_playerStatusObject.IsSingleJumping && _jumpInput && _jumpTimer < _playerValuesObject.SingleJumpTime)
        {
            //jumping
            _velocity.y = _playerValuesObject.SingleJumpHeight * Mathf.PI / 2.0f / _playerValuesObject.SingleJumpTime * Mathf.Sin(Mathf.Lerp(Mathf.PI / 2.0f, Mathf.PI, _jumpTimer / _playerValuesObject.SingleJumpTime));
            _jumpTimer += Time.deltaTime;
        }
        else if (_playerStatusObject.IsSingleJumping)
        {
            //end of jump
            _playerStatusObject.IsSingleJumping = false;
            _playerStatusObject.IsJumping = false;
        }
    }

    private void DoubleJump()
    {
        if (_playerStatusObject.HasGeneralJumpToken && _playerStatusObject.HasDoubleJumpToken && _jumpInput && _jumpCooldownTimer <= 0.0f)
        {
            //start jump
            _playerStatusObject.IsJumping = true;
            _playerStatusObject.IsSingleJumping = false;
            _playerStatusObject.IsDoubleJumping = true;
            _playerStatusObject.HasGeneralJumpToken = false;
            _playerStatusObject.HasDoubleJumpToken = false;
            _playerStatusObject.HasTripleJumpToken = true;
            _jumpTimer = 0.0f;
            _jumpCooldownTimer = _playerValuesObject.JumpCooldown;

            //interrupt pounce attack
            if (_playerStatusObject.IsPounceAttacking)
            {
                //TODO: change to event
                GetComponent<PlayerAttackController>().InterruptAttack(_playerValuesObject.PounceAttackCooldown);
            }
        }

        if (_playerStatusObject.IsDoubleJumping && _jumpInput && _jumpTimer < _playerValuesObject.DoubleJumpTime)
        {
            //jumping
            _velocity.y = _playerValuesObject.DoubleJumpHeight * Mathf.PI / 2.0f / _playerValuesObject.DoubleJumpTime * Mathf.Sin(Mathf.Lerp(Mathf.PI / 2.0f, Mathf.PI, _jumpTimer / _playerValuesObject.DoubleJumpTime));
            _jumpTimer += Time.deltaTime;
        }
        else if (_playerStatusObject.IsDoubleJumping)
        {
            //end of jump
            _playerStatusObject.IsDoubleJumping = false;
            _playerStatusObject.IsJumping = false;
        }
    }

    private void TripleJump()
    {
        if (_playerStatusObject.HasGeneralJumpToken && _playerStatusObject.HasTripleJumpToken && _jumpInput && _jumpCooldownTimer <= 0.0f)
        {
            //start jump
            _playerStatusObject.IsJumping = true;
            _playerStatusObject.IsDoubleJumping = false;
            _playerStatusObject.IsTripleJumping = true;
            _playerStatusObject.HasGeneralJumpToken = false;
            _playerStatusObject.HasTripleJumpToken = false;
            _jumpTimer = 0.0f;
            _jumpCooldownTimer = _playerValuesObject.JumpCooldown;
        }

        if (_playerStatusObject.IsTripleJumping && _jumpInput && _jumpTimer < _playerValuesObject.TripleJumpTime)
        {
            //jumping
            _velocity.y = _playerValuesObject.TripleJumpHeight * Mathf.PI / 2.0f / _playerValuesObject.TripleJumpTime * Mathf.Sin(Mathf.Lerp(Mathf.PI / 2.0f, Mathf.PI, _jumpTimer / _playerValuesObject.TripleJumpTime));
            _jumpTimer += Time.deltaTime;
        }
        else if (_playerStatusObject.IsTripleJumping)
        {
            //end of jump
            _playerStatusObject.IsTripleJumping = false;
            _playerStatusObject.IsJumping = false;
        }
    }

    private void PounceJump()
    {        
        if (!_playerStatusObject.IsPounceJumping)
        {
            //start jump
            _playerStatusObject.IsJumping = true;
            _playerStatusObject.IsPounceJumping = true;
            _playerStatusObject.HasSingleJumpToken = false;
            _playerStatusObject.HasDoubleJumpToken = true;
            _jumpTimer = 0.0f;
            _jumpCooldownTimer = _playerValuesObject.JumpCooldown;
        }        

        if (_playerStatusObject.IsPounceAttacking && _jumpTimer < _playerValuesObject.PounceJumpTime)
        {
            //jumping
            _velocity.y = _playerValuesObject.PounceJumpHeight * Mathf.PI / 2.0f / _playerValuesObject.PounceJumpTime * Mathf.Sin(Mathf.Lerp(Mathf.PI / 2.0f, Mathf.PI, _jumpTimer / _playerValuesObject.PounceJumpTime));
            _jumpTimer += Time.deltaTime;
        }
        else
        {
            //end of jump
            _playerStatusObject.IsJumping = false;
        }
    }

    private void Gravity()
    {
        if (!_playerStatusObject.IsJumping)
        {
            //additional positive velocity decay when not jumping
            if (_velocity.y > 0.0f)
            {
                   _velocity.y -= _velocity.y / _playerValuesObject.PositiveVelocityHalflife * Time.deltaTime;
            }

            //gravity
            _velocity.y -= _playerValuesObject.Gravity * Time.deltaTime;
        }
    }

    private void ClampVelocity()
    {
        //clamp x velocity
        if (_playerStatusObject.IsPounceAttacking)
        {
            _velocity.x = Mathf.Clamp(_velocity.x, -_playerValuesObject.PounceSpeed, _playerValuesObject.PounceSpeed);
        }
        else if (_playerStatusObject.IsLiquid)
        {
            _velocity.x = Mathf.Clamp(_velocity.x, -_playerValuesObject.LiquidMovementSpeed, _playerValuesObject.LiquidMovementSpeed);
        }
        else if (_playerStatusObject.IsGrounded && _crouchInput && _runInput)
        {
            _velocity.x = Mathf.Clamp(_velocity.x, -_playerValuesObject.GroundRunMovementSpeed, _playerValuesObject.GroundRunMovementSpeed);
        }
        else if (_playerStatusObject.IsGrounded && _crouchInput)
        {
            _velocity.x = Mathf.Clamp(_velocity.x, -_playerValuesObject.CrouchMovementSpeed, _playerValuesObject.CrouchMovementSpeed);
        }
        else if (_playerStatusObject.IsGrounded && _runInput)
        {
            _velocity.x = Mathf.Clamp(_velocity.x, -_playerValuesObject.GroundRunMovementSpeed, _playerValuesObject.GroundRunMovementSpeed);
        }
        else if (_playerStatusObject.IsGrounded)
        {
            _velocity.x = Mathf.Clamp(_velocity.x, -_playerValuesObject.GroundMovementSpeed, _playerValuesObject.GroundMovementSpeed);
        }
        else if (_runInput)
        {
            _velocity.x = Mathf.Clamp(_velocity.x, -_playerValuesObject.AirRunMovementSpeed, _playerValuesObject.AirRunMovementSpeed);
        }
        else
        {
            _velocity.x = Mathf.Clamp(_velocity.x, -_playerValuesObject.AirMovementSpeed, _playerValuesObject.AirMovementSpeed);
        }

        //clamp y velocity
        _velocity.y = Mathf.Clamp(_velocity.y, -_playerValuesObject.NegativeTerminalVelocity, Mathf.Infinity);
    }

    private void OrientSprite()
    {
        if (_playerStatusObject.IsFacingRight && _movementInput.x < 0.0f)
        {
            _playerStatusObject.IsFacingRight = false;
            _spriteRenderer.flipX = true;
        }
        else if (!_playerStatusObject.IsFacingRight && _movementInput.x > 0.0f)
        {
            _playerStatusObject.IsFacingRight = true;
            _spriteRenderer.flipX = false;
        }
    }

    private void Animate()
    {
        //horizontal movement
        _animator.SetFloat("HorizontalInputScalar", Mathf.Abs(_movementInput.x));
        _animator.SetFloat("HorizontalSpeed", Mathf.Abs(_velocity.x));

        //vertical movement
        if (_playerStatusObject.IsGrounded)
        {
            _animator.SetFloat("VerticalVelocity", 0.0f);
        }
        else
        {
            _animator.SetFloat("VerticalVelocity", _velocity.y);
        }

        //is grounded
        _animator.SetBool("IsGrounded", _playerStatusObject.IsGrounded);

        //is crouching
        _animator.SetBool("IsCrouching", _crouchInput);

        //is sliding
        _animator.SetBool("IsSliding", _runInput && _crouchInput);
    }

    private void OnMovement(InputValue value)
    {
        _movementInput = value.Get<Vector2>();
    }

    private void OnRun(InputValue value)
    {
        _runInput = value.Get<float>() != 0.0f;
    }

    private void OnCrouch(InputValue value)
    {
        _crouchInput = value.Get<float>() != 0.0f;
    }

    private void OnSpecial(InputValue value)
    {
        _specialInput = value.Get<float>() != 0.0f;
    }

    private void OnJump(InputValue value)
    {
        _jumpInput = value.Get<float>() != 0.0f;
    }
}
