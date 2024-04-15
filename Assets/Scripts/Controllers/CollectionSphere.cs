using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionSphere : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Treat treat = other.GetComponent<Treat>();

        if (treat != null)
        {
            treat.Suck();
        }
    }
}
