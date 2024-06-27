using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnBall : MonoBehaviour
{
    [SerializeField] public Effect CollectEffect;
    [SerializeField] public ParticleSystem ParticleSystem;

    [SerializeField] public float YarnLength = 30.0f;
    [SerializeField] public float GroundDetectionRange = 0.0625f;
    [SerializeField] public float DistanceFromWalls = 0.5f;
    [SerializeField] public float AccelerationFromWalls = 0.5f;
    [SerializeField] public float AnimationSpeedHalflife = 1.0f;
    [SerializeField] public float Stage0Radius = 0.25f;
    [SerializeField] public float Stage1Radius = 0.21875f;
    [SerializeField] public float Stage2Radius = 0.1875f;
    [SerializeField] public float Stage3Radius = 0.15625f;
    [SerializeField] public float Stage4Radius = 0.125f;
    [SerializeField] public float Stage5Radius = 0.09375f;

    private CircleCollider2D _collider;
    private Rigidbody2D _rigidbody;
    private Animator _animator;

    private float _lengthUnravelled;
    private int _stage;
    private bool _isGrounded;

    private void Start()
    {
        //init fields
        _collider = GetComponent<CircleCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _lengthUnravelled = 0.0f;
        _stage = 0;
        _isGrounded = false;

        //init collider size
        _collider.radius = Stage0Radius;
        _collider.offset = new Vector2(0.0f, 0.0f);
    }

    private void Update()
    {
        //check for ground
        RaycastHit2D hitGround = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y) + Vector2.down * (Stage0Radius + GroundDetectionRange / 2), Vector2.down, GroundDetectionRange / 2, LayerMask.GetMask("Ground"));
        _isGrounded = hitGround.collider != null;

        //check for obstacles
        RaycastHit2D hitLeft = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y) + Vector2.left * (_collider.radius + GroundDetectionRange / 2), Vector2.left, DistanceFromWalls - GroundDetectionRange / 2, LayerMask.GetMask("Ground"));
        RaycastHit2D hitRight = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y) + Vector2.right * (_collider.radius + GroundDetectionRange / 2), Vector2.right, DistanceFromWalls - GroundDetectionRange / 2, LayerMask.GetMask("Ground"));
        if (hitLeft.collider != null && hitRight.collider == null)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x + AccelerationFromWalls * Time.deltaTime, _rigidbody.velocity.y);
        }
        else if (hitRight.collider != null && hitLeft.collider == null)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x - AccelerationFromWalls * Time.deltaTime, _rigidbody.velocity.y);
        }        

        //update animator velocity
        if (_isGrounded)
        {
            _animator.SetFloat("Velocity", _rigidbody.velocity.x);
        }
        else
        {
            _animator.SetFloat("Velocity", Mathf.Lerp(_animator.GetFloat("Velocity"), 0.0f, AnimationSpeedHalflife * Time.deltaTime));
        }

        //unravel yarn
        _lengthUnravelled += _rigidbody.velocity.magnitude * Time.deltaTime;

        //check yarn stage
        if (_stage == 5 && _lengthUnravelled >= YarnLength)
        {
            Collect();
        }
        else if (_stage == 4 && _lengthUnravelled >= YarnLength * 5 / 6)
        {
            _stage = 5;

            _collider.radius = Stage5Radius;
            _collider.offset = new Vector2(0.0f, Stage5Radius - Stage0Radius);

            ParticleSystem.transform.localPosition = new Vector3(0.0f, Stage5Radius - Stage0Radius, 0.0f);

            _animator.SetInteger("Stage", 5);
        }
        else if (_stage == 3 && _lengthUnravelled >= YarnLength * 4 / 6)
        {
            _stage = 4;

            _collider.radius = Stage4Radius;
            _collider.offset = new Vector2(0.0f, Stage4Radius - Stage0Radius);

            ParticleSystem.transform.localPosition = new Vector3(0.0f, Stage4Radius - Stage0Radius, 0.0f);

            _animator.SetInteger("Stage", 4);
        }
        else if (_stage == 2 && _lengthUnravelled >= YarnLength * 3 / 6)
        {
            _stage = 3;

            _collider.radius = Stage3Radius;
            _collider.offset = new Vector2(0.0f, Stage3Radius - Stage0Radius);

            ParticleSystem.transform.localPosition = new Vector3(0.0f, Stage3Radius - Stage0Radius, 0.0f);

            _animator.SetInteger("Stage", 3);
        }
        else if (_stage == 1 && _lengthUnravelled >= YarnLength * 2 / 6)
        {
            _stage = 2;

            _collider.radius = Stage2Radius;
            _collider.offset = new Vector2(0.0f, Stage2Radius - Stage0Radius);

            ParticleSystem.transform.localPosition = new Vector3(0.0f, Stage2Radius - Stage0Radius, 0.0f);

            _animator.SetInteger("Stage", 2);
        }
        else if (_stage == 0 && _lengthUnravelled >= YarnLength * 1 / 6)
        {
            _stage = 1;

            _collider.radius = Stage1Radius;
            _collider.offset = new Vector2(0.0f, Stage1Radius - Stage0Radius);

            ParticleSystem.transform.localPosition = new Vector3(0.0f, Stage1Radius - Stage0Radius, 0.0f);

            _animator.SetInteger("Stage", 1);
        }
    }

    private void Collect()
    {
        //effects
        PoolManager.Instance.Spawn(CollectEffect.name, transform.position, transform.rotation);

        //TODO: collect via manager
        Destroy(gameObject);
    }

    public void AttackHit(Vector3 attackPosition, float deltaSpeed)
    {
        if (transform.position.x - attackPosition.x > 0.0f)
        {
            //roll right
            _rigidbody.velocity += new Vector2(deltaSpeed, 0.0f);
        }
        else
        {
            //roll left
            _rigidbody.velocity += new Vector2(-deltaSpeed, 0.0f);
        }
    }
}
