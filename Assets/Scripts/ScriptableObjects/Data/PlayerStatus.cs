using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatusObject", menuName = "Data/PlayerStatus", order = 0)]
public class PlayerStatus : ScriptableObject
{
    internal Player Player;

    internal int CurrentHitPoints = 3;
    internal int MaxHitPoints = 3;
    internal float CurrentSpecialPoints = 3.0f;
    internal float MaxSpecialPoints = 3.0f;

    internal bool IsFacingRight = true;
    internal bool IsGrounded = false;
    internal bool IsLiquid = false;

    internal bool IsAttacking = false;
    internal bool IsSlapAttacking = false;
    internal bool IsSpinAttacking = false;
    internal bool IsPounceAttacking = false;

    internal bool HasGeneralJumpToken = true;
    internal bool IsJumping = false;
    internal bool HasHighJumpToken = false;
    internal bool IsHighJumping = false;
    internal bool HasSingleJumpToken = false;
    internal bool IsSingleJumping = false;
    internal bool HasDoubleJumpToken = false;
    internal bool IsDoubleJumping = false;
    internal bool HasTripleJumpToken = false;
    internal bool IsTripleJumping = false;
    internal bool IsPounceJumping = false;
}
