using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectTreat : MonoBehaviour
{
    public int value;

    void OnTriggerEnter2D(Collider2D player)
    {
        if (player.GetComponent<PlayerController>() != null)
        {
            ScoreManager.Add(value);

            Destroy(gameObject);
        }
    }
}
