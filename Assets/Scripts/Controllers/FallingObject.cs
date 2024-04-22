using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Collider2D _collider;

    [SerializeField] public float Gravity = 15.0f;
    [SerializeField] public float TerminalVelocity = 7.5f;
    [SerializeField] public float GroundDetectionRange = 0.0625f;
    private float _raycastDistance;
    private Vector3 _localRaycastOrigin;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _raycastDistance = GroundDetectionRange / 2.0f;
        _localRaycastOrigin = new Vector3(0.0f, -_collider.bounds.extents.y - _raycastDistance, 0.0f);
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + _localRaycastOrigin, Vector2.down, _raycastDistance, LayerMask.GetMask("Ground"));
        
        if (hit.collider == null)
        {
            _rigidbody.velocity += new Vector2(0.0f, -Gravity) * Time.deltaTime;
            _rigidbody.velocity = Vector2.ClampMagnitude(_rigidbody.velocity, TerminalVelocity);
        }
        else
        {
            _rigidbody.velocity = Vector2.zero;
        }
    }
}
