using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTrigger : MonoBehaviour
{
    private int _waterDamage;

    private void Start()
    {
        //init fields
        _waterDamage = DataManager.Instance.PlayerValuesObject.DamageFromWater;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHitPointController player = other.GetComponent<PlayerHitPointController>();

        if (player != null)
        {
            //deal damage
            player.Damage(_waterDamage);

            //respawn at minor checkpoint
            GameManager.Instance.DamageRespawn();
        }
    }
}
