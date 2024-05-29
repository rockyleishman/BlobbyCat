using System.Collections;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private PlayerValues _playerValuesObject;
    private PlayerStatus _playerStatusObject;
    protected LevelCollectionData _levelCollectionData;
    protected Animator _animator;
    protected SpriteRenderer _renderer;

    internal bool PreviouslyCollected;
    internal bool CurrentlyCollected;

    [SerializeField] public Effect CollectEffect;

    [SerializeField] public float FloatHeight = 0.5f;
    [SerializeField] public float FloatSpeed = 2.0f;
    private Vector2 _velocity;
    private bool _isBeingSucked;
    private bool _isFloatingUp;

    protected virtual void Start()
    {
        //init fields
        _playerValuesObject = DataManager.Instance.PlayerValuesObject;
        _playerStatusObject = DataManager.Instance.PlayerStatusObject;
        _levelCollectionData = DataManager.Instance.LevelCollectionDataObject;
        _animator = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
        _velocity = Vector2.zero;
        _isBeingSucked = false;
        _isFloatingUp = false;

        //start in floating animation state
        _animator.SetBool("IsFloating", true);

        //randomize orientation
        _renderer.flipX = Random.value > 0.5f;
    }

    public void Launch(Vector2 velocity, float gravity, float xResistance, float terminalVelocity)
    {
        StartCoroutine(OnLaunch(velocity, gravity, xResistance, terminalVelocity));
    }

    private IEnumerator OnLaunch(Vector2 velocity, float gravity, float xResistance, float terminalVelocity)
    {
        _velocity = velocity;

        while (true)
        {
            //adjust velocity
            if (_velocity.x > 0.0f)
            {
                _velocity = new Vector2(Mathf.Clamp(_velocity.x - xResistance * Time.deltaTime, 0.0f, Mathf.Infinity), Mathf.Clamp(_velocity.y - gravity * Time.deltaTime, -terminalVelocity, Mathf.Infinity));
            }
            else if (_velocity.x < 0.0f)
            {
                _velocity = new Vector2(Mathf.Clamp(_velocity.x + xResistance * Time.deltaTime, -Mathf.Infinity, 0.0f), Mathf.Clamp(_velocity.y - gravity * Time.deltaTime, -terminalVelocity, Mathf.Infinity));
            }
            else
            {
                _velocity.y = Mathf.Clamp(_velocity.y - gravity * Time.deltaTime, -terminalVelocity, Mathf.Infinity);
            }

            //move
            transform.position = transform.position + new Vector3(_velocity.x, _velocity.y, 0.0f) * Time.deltaTime;

            yield return null;
        }
    }

    private IEnumerator OnGround(RaycastHit2D firstHit)
    {
        _isFloatingUp = true;

        RaycastHit2D hit = firstHit;

        while (hit.distance < FloatHeight)
        {
            //float up
            transform.position = transform.position + new Vector3(0.0f, FloatSpeed, 0.0f) * Time.deltaTime;

            //update raycast hit
            hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Ground"));
            yield return null;
        }

        transform.position = new Vector2(transform.position.x, hit.point.y + FloatHeight);

        _isFloatingUp = false;
    }

    public void Suck()
    {
        if (!_isBeingSucked)
        {
            StopAllCoroutines();
            _isFloatingUp = false;
            _isBeingSucked = true;
            StartCoroutine(OnSuck());
        }
    }

    private IEnumerator OnSuck()
    {
        float suctionTimer = 0.0f;

        while (true)
        {
            suctionTimer += Time.deltaTime;

            transform.position = Vector3.Lerp(transform.position, _playerStatusObject.Player.transform.position, Mathf.Clamp01(suctionTimer * _playerValuesObject.TreatSuction));

            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //check for ground
        Ground ground = other.GetComponent<Ground>();

        if (ground != null)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, FloatHeight, LayerMask.GetMask("Ground"));

            if (hit.collider != null)
            {
                //don't stop if being sucked or already floating up from ground hit
                if (!_isBeingSucked && !_isFloatingUp)
                {
                    //stop physics
                    StopAllCoroutines();

                    //move towards new position
                    StartCoroutine(OnGround(hit));
                }
            }
            else
            {
                //reverse x velocity
                _velocity.x = -_velocity.x;
            }
        }

        //check for player
        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            //stop physics
            StopAllCoroutines();

            //collect
            Collect();
        }
    }

    protected virtual void Collect()
    {
        //effects
        PoolManager.Instance.Spawn(CollectEffect.name, transform.position, transform.rotation);

        //TODO: collect via manager
        StopAllCoroutines();
        Destroy(gameObject);
    }
}