using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    [SerializeField] public int Damage = 1;

    private void OnTriggerStay2D(Collider2D other)
    {
        PlayerHitPointController player = other.GetComponent<PlayerHitPointController>();

        if (player != null)
        {
            //deal damage
            player.Damage(Damage);
        }
    }
}
