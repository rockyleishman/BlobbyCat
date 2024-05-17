using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoxHeadHitbox : MonoBehaviour
{
    private PlayerValues _playerValuesObject;
    private PlayerStatus _playerStatusObject;

    private void Start()
    {
        //init fields
        _playerValuesObject = DataManager.Instance.PlayerValuesObject;
        _playerStatusObject = DataManager.Instance.PlayerStatusObject;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        FallingBoxSafe safeBox = collider.GetComponent<FallingBoxSafe>();
        FallingBoxDangerous dangerousBox = collider.GetComponent<FallingBoxDangerous>();

        if (safeBox != null)
        {
            //break box
            safeBox.GetComponent<HitPointController>().Defeat();
        }
        else if (dangerousBox != null)
        {
            //hurt player
            _playerStatusObject.Player.GetComponent<PlayerHitPointController>().Damage(_playerValuesObject.DamageFromFallingBoxes);

            //respawn at minor checkpoint
            GameManager.Instance.DamageRespawn();
        }
    }
}
