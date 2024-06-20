using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingGrate : MonoBehaviour
{
    private PlayerStatus _playerStatusObject;

    [SerializeField] public Sprite[] RandomizedSprites;

    [SerializeField] public float TimeBeforeFall = 0.25f;
    [SerializeField] public float Gravity = 20.0f;
    [SerializeField] public float TerminalVelocity = 20.0f;
    [SerializeField] public float GroundDetectionYOrigin = 0.0f;

    private bool _isFalling;

    private void Start()
    {
        //init fields
        _playerStatusObject = DataManager.Instance.PlayerStatusObject;
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
        float velocity = 0.0f;
        while (_isFalling)
        {
            velocity = Mathf.Clamp(velocity - Gravity * Time.deltaTime, -TerminalVelocity, 0.0f);
            float deltaY = velocity * Time.deltaTime;

            RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0.0f, GroundDetectionYOrigin, 0.0f), Vector2.down, -deltaY, LayerMask.GetMask("Ground"));
            if (hit.collider != null)
            {
                //end fall
                //TODO: effects
                _isFalling = false;
                Destroy(gameObject);
            }
            else
            {
                //continue fall
                transform.position = new Vector3(transform.position.x, transform.position.y + deltaY, 0.0f);
            }            

            yield return null;
        }        
    }
}
