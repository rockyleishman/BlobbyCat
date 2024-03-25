using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HornetA : MonoBehaviour
{
    private Rigidbody2D enemyRigidbody;
    private Animator enemyAnimator;

    public HornetPivot pivot;
    public float radius;

    public float orbitalSpeed;
    public bool clockwiseMovement;

    private Vector3 orbitAngle;

    public float spriteFlipBuffer;

    // Start is called before the first frame update
    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();

        orbitAngle = (transform.position - pivot.transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 oldPosition = transform.position;

        // Move around Anchor
        transform.position = pivot.transform.position + (orbitAngle * radius);

        if (clockwiseMovement)
        {
            transform.RotateAround(pivot.transform.position, new Vector3(0, 0, -1), orbitalSpeed * Time.deltaTime);
        }
        else
        {
            transform.RotateAround(pivot.transform.position, new Vector3(0, 0, 1), orbitalSpeed * Time.deltaTime);
        }

        orbitAngle = (transform.position - pivot.transform.position).normalized;

        // Determine Direction Enemy is Facing
        if (transform.position.x - oldPosition.x < -spriteFlipBuffer)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (transform.position.x - oldPosition.x > spriteFlipBuffer)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        // Keep Enemy Sprite Upright
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }
}
