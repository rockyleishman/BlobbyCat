using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDetector : MonoBehaviour
{
    public bool LedgeCheck(float detectionRange)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0.0f, -detectionRange, 0.0f), Vector2.down, detectionRange, LayerMask.GetMask("Ground"));

        return hit.collider == null;
    }
}
