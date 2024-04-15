using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private PlayerStatus _playerStatusObject;

    public MajorCheckpoint StartingCheckpoint;

    private bool _isRespawning;

    private void Awake()
    {
        //init fields
        _playerStatusObject = DataManager.Instance.PlayerStatusObject;
        _isRespawning = false;

        //TODO: change when implementing persistant player data
        //reset player status object
        ResetPlayerStatus();
    }

    private void ResetPlayerStatus()
    {
        _playerStatusObject.Player = FindFirstObjectByType<Player>();

        _playerStatusObject.CurrentMajorCheckpoint = StartingCheckpoint;
        _playerStatusObject.CurrentMinorCheckpoint = StartingCheckpoint;

        _playerStatusObject.CurrentHitPoints = 3;
        _playerStatusObject.MaxHitPoints = 3;
        _playerStatusObject.CurrentLives = 9;
        _playerStatusObject.MaxLives = 9;
        _playerStatusObject.CurrentSpecialPoints = 3.0f;
        _playerStatusObject.MaxSpecialPoints = 3.0f;

        _playerStatusObject.IsFacingRight = true;
        _playerStatusObject.IsGrounded = false;

        _playerStatusObject.IsAttacking = false;
        _playerStatusObject.IsSlapAttacking = false;
        _playerStatusObject.IsSpinAttacking = false;
        _playerStatusObject.IsPounceAttacking = false;

        _playerStatusObject.HasDartToken = true;

        _playerStatusObject.HasGeneralJumpToken = true;
        _playerStatusObject.IsJumping = false;
        _playerStatusObject.HasHighJumpToken = false;
        _playerStatusObject.IsHighJumping = false;
        _playerStatusObject.HasSingleJumpToken = false;
        _playerStatusObject.IsSingleJumping = false;
        _playerStatusObject.HasDoubleJumpToken = false;
        _playerStatusObject.IsDoubleJumping = false;
        _playerStatusObject.HasTripleJumpToken = false;
        _playerStatusObject.IsTripleJumping = false;
        _playerStatusObject.IsPounceJumping = false;
    }

    public void DamageRespawn()
    {
        if (!_isRespawning)
        {
            StartCoroutine(DamageRespawnCoroutine());
        }
    }

    private IEnumerator DamageRespawnCoroutine()
    {
        _isRespawning = true;

        _playerStatusObject.Player.gameObject.SetActive(false);

        //TODO: effects

        //hide
        _playerStatusObject.Player.GetComponent<Renderer>().enabled = false;

        //respawn delay
        yield return new WaitForSeconds(1.0f);

        //teleport to minor checkpoint
        _playerStatusObject.Player.transform.position = _playerStatusObject.CurrentMinorCheckpoint.transform.position;

        //zero velocity
        _playerStatusObject.Player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        //enable player
        _playerStatusObject.Player.GetComponent<Renderer>().enabled = true;
        _playerStatusObject.Player.gameObject.SetActive(true);

        //TODO: respawn effects

        _isRespawning = false;
    }

    public void DeathRespawn()
    {
        if (!_isRespawning)
        {
            StartCoroutine(DeathRespawnCoroutine());
        }
    }

    private IEnumerator DeathRespawnCoroutine()
    {
        _isRespawning = true;

        //disable
        _playerStatusObject.Player.gameObject.SetActive(false);

        //TODO: play death animation & effects

        //hide
        _playerStatusObject.Player.GetComponent<Renderer>().enabled = false;

        //respawn delay
        yield return new WaitForSeconds(1.0f);

        //teleport to major checkpoint
        _playerStatusObject.Player.transform.position = _playerStatusObject.CurrentMajorCheckpoint.transform.position;

        //zero velocity
        _playerStatusObject.Player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        //enable player
        _playerStatusObject.Player.GetComponent<Renderer>().enabled = true;
        _playerStatusObject.Player.gameObject.SetActive(true);

        //TODO: respawn effects

        _isRespawning = false;
    }
}
