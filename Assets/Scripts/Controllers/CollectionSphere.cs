using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionSphere : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        Collectable collectable = other.GetComponent<Collectable>();

        if (collectable != null)
        {
            collectable.Suck();
        }
    }
}
