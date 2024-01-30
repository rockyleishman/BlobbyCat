 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRigidbody;
    private Animator playerAnimator;

    public float moveSpeed;
    public float jumpHeight;

    private float moveVelocity;
    private float moveMultiplier;

    public float groundFriction;
    public float airFriction;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;

    private bool isGrounded;
    private bool isCrouching;
    private bool isLiquid;

    private bool doubleJumpReady;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();

        if (groundFriction > 1f)
        {
            groundFriction = 1f;
        }
        else if (groundFriction < 0f)
        {
            groundFriction = 0f;
        }

        if (airFriction > 1f)
        {
            airFriction = 1f;
        }
        else if (airFriction < 0f)
        {
            airFriction = 0f;
        }

        isCrouching = false;
        isLiquid = false;
    }

    // FixedUpdate is called a consistent amount of times per second (for physics)
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Move Velocity Modification
        if (isGrounded)
        {
            moveVelocity *= (1f - groundFriction);
        }
        else
        {
            moveVelocity *= (1f - airFriction);
        }
        if (moveVelocity < 0.001f && moveVelocity > -0.001f)
        {
            moveVelocity = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Reset Double Jump
        if (isGrounded)
        {
            doubleJumpReady = true;
        }
        else // Un-Liquid in air
        {
            isLiquid = false;
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump(1f);
        }
        
        // Double Jump
        if (Input.GetKeyDown(KeyCode.Space) && doubleJumpReady && !isGrounded)
        {
            Jump(1f);
            doubleJumpReady = false;
        }

        // Crouching
        isCrouching = (Input.GetKey(KeyCode.LeftShift) && isGrounded);

        // Liquid
        if (Input.GetKeyDown(KeyCode.F) && isCrouching && !isLiquid)
        {
            isLiquid = true;
        }
        else if (Input.GetKeyDown(KeyCode.F) && isLiquid)
        {
            isLiquid = false;
        }

        // Movement
            // Move Multiplier
        if (isLiquid)
        {
            moveMultiplier = 0.333f;
        }
        else if (isCrouching)
        {
            moveMultiplier = 0.5f;
        }
        else
        {
            moveMultiplier = 1f;
        }
            // Move Velocity (modified in FixedUpdate())
                // Move Right
        if (Input.GetKey(KeyCode.D))
        {
            moveVelocity = moveSpeed;
        }
                // Move Left
        if (Input.GetKey(KeyCode.A))
        {
            moveVelocity = -moveSpeed;
        }
            // Apply Movement
        playerRigidbody.velocity = new Vector2(moveVelocity * moveMultiplier, playerRigidbody.velocity.y);

        // Determine Direction Player is Facing
        if (Input.GetKey(KeyCode.D) && playerRigidbody.velocity.x > 0f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (Input.GetKey(KeyCode.A) && playerRigidbody.velocity.x < 0f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        // Parameters for Animations
        playerAnimator.SetFloat("Velocity Y", playerRigidbody.velocity.y);
        playerAnimator.SetBool("Is Moving", ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)) && Mathf.Abs(playerRigidbody.velocity.x) > 0.001f));
        playerAnimator.SetBool("Is Grounded", isGrounded);
        playerAnimator.SetBool("Is Crouching", isCrouching);
        playerAnimator.SetBool("Is Liquid", isLiquid);
    }

    // Jump
    public void Jump(float multiplier)
    {
        playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, (jumpHeight * multiplier));
    }
}
