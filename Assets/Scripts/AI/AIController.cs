using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _renderer;
    private Animator _animator;
    private AIState _currentState;

    [Header("Basic Settings")]
    [SerializeField] public Collider2D MovementArea;
    [SerializeField] public float MovementSpeed = 3.0f;
    [SerializeField] public float AggroSpeed = 4.0f;
    [SerializeField] public float StunRecoveryMultiplier = 1.0f;
    private float _stunTime;
    [SerializeField] public bool CanJump = false;
    [SerializeField] public bool StartFacingRight = true;
    [SerializeField] public WallDetector LeftWallDetector;
    [SerializeField] public WallDetector RightWallDetector;
    [SerializeField] public float WallDetectionRange = 0.0625f;

    [Header("Player Detection")]
    [SerializeField] public bool IsPassive = false;
    [SerializeField] public AIDetectionType DetectionType;
    [SerializeField] public Collider2D SightCollider;
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
        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _currentState = AIState.Idle;
        _stunTime = 0.0f;
        _renderer.flipX = !StartFacingRight;
        _roamSpeed = MovementSpeed;
        _roamTimer = 0.0f;
        _patrolIndex = 0;

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
            _animator.SetTrigger("Idle1");
        }
        else
        {
            _animator.SetTrigger("Idle2");
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
        _animator.SetTrigger("Stun");

        //stun idle
        _rigidbody.velocity = Vector2.zero;
        yield return new WaitForSeconds(_stunTime);

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

    private IEnumerator OnRoam()
    {
        //animate
        _animator.SetTrigger("Run");

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

            //check for walls
            if (WallCheck())
            {
                _renderer.flipX = !_renderer.flipX;
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
        _animator.SetTrigger("Run");

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

    public void Stun(float time)
    {
        _stunTime = time / StunRecoveryMultiplier;
        SetState(AIState.Stunned);
    }

    private bool WallCheck()
    {
        if (_renderer.flipX)
        {
            return LeftWallDetector.WallCheck(WallDetectionRange, false);
        }
        else
        {
            return RightWallDetector.WallCheck(WallDetectionRange, true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other == MovementArea.GetComponent<Collider2D>())
        {
            //turn around
            _renderer.flipX = !_renderer.flipX;
        }
    }
}
