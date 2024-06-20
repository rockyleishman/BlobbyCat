using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    private PlayerValues _playerValuesObject;
    private PlayerStatus _playerStatusObject;
    private GameStatus _gameStatusObject;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    [SerializeField] private ParticleSystem _stepEffect;

    private Vector2 _velocity;
    private Vector2 _movementInput;

    private bool _runInput;
    private bool _jumpInput;

    private float _dartTimer;

    private float _jumpTimer;
    private float _jumpCooldownTimer;

    private bool _isConflictingInputEnabled;

    private float _runningJumpTime;
    private float _runningJumpHeight;

    private void Start()
    {
        //init fields
        _playerValuesObject = DataManager.Instance.PlayerValuesObject;
        _playerStatusObject = DataManager.Instance.PlayerStatusObject;
        _gameStatusObject = DataManager.Instance.GameStatusObject;
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _stepEffect = GetComponentInChildren<ParticleSystem>();
        _velocity = Vector2.zero;
        _movementInput = Vector2.zero;
        _runInput = false;
        _jumpInput = false;
        _dartTimer = 0.0f;
        _jumpTimer = 0.0f;
        _jumpCooldownTimer = 0.0f;
        _isConflictingInputEnabled = true;
    }

    private void OnEnable()
    {
        //reset velocity
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        //reset input
        _movementInput = Vector2.zero;
        _runInput = false;
        _jumpInput = false;
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

        //decrease dart timer
        _dartTimer -= Time.deltaTime;

        //jump movement
        if (!_playerStatusObject.IsSlamAttacking)
        {
            Jump();
        }

        //apply gravity
        Gravity();

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

        OverdriveDeceleration();
    }

    private void Accelerate()
    {
        float gainedSpeed;
        float maximumGainedSpeed;

        if (_playerStatusObject.IsGrounded && _playerStatusObject.IsCrouching && _runInput) /////////////////////////////TODO: allow crawl movement
        {
            //slide
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

            gainedSpeed = 0.0f;
        }
        else if (_playerStatusObject.IsGrounded && _playerStatusObject.IsCrouching)
        {
            //crouch
            gainedSpeed = Mathf.Abs(_movementInput.x) * _playerValuesObject.CrouchAcceleration * Time.deltaTime;
            maximumGainedSpeed = _playerValuesObject.CrouchMovementSpeed - Mathf.Abs(_velocity.x);
            if (maximumGainedSpeed > 0.0f && gainedSpeed > maximumGainedSpeed)
            {
                gainedSpeed = maximumGainedSpeed;
            }
            else if (gainedSpeed > maximumGainedSpeed && ((_movementInput.x > 0.0f && _velocity.x > 0.0f) || (_movementInput.x < 0.0f && _velocity.x < 0.0f)))
            {
                gainedSpeed = 0.0f;
            }
        }
        else if (_playerStatusObject.IsGrounded && _runInput && Mathf.Abs(_velocity.x) >= _playerValuesObject.GroundMovementSpeed && ((_movementInput.x > 0.0f && _velocity.x > 0.0f) || (_movementInput.x < 0.0f && _velocity.x < 0.0f)))
        {
            //ground run
            gainedSpeed = Mathf.Abs(_movementInput.x) * _playerValuesObject.GroundRunAcceleration * Time.deltaTime;
            maximumGainedSpeed = _playerValuesObject.GroundRunMovementSpeed - Mathf.Abs(_velocity.x);
            if (maximumGainedSpeed > 0.0f && gainedSpeed > maximumGainedSpeed)
            {
                gainedSpeed = maximumGainedSpeed;
            }
            else if (gainedSpeed > maximumGainedSpeed && ((_movementInput.x > 0.0f && _velocity.x > 0.0f) || (_movementInput.x < 0.0f && _velocity.x < 0.0f)))
            {
                gainedSpeed = 0.0f;
            }
        }
        else if (_playerStatusObject.IsGrounded)
        {
            //ground walk
            gainedSpeed = Mathf.Abs(_movementInput.x) * _playerValuesObject.GroundAcceleration * Time.deltaTime;
            maximumGainedSpeed = _playerValuesObject.GroundMovementSpeed - Mathf.Abs(_velocity.x);
            if (maximumGainedSpeed > 0.0f && gainedSpeed > maximumGainedSpeed)
            {
                gainedSpeed = maximumGainedSpeed;
            }
            else if (gainedSpeed > maximumGainedSpeed && ((_movementInput.x > 0.0f && _velocity.x > 0.0f) || (_movementInput.x < 0.0f && _velocity.x < 0.0f)))
            {
                gainedSpeed = 0.0f;
            }
        }
        else if (_runInput && Mathf.Abs(_velocity.x) >= _playerValuesObject.AirMovementSpeed && ((_movementInput.x > 0.0f && _velocity.x > 0.0f) || (_movementInput.x < 0.0f && _velocity.x < 0.0f)))
        {
            //air "run"
            gainedSpeed = Mathf.Abs(_movementInput.x) * _playerValuesObject.AirRunAcceleration * Time.deltaTime;
            maximumGainedSpeed = _playerValuesObject.AirRunMovementSpeed - Mathf.Abs(_velocity.x);
            if (maximumGainedSpeed > 0.0f && gainedSpeed > maximumGainedSpeed)
            {
                gainedSpeed = maximumGainedSpeed;
            }
            else if (gainedSpeed > maximumGainedSpeed && ((_movementInput.x > 0.0f && _velocity.x > 0.0f) || (_movementInput.x < 0.0f && _velocity.x < 0.0f)))
            {
                gainedSpeed = 0.0f;
            }
        }
        else
        {
            //air "walk"
            gainedSpeed = Mathf.Abs(_movementInput.x) * _playerValuesObject.AirAcceleration * Time.deltaTime;
            maximumGainedSpeed = _playerValuesObject.AirMovementSpeed - Mathf.Abs(_velocity.x);
            if (maximumGainedSpeed > 0.0f && gainedSpeed > maximumGainedSpeed)
            {
                gainedSpeed = maximumGainedSpeed;
            }
            else if (gainedSpeed > maximumGainedSpeed && ((_movementInput.x > 0.0f && _velocity.x > 0.0f) || (_movementInput.x < 0.0f && _velocity.x < 0.0f)))
            {
                gainedSpeed = 0.0f;
            }
        }

        //apply velocity changes
        if (_movementInput.x > 0.0f)
        {
            _velocity.x += gainedSpeed;
        }
        else
        {
            _velocity.x -= gainedSpeed;
        }
    }

    private void Decelerate()
    {
        float lostSpeed;

        if (_playerStatusObject.IsGrounded && _playerStatusObject.IsCrouching && _runInput)
        {
            //slide
            lostSpeed = _playerValuesObject.SlideDeceleration * Time.deltaTime;
        }
        else if (_playerStatusObject.IsGrounded && _playerStatusObject.IsCrouching)
        {
            //crouch
            lostSpeed = _playerValuesObject.CrouchDeceleration * Time.deltaTime;
        }
        else if (_playerStatusObject.IsGrounded)
        {
            //ground
            lostSpeed = _playerValuesObject.GroundDeceleration * Time.deltaTime;
        }
        else
        {
            //air
            lostSpeed = _playerValuesObject.AirDeceleration * Time.deltaTime;            
        }

        //apply velocity changes
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

    private void OverdriveDeceleration()
    {
        if (_dartTimer <= 0.0f)
        {
            float lostSpeed;

            if (_playerStatusObject.IsGrounded && _playerStatusObject.IsCrouching)
            {
                lostSpeed = _playerValuesObject.CrouchOverdriveDeceleration * Time.deltaTime;
            }
            else
            {
                lostSpeed = _playerValuesObject.OverdriveDeceleration * Time.deltaTime;
            }            

            if (_playerStatusObject.IsPounceAttacking && Mathf.Abs(_velocity.x) - _playerValuesObject.PounceSpeed < lostSpeed)
            {
                //pounce
                _velocity.x = Mathf.Clamp(_velocity.x, -_playerValuesObject.PounceSpeed, _playerValuesObject.PounceSpeed);
                lostSpeed = 0.0f;
            }
            else if (!_playerStatusObject.IsPounceAttacking && _playerStatusObject.IsGrounded && _runInput && Mathf.Abs(_velocity.x) - _playerValuesObject.GroundRunMovementSpeed < lostSpeed)
            {
                //ground run / slide
                _velocity.x = Mathf.Clamp(_velocity.x, -_playerValuesObject.GroundRunMovementSpeed, _playerValuesObject.GroundRunMovementSpeed);
                lostSpeed = 0.0f;
            }
            else if (!_playerStatusObject.IsPounceAttacking && _playerStatusObject.IsGrounded && !_runInput && _playerStatusObject.IsCrouching && Mathf.Abs(_velocity.x) - _playerValuesObject.CrouchMovementSpeed < lostSpeed)
            {
                //crouch
                _velocity.x = Mathf.Clamp(_velocity.x, -_playerValuesObject.CrouchMovementSpeed, _playerValuesObject.CrouchMovementSpeed);
                lostSpeed = 0.0f;
            }
            else if (!_playerStatusObject.IsPounceAttacking && _playerStatusObject.IsGrounded && !_runInput && !_playerStatusObject.IsCrouching && Mathf.Abs(_velocity.x) - _playerValuesObject.GroundMovementSpeed < lostSpeed)
            {
                //ground walk
                _velocity.x = Mathf.Clamp(_velocity.x, -_playerValuesObject.GroundMovementSpeed, _playerValuesObject.GroundMovementSpeed);
                lostSpeed = 0.0f;
            }
            else if (!_playerStatusObject.IsPounceAttacking && !_playerStatusObject.IsGrounded && _runInput && Mathf.Abs(_velocity.x) - _playerValuesObject.AirRunMovementSpeed < lostSpeed)
            {
                //air "run"
                _velocity.x = Mathf.Clamp(_velocity.x, -_playerValuesObject.AirRunMovementSpeed, _playerValuesObject.AirRunMovementSpeed);
                lostSpeed = 0.0f;
            }
            else if (!_playerStatusObject.IsPounceAttacking && !_playerStatusObject.IsGrounded && !_runInput && Mathf.Abs(_velocity.x) - _playerValuesObject.AirMovementSpeed < lostSpeed)
            {
                //air "walk"
                _velocity.x = Mathf.Clamp(_velocity.x, -_playerValuesObject.AirMovementSpeed, _playerValuesObject.AirMovementSpeed);
                lostSpeed = 0.0f;
            }

            //apply velocity changes
            if (_velocity.x > lostSpeed)
            {
                _velocity.x -= lostSpeed;
            }
            else if (_velocity.x < -lostSpeed)
            {
                _velocity.x += lostSpeed;
            }
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

        //disallow high jumps if darting
        if (_dartTimer > 0.0f && _playerStatusObject.HasHighJumpToken)
        {
            _playerStatusObject.HasHighJumpToken = false;
            _playerStatusObject.HasSingleJumpToken = true;
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

            //play effect
            if (_playerValuesObject.SingleJumpEffect != null)
            {
                PoolManager.Instance.Spawn(_playerValuesObject.SingleJumpEffect.name, transform.position, transform.rotation);
            }
            if (_playerValuesObject.HighJumpEffect != null)
            {
                PoolManager.Instance.Spawn(_playerValuesObject.HighJumpEffect.name, transform.position, transform.rotation);
            }
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

            //play effect
            if (_playerValuesObject.SingleJumpEffect != null)
            {
                PoolManager.Instance.Spawn(_playerValuesObject.SingleJumpEffect.name, transform.position, transform.rotation);
            }

            //calculate jump time and height based on horizontal speed
            if (_dartTimer > 0.0f || Mathf.Abs(_velocity.x) <= _playerValuesObject.GroundMovementSpeed)
            {
                _runningJumpHeight = _playerValuesObject.SingleJumpHeight;
                _runningJumpTime = _playerValuesObject.SingleJumpTime;
            }            
            else if (Mathf.Abs(_velocity.x) >= _playerValuesObject.GroundRunMovementSpeed)
            {
                _runningJumpHeight = _playerValuesObject.HighJumpHeight;
                _runningJumpTime = _playerValuesObject.HighJumpTime;

                //play extra effect
                if (_playerValuesObject.HighJumpEffect != null)
                {
                    PoolManager.Instance.Spawn(_playerValuesObject.HighJumpEffect.name, transform.position, transform.rotation);
                }
            }
            else
            {
                _runningJumpHeight = Mathf.Lerp(_playerValuesObject.SingleJumpHeight, _playerValuesObject.HighJumpHeight * 0.8f, (Mathf.Abs(_velocity.x) - _playerValuesObject.GroundMovementSpeed) / (_playerValuesObject.GroundRunMovementSpeed - _playerValuesObject.GroundMovementSpeed));
                _runningJumpTime = Mathf.Lerp(_playerValuesObject.SingleJumpTime, _playerValuesObject.HighJumpTime * 0.8f, (Mathf.Abs(_velocity.x) - _playerValuesObject.GroundMovementSpeed) / (_playerValuesObject.GroundRunMovementSpeed - _playerValuesObject.GroundMovementSpeed));
            }
        }

        if (_playerStatusObject.IsSingleJumping && _jumpInput && _jumpTimer < _runningJumpTime)
        {
            //jumping
            _velocity.y = _runningJumpHeight * Mathf.PI / 2.0f / _runningJumpTime * Mathf.Sin(Mathf.Lerp(Mathf.PI / 2.0f, Mathf.PI, _jumpTimer / _runningJumpTime));
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
        if (_playerStatusObject.HasGeneralJumpToken && ((_playerStatusObject.HasDoubleJumpToken && _gameStatusObject.unlockedDoubleJump) || _playerStatusObject.HasCatnipToken) && _jumpInput && _jumpCooldownTimer <= 0.0f && !_playerStatusObject.IsAlmostGrounded)
        {
            //use catnip token if no double jump unlocked
            if (!_gameStatusObject.unlockedDoubleJump)
            {
                GetComponent<PlayerCatnipController>().UseCatnip();
            }

            //start jump
            _playerStatusObject.IsJumping = true;
            _playerStatusObject.IsSingleJumping = false;
            _playerStatusObject.IsDoubleJumping = true;
            _playerStatusObject.HasGeneralJumpToken = false;
            _playerStatusObject.HasDoubleJumpToken = false;
            _jumpTimer = 0.0f;
            _jumpCooldownTimer = _playerValuesObject.JumpCooldown;

            //play effect
            if (_playerValuesObject.DoubleJumpEffect != null)
            {
                PoolManager.Instance.Spawn(_playerValuesObject.DoubleJumpEffect.name, transform.position, transform.rotation);
            }

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
        if (_playerStatusObject.HasGeneralJumpToken && _gameStatusObject.unlockedDoubleJump && _playerStatusObject.HasCatnipToken && _jumpInput && _jumpCooldownTimer <= 0.0f && !_playerStatusObject.IsAlmostGrounded)
        {
            //use catnip token
            GetComponent<PlayerCatnipController>().UseCatnip();

            //start jump
            _playerStatusObject.IsJumping = true;
            _playerStatusObject.IsDoubleJumping = false;
            _playerStatusObject.IsTripleJumping = true;
            _playerStatusObject.HasGeneralJumpToken = false;
            _jumpTimer = 0.0f;
            _jumpCooldownTimer = _playerValuesObject.JumpCooldown;

            //play effect
            if (_playerValuesObject.TripleJumpEffect != null)
            {
                PoolManager.Instance.Spawn(_playerValuesObject.TripleJumpEffect.name, transform.position, transform.rotation);
            }
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

            //play effect
            if (_playerValuesObject.PounceJumpEffect != null)
            {
                PoolManager.Instance.Spawn(_playerValuesObject.PounceJumpEffect.name, transform.position, transform.rotation);
            }
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

            //no gravity when darting and rising
            if (_dartTimer <= 0.0f || _velocity.y > 0.0f)
            {
                //gravity
                if (_playerStatusObject.IsSlamAttacking)
                {
                    _velocity.y -= _playerValuesObject.SlamAttackGravity * Time.deltaTime;
                }
                else if (_playerStatusObject.IsSpinAttacking)
                {
                    _velocity.y -= _playerValuesObject.SpinAttackGravity * Time.deltaTime;
                }
                else if (_playerStatusObject.IsGrounded)
                {
                    _velocity.y -= _playerValuesObject.GroundedGravity * Time.deltaTime;
                }
                else
                {
                    _velocity.y -= _playerValuesObject.Gravity * Time.deltaTime;
                }
            }            
        }

        //clamp y velocity
        if (_playerStatusObject.IsSlamAttacking)
        {
            _velocity.y = Mathf.Clamp(_velocity.y, -_playerValuesObject.SlamAttackNegativeTerminalVelocity, Mathf.Infinity);
        }
        else
        {
            _velocity.y = Mathf.Clamp(_velocity.y, -_playerValuesObject.NegativeTerminalVelocity, Mathf.Infinity);
        }        
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

        //play effect for running/walking
        if (!_stepEffect.isPlaying && _playerStatusObject.IsGrounded && Mathf.Abs(_movementInput.x) > 0.0f && Mathf.Abs(_velocity.x) > 0.0f)
        {
            _stepEffect.Play();
        }
        else
        {
            _stepEffect.Stop();
        }

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
        _animator.SetBool("IsCrouching", _playerStatusObject.IsCrouching && _playerStatusObject.IsGrounded);

        //is sliding
        _animator.SetBool("IsSliding", _runInput && _playerStatusObject.IsCrouching && _playerStatusObject.IsGrounded);        
    }

    private void OnMovement(InputValue value)
    {
        _movementInput = value.Get<Vector2>();

        //deadzone
        if (_movementInput.magnitude < _playerValuesObject.JoystickDeadzone)
        {
            _movementInput = Vector2.zero;
        }
    }

    private void OnRun(InputValue value)
    {
        _runInput = value.Get<float>() != 0.0f;
    }

    private void OnCrouch(InputValue value)
    {
        _playerStatusObject.IsCrouching = value.Get<float>() != 0.0f;
    }

    private void OnJump(InputValue value)
    {
        if (_isConflictingInputEnabled)
        {
            _jumpInput = value.Get<float>() != 0.0f;
        }
    }

    private void OnDart()
    {
        if (_gameStatusObject.unlockedDart && _playerStatusObject.HasDartToken && _isConflictingInputEnabled)
        {
            //calculate horizontal velocity
            float boostedVelocity;
            if (_playerStatusObject.IsGrounded && _playerStatusObject.IsFacingRight)
            {
                boostedVelocity = _velocity.x + _playerValuesObject.GroundDartSpeedBoost;

                if (boostedVelocity > _playerValuesObject.GroundDartMaximumSpeed)
                {
                    boostedVelocity = _playerValuesObject.GroundDartMaximumSpeed;
                }
                else if (boostedVelocity < _playerValuesObject.GroundDartMinimumSpeed)
                {
                    boostedVelocity = _playerValuesObject.GroundDartMinimumSpeed;
                }
            }
            else if (_playerStatusObject.IsGrounded)
            {
                boostedVelocity = _velocity.x - _playerValuesObject.GroundDartSpeedBoost;

                if (boostedVelocity < -_playerValuesObject.GroundDartMaximumSpeed)
                {
                    boostedVelocity = -_playerValuesObject.GroundDartMaximumSpeed;
                }
                else if (boostedVelocity > -_playerValuesObject.GroundDartMinimumSpeed)
                {
                    boostedVelocity = -_playerValuesObject.GroundDartMinimumSpeed;
                }
            }
            else if (_playerStatusObject.IsFacingRight)
            {
                boostedVelocity = _velocity.x + _playerValuesObject.AirDartSpeedBoost;

                if (boostedVelocity > _playerValuesObject.AirDartMaximumSpeed)
                {
                    boostedVelocity = _playerValuesObject.AirDartMaximumSpeed;
                }
                else if (boostedVelocity < _playerValuesObject.AirDartMinimumSpeed)
                {
                    boostedVelocity = _playerValuesObject.AirDartMinimumSpeed;
                }
            }
            else
            {
                boostedVelocity = _velocity.x - _playerValuesObject.AirDartSpeedBoost;

                if (boostedVelocity < -_playerValuesObject.AirDartMaximumSpeed)
                {
                    boostedVelocity = -_playerValuesObject.AirDartMaximumSpeed;
                }
                else if (boostedVelocity > -_playerValuesObject.AirDartMinimumSpeed)
                {
                    boostedVelocity = -_playerValuesObject.AirDartMinimumSpeed;
                }
            }

            //calculate vertical velocity
            float verticalVelocity;
            if (_rigidbody.velocity.y < 0.0f)
            {
                verticalVelocity = 0.0f;
            }
            else
            {
                verticalVelocity = _rigidbody.velocity.y;
            }

            //update velocity
            _rigidbody.velocity = new Vector2(boostedVelocity, verticalVelocity);

            //play effect
            if (boostedVelocity >= _playerValuesObject.GroundDartMaximumSpeed && _playerValuesObject.DartSuperFastEffect != null)
            {
                PoolManager.Instance.Spawn(_playerValuesObject.DartSuperFastEffect.name, transform.position, Quaternion.Euler(0.0f, 0.0f, -90.0f));
            }
            else if (boostedVelocity <= -_playerValuesObject.GroundDartMaximumSpeed && _playerValuesObject.DartSuperFastEffect != null)
            {
                PoolManager.Instance.Spawn(_playerValuesObject.DartSuperFastEffect.name, transform.position, Quaternion.Euler(0.0f, 0.0f, 90.0f));
            }
            if (boostedVelocity >= _playerValuesObject.GroundRunMovementSpeed && _playerValuesObject.DartFastEffect != null)
            {
                PoolManager.Instance.Spawn(_playerValuesObject.DartFastEffect.name, transform.position, Quaternion.Euler(0.0f, 0.0f, -90.0f));
            }
            else if (boostedVelocity <= -_playerValuesObject.GroundRunMovementSpeed && _playerValuesObject.DartFastEffect != null)
            {
                PoolManager.Instance.Spawn(_playerValuesObject.DartFastEffect.name, transform.position, Quaternion.Euler(0.0f, 0.0f, 90.0f));
            }
            else if (boostedVelocity > 0.0f && _playerValuesObject.DartSlowEffect != null)
            {
                PoolManager.Instance.Spawn(_playerValuesObject.DartSlowEffect.name, transform.position, Quaternion.Euler(0.0f, 0.0f, -90.0f));
            }
            else if (boostedVelocity < 0.0f && _playerValuesObject.DartSlowEffect != null)
            {
                PoolManager.Instance.Spawn(_playerValuesObject.DartSlowEffect.name, transform.position, Quaternion.Euler(0.0f, 0.0f, 90.0f));
            }

            _dartTimer = _playerValuesObject.DartTime;
            _playerStatusObject.HasDartToken = false;
        }
    }

    public void OnPause()
    {
        //disable conflicting inputs
        _isConflictingInputEnabled = false;
    }

    public void OnResume()
    {
        StartCoroutine(OnResumeDelay());
    }

    private IEnumerator OnResumeDelay()
    {
        //delay
        yield return null;
        
        //enable inputs
        _isConflictingInputEnabled = true;
    }
}
