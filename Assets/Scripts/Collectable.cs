using System.Collections;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    internal bool PreviouslyCollected;
    internal bool CurrentlyCollected;

    [SerializeField] public float FloatHeight = 0.5f;
    [SerializeField] public float FloatSpeed = 2.0f;
    private Vector2 _velocity;

    private void Start()
    {
        _velocity = Vector2.zero;
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
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        Ground ground = other.GetComponent<Ground>();

        if (ground != null)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, FloatHeight, LayerMask.GetMask("Ground"));

            if (hit.collider != null)
            {
                //stop physics
                StopAllCoroutines();

                //move towards new position
                StartCoroutine(OnGround(hit));
            }
            else
            {
                //reverse x velocity
                _velocity.x = -_velocity.x;
            }
        }
    }
}