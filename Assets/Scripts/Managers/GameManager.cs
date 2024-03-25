using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private PlayerStatus _playerStatusObject;

    private void Awake()
    {
        //init fields
        _playerStatusObject = DataManager.Instance.PlayerStatusObject;

        //find player
        _playerStatusObject.Player = FindFirstObjectByType<Player>();

        //TODO: change when implementing persistant player data
        //reset player status object
        ResetPlayerStatus();
    }

    private void ResetPlayerStatus()
    {
        _playerStatusObject.CurrentHitPoints = 3;
        _playerStatusObject.MaxHitPoints = 3;
        _playerStatusObject.CurrentSpecialPoints = 3.0f;
        _playerStatusObject.MaxSpecialPoints = 3.0f;

        _playerStatusObject.IsFacingRight = true;
        _playerStatusObject.IsGrounded = false;
        _playerStatusObject.IsLiquid = false;

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
    }
}
