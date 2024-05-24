using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionSphere : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Collectable collectable = other.GetComponent<Collectable>();

        if (collectable != null)
        {
            collectable.Suck();
        }
    }
}
