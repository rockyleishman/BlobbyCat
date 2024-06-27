using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyAnimationState
{
    Idle1,
    Idle2,
    Move1,
    Move2,
    AttackA,
    AttackB,
    AttackC,
    AttackD
}

public enum AIState
{
    Idle,
    Stunned,
    Roam,
    Patrol,
    Aggro,
    AttackA,
    AttackB,
    AttackC,
    AttackD
}

public enum AIDetectionType
{
    Idle,
    Roaming,
    Patrolling
}

public class AIController : MonoBehaviour
{
    protected Rigidbody2D _rigidbody;
    private Collider2D _collider;
    protected SpriteRenderer _renderer;
    protected Animator _animator;
    private AttackTrigger[] _attackTriggers;
    protected AIState _currentState;

    [Header("Basic Settings")]
    private Collider2D _movementArea;
    [SerializeField] public float MovementSpeed = 3.0f;
    [SerializeField] public float AggroSpeed = 4.0f;
    [SerializeField] public float StunRecoveryMultiplier = 1.0f;
    private float _stunTime;
    [SerializeField] public float KnockbackDistance;
    [SerializeField] public float KnockbackSpeed;
    private bool _isBeingKnockedBack;
    [SerializeField] public bool CanJump = false;
    [SerializeField] public bool StartFacingRight = true;
    [SerializeField] public WallDetector LeftWallDetector;
    [SerializeField] public WallDetector RightWallDetector;
    [SerializeField] public LedgeDetector LeftLedgeDetector;
    [SerializeField] public LedgeDetector RightLedgeDetector;
    [SerializeField] public float DetectionRange = 0.0625f;
    [SerializeField] public bool CollidesWithPlayer = true;
    [SerializeField] public bool CollidesWithEnemies = true;

    [Header("Player Detection")]
    [SerializeField] public bool IsPassive = false;
    [SerializeField] public AIDetectionType DetectionType;
    [SerializeField] public float SightDetectionRange = 8.0f;
    [SerializeField] public float AudioDetectionRange = 4.0f;

    [Header("Roam")]
    [SerializeField, Range(0.0f, 1.0f)] public float MinRandomSpeedMultiplier = 0.75f;
    private float _roamSpeed;
    [SerializeField] public float MinRoamTime = 3.0f;
    [SerializeField] public float MaxRoamTime = 7.0f;
    private float _roamTimer;
    [SerializeField] public float MinIdleTime = 1.0f;
    [SerializeField] public float MaxIdleTime = 5.0f;

    [Header("Patrol")]
    [SerializeField] public PatrolPoint[] PatrolPoints;
    private int _patrolIndex;
    [SerializeField] public float PatrolPointIdleTime = 1.0f;
    [SerializeField] public float PatrolPointDetectionRadius = 0.5f;

    private void Start()
    {
        //init fields
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _attackTriggers = GetComponentsInChildren<AttackTrigger>();
        _currentState = AIState.Idle;
        _stunTime = 0.0f;
        _isBeingKnockedBack = false;
        _renderer.flipX = !StartFacingRight;
        _roamSpeed = MovementSpeed;
        _roamTimer = 0.0f;
        _patrolIndex = 0;

        if (transform.parent != null)
        {
            _movementArea = transform.parent.GetComponent<Collider2D>();
        }

        //enable collision with player
        if (CollidesWithPlayer)
        {
            GetComponent<Collider2D>().forceSendLayers = LayerMask.GetMask("Player");
        }

        //set initial state
        SetState(AIState.Idle);
    }

    private void SetState(AIState newState)
    {
        //change state
        _currentState = newState;

        //end all coroutines
        StopAllCoroutines();

        //start new state coroutine
        switch (_currentState)
        {
            case AIState.Stunned:
                StartCoroutine(OnStunned());
                break;

            case AIState.Roam:
                StartCoroutine(OnRoam());
                break;

            case AIState.Patrol:
                StartCoroutine(OnPatrol());
                break;

            case AIState.Aggro:
                StartCoroutine(OnAggro());
                break;

            case AIState.AttackA:
                StartCoroutine(OnAttackA());
                break;

            case AIState.AttackB:
                StartCoroutine(OnAttackB());
                break;

            case AIState.AttackC:
                StartCoroutine(OnAttackC());
                break;

            case AIState.AttackD:
                StartCoroutine(OnAttackD());
                break;

            case AIState.Idle:
            default:
                StartCoroutine(OnIdle());
                break;
        }
    }

