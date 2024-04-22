using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetector : MonoBehaviour
{
    public bool WallCheck(float detectionRange, bool isCheckingRightSide)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, detectionRange, LayerMask.GetMask("Ground"));

        return hit.collider != null;
    }
}
