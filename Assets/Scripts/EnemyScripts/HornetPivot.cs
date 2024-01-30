using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HornetPivot : MonoBehaviour
{
    public Rigidbody2D pivotRigidbody;

    public float horizontalMoveSpeed;
    public float verticalMoveSpeed;
    public bool isMovingRight;
    public bool isMovingUp;

    public Transform flyAreaCheckTop;
    public Transform flyAreaCheckTopRight;
    public Transform flyAreaCheckRight;
    public Transform flyAreaCheckBottomRight;
    public Transform flyAreaCheckBottom;
    public Transform flyAreaCheckBottomLeft;
    public Transform flyAreaCheckLeft;
    public Transform flyAreaCheckTopLeft;
    public float flyAreaCheckRadius;
    public LayerMask flyAreaLayer;
    public LayerMask groundLayer;

    private bool isInAreaRight;
    private bool isInAreaLeft;
    private bool isInAreaTop;
    private bool isInAreaBottom;

    // Start is called before the first frame update
    void Start()
    {
        pivotRigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        isInAreaRight = (Physics2D.OverlapCircle(flyAreaCheckTopRight.position, flyAreaCheckRadius, flyAreaLayer)
            && !Physics2D.OverlapCircle(flyAreaCheckTopRight.position, flyAreaCheckRadius, groundLayer)
            && Physics2D.OverlapCircle(flyAreaCheckRight.position, flyAreaCheckRadius, flyAreaLayer)
            && !Physics2D.OverlapCircle(flyAreaCheckRight.position, flyAreaCheckRadius, groundLayer)
            && Physics2D.OverlapCircle(flyAreaCheckBottomRight.position, flyAreaCheckRadius, flyAreaLayer)
            && !Physics2D.OverlapCircle(flyAreaCheckBottomRight.position, flyAreaCheckRadius, groundLayer));

        isInAreaLeft = (Physics2D.OverlapCircle(flyAreaCheckBottomLeft.position, flyAreaCheckRadius, flyAreaLayer)
            && !Physics2D.OverlapCircle(flyAreaCheckBottomLeft.position, flyAreaCheckRadius, groundLayer)
            && Physics2D.OverlapCircle(flyAreaCheckLeft.position, flyAreaCheckRadius, flyAreaLayer)
            && !Physics2D.OverlapCircle(flyAreaCheckLeft.position, flyAreaCheckRadius, groundLayer)
            && Physics2D.OverlapCircle(flyAreaCheckTopLeft.position, flyAreaCheckRadius, flyAreaLayer)
            && !Physics2D.OverlapCircle(flyAreaCheckTopLeft.position, flyAreaCheckRadius, groundLayer));

        isInAreaTop = (Physics2D.OverlapCircle(flyAreaCheckTopLeft.position, flyAreaCheckRadius, flyAreaLayer)
            && !Physics2D.OverlapCircle(flyAreaCheckTopLeft.position, flyAreaCheckRadius, groundLayer)
            && Physics2D.OverlapCircle(flyAreaCheckTop.position, flyAreaCheckRadius, flyAreaLayer) 
            && !Physics2D.OverlapCircle(flyAreaCheckTop.position, flyAreaCheckRadius, groundLayer)
            && Physics2D.OverlapCircle(flyAreaCheckTopRight.position, flyAreaCheckRadius, flyAreaLayer)
            && !Physics2D.OverlapCircle(flyAreaCheckTopRight.position, flyAreaCheckRadius, groundLayer));

        isInAreaBottom = (Physics2D.OverlapCircle(flyAreaCheckBottomRight.position, flyAreaCheckRadius, flyAreaLayer) 
            && !Physics2D.OverlapCircle(flyAreaCheckBottomRight.position, flyAreaCheckRadius, groundLayer)
            && Physics2D.OverlapCircle(flyAreaCheckBottom.position, flyAreaCheckRadius, flyAreaLayer)
            && !Physics2D.OverlapCircle(flyAreaCheckBottom.position, flyAreaCheckRadius, groundLayer)
            && Physics2D.OverlapCircle(flyAreaCheckBottomLeft.position, flyAreaCheckRadius, flyAreaLayer)
            && !Physics2D.OverlapCircle(flyAreaCheckBottomLeft.position, flyAreaCheckRadius, groundLayer));
    }

    // Update is called once per frame
    void Update()
    {
        // Flip on Fly Area Boundry collision
        if (!isInAreaRight)
        {
            isMovingRight = false;
        }
        else if (!isInAreaLeft)
        {
            isMovingRight = true;
        }
        if (!isInAreaTop)
        {
            isMovingUp = false;
        }
        else if (!isInAreaBottom)
        {
            isMovingUp = true;
        }

        // Move
        if (isMovingRight && isMovingUp)
        {
            pivotRigidbody.velocity = new Vector2(horizontalMoveSpeed, verticalMoveSpeed);
        }
        else if (isMovingRight)
        {
            pivotRigidbody.velocity = new Vector2(horizontalMoveSpeed, -verticalMoveSpeed);
        }
        else if (isMovingUp)
        {
            pivotRigidbody.velocity = new Vector2(-horizontalMoveSpeed, verticalMoveSpeed);
        }
        else
        {
            pivotRigidbody.velocity = new Vector2(-horizontalMoveSpeed, -verticalMoveSpeed);
        }
    }
}