    private IEnumerator OnIdle()
    {
        //animate
        if (Random.value >= 0.5f)
        {
            _animator.SetInteger("State", (int)EnemyAnimationState.Idle1);
        }
        else
        {
            _animator.SetInteger("State", (int)EnemyAnimationState.Idle2);
        }

        //idle
        _rigidbody.velocity = Vector2.zero;

        if (DetectionType == AIDetectionType.Roaming)
        {
            //idle
            yield return new WaitForSeconds(Random.Range(MinIdleTime, MaxIdleTime));

            //roam
            SetState(AIState.Roam);
        }
        else if (DetectionType == AIDetectionType.Patrolling && PatrolPoints.Length >= 2)
        {
            //idle
            yield return new WaitForSeconds(PatrolPointIdleTime);

            //patrol
            SetState(AIState.Patrol);
        }

        while (true)
        {
            //idle (wait for next SetState() call)

            yield return null;
        }        
    }

    private IEnumerator OnStunned()
    {
        //animate
        _animator.SetBool("Hurt", true);

        //disable attached attack triggers
        foreach (AttackTrigger attackTrigger in _attackTriggers)
        {
            attackTrigger.gameObject.SetActive(false);
        }

        //stun idle/knockback
        if (_isBeingKnockedBack)
        {
            StartCoroutine(OnKnockback());
        }
        else
        {
            _rigidbody.velocity = Vector2.zero;
        }
        yield return new WaitForSeconds(_stunTime);

        //enable attached attack triggers
        foreach (AttackTrigger attackTrigger in _attackTriggers)
        {
            attackTrigger.gameObject.SetActive(true);
        }

        //end animation
        _animator.SetBool("Hurt", false);

        if (DetectionType == AIDetectionType.Roaming)
        {
            //roam
            SetState(AIState.Roam);
        }
        else if (DetectionType == AIDetectionType.Patrolling && PatrolPoints.Length >= 2)
        {
            //patrol
            SetState(AIState.Patrol);
        }
    }

    private IEnumerator OnKnockback()
    {
        float knockbackTime = KnockbackDistance / KnockbackSpeed;
        int direction;
        if (transform.position.x >= DataManager.Instance.PlayerStatusObject.Player.transform.position.x)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }

        while (knockbackTime > 0.0f)
        {
            knockbackTime -= Time.deltaTime;
            _rigidbody.velocity = new Vector2(KnockbackSpeed * direction, 0.0f);
            yield return null;
        }

