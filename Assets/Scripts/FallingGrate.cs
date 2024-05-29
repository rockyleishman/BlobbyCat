using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingGrate : MonoBehaviour
{
    private PlayerStatus _playerStatusObject;

    [SerializeField] public Sprite[] RandomizedSprites;

    [SerializeField] public float TimeBeforeFall = 0.25f;
    [SerializeField] public float Gravity = 20.0f;
    [SerializeField] public float TerminalVelocity = 15f;

    private Collider2D _collider;
    private Rigidbody2D _rigidbody;

    private bool _isFalling;

    private void Start()
    {
        //init fields
        _playerStatusObject = DataManager.Instance.PlayerStatusObject;
        _collider = GetComponent<Collider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _isFalling = false;

        //randomize sprite
        GetComponent<SpriteRenderer>().sprite = RandomizedSprites[Random.Range(0, RandomizedSprites.Length)];
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!_isFalling && collision.collider.GetComponent<Player>() != null && _playerStatusObject.IsGrounded)
        {
            StartCoroutine(Fall());
        }
    }

    private IEnumerator Fall()
    {
        _isFalling = true;

        //wait to fall
        yield return new WaitForSeconds(TimeBeforeFall);

        //fall
        while (!_collider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            _rigidbody.velocity = new Vector2(0.0f, Mathf.Clamp(_rigidbody.velocity.y - Gravity * Time.deltaTime, -TerminalVelocity, 0.0f));

            yield return null;
        }

        //end fall
        //TODO: effects
        Destroy(gameObject);
    }
}