        _rigidbody.velocity = Vector2.zero;
    }

    private IEnumerator OnRoam()
    {
        //animate
        _animator.SetInteger("State", (int)EnemyAnimationState.Move1);

        //set roam timer, speed, and direction
        _roamTimer = Random.Range(MinRoamTime, MaxRoamTime);
        _roamSpeed = Random.Range(MovementSpeed * MinRandomSpeedMultiplier, MovementSpeed);
        if (Random.value >= 0.5)
        {
            _renderer.flipX = !_renderer.flipX;
        }

        //roam
        while (_roamTimer > 0.0f)
        {
            //decrement roam timer
            _roamTimer -= Time.deltaTime;

            //check for walls & ledges
            if (WallCheck() || LedgeCheck())
            {
                _renderer.flipX = !_renderer.flipX;
            }

            //check for player in path
            if (PlayerCheck())
            {
                //idle
                break;
            }

            //roam
            if (_renderer.flipX)
            {
                _rigidbody.velocity = new Vector2(-_roamSpeed, 0.0f);
            }
            else
            {
                _rigidbody.velocity = new Vector2(_roamSpeed, 0.0f);
            }

            //keep roaming
            yield return null;
        }

        //idle
        SetState(AIState.Idle);
    }

    private IEnumerator OnPatrol()
    {
        //animate
        _animator.SetInteger("State", (int)EnemyAnimationState.Move1);

        while (true)
        {
            if (Mathf.Abs(PatrolPoints[_patrolIndex].transform.position.x - transform.position.x) < PatrolPointDetectionRadius)
            {
                //change patrol point
                _patrolIndex++;
                if (_patrolIndex >= PatrolPoints.Length)
                {
                    _patrolIndex = 0;
                }

                //idle at point
                _renderer.flipX = !PatrolPoints[_patrolIndex].IsFacingRight;
                SetState(AIState.Idle);

            }
            else if (PatrolPoints[_patrolIndex].transform.position.x > transform.position.x)
            {
                //go right
                _renderer.flipX = false;
                _rigidbody.velocity = new Vector2(MovementSpeed, 0.0f);
            }
            else
            {
                //go left
                _renderer.flipX = true;
                _rigidbody.velocity = new Vector2(-MovementSpeed, 0.0f);
            }

            yield return null;
        }        
    }

    protected virtual IEnumerator OnAggro()
    {
        //to be implemented in subclasses that use attacks
        SetState(AIState.Idle);
        yield return null;
    }

    protected virtual IEnumerator OnAttackA()
    {
        //to be implemented in subclasses that use at least 1 attack
        SetState(AIState.Idle);
        yield return null;
    }

    protected virtual IEnumerator OnAttackB()
    {
        //to be implemented in subclasses that use at least 2 attacks
        SetState(AIState.Idle);
        yield return null;
    }

    protected virtual IEnumerator OnAttackC()
    {
        //to be implemented in subclasses that use at least 3 attacks
        SetState(AIState.Idle);
        yield return null;
    }

    protected virtual IEnumerator OnAttackD()
    {
        //to be implemented in subclasses that use 4 attacks
        SetState(AIState.Idle);
        yield return null;
    }

    public void Hurt(float time)
    {
        _stunTime = time;
        _isBeingKnockedBack = true;
        SetState(AIState.Stunned);
    }

    public void Stun(float time)
    {
        _stunTime = time / StunRecoveryMultiplier;
        _isBeingKnockedBack = false;
        SetState(AIState.Stunned);
    }

    private bool WallCheck()
    {
        if (_renderer.flipX)
        {
            if (CollidesWithEnemies)
            {
                return LeftWallDetector.WallCheck(DetectionRange) || Physics2D.Raycast(_collider.bounds.center - _collider.bounds.extents - new Vector3(DetectionRange, 0.0f, 0.0f), Vector2.up, _collider.bounds.size.y, LayerMask.GetMask("Enemy")).collider != null;
            }
            else
            {
                return LeftWallDetector.WallCheck(DetectionRange);
            }            
        }
        else
        {
            if (CollidesWithEnemies)
            {
                return RightWallDetector.WallCheck(DetectionRange) || Physics2D.Raycast(_collider.bounds.center + _collider.bounds.extents + new Vector3(DetectionRange, 0.0f, 0.0f), Vector2.down, _collider.bounds.size.y, LayerMask.GetMask("Enemy")).collider != null;
            }
            else
            {
                return RightWallDetector.WallCheck(DetectionRange);
            }
        }
    }

    private bool LedgeCheck()
    {
        if (_renderer.flipX)
        {
            return LeftLedgeDetector.LedgeCheck(DetectionRange);
        }
        else
        {
            return RightLedgeDetector.LedgeCheck(DetectionRange);
        }
    }

    private bool PlayerCheck()
    {
        if (CollidesWithPlayer && _renderer.flipX)
        {
            return LeftWallDetector.WallCheck(DetectionRange) || Physics2D.Raycast(_collider.bounds.center - _collider.bounds.extents - new Vector3(DetectionRange, 0.0f, 0.0f), Vector2.up, _collider.bounds.size.y, LayerMask.GetMask("Player")).collider != null;
        }
        else if (CollidesWithPlayer)
        {
            return RightWallDetector.WallCheck(DetectionRange) || Physics2D.Raycast(_collider.bounds.center + _collider.bounds.extents + new Vector3(DetectionRange, 0.0f, 0.0f), Vector2.down, _collider.bounds.size.y, LayerMask.GetMask("Player")).collider != null;
        }
        else
        {
            return false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_movementArea != null && other == _movementArea)
        {
            //turn around
            _renderer.flipX = !_renderer.flipX;
        }
    }
}
