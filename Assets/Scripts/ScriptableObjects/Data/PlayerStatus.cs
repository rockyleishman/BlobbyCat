using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatusObject", menuName = "Data/PlayerStatus", order = 0)]
public class PlayerStatus : ScriptableObject
{
    internal Player Player;

    internal MajorCheckpoint CurrentMajorCheckpoint;
    internal MinorCheckpoint CurrentMinorCheckpoint;

    internal int CurrentSubHitPoints = 0;
    internal int CurrentHitPoints = 3;
    internal int MaxHitPoints = 3;

    internal bool IsFacingRight = true;
    internal bool IsGrounded = false;

    internal bool IsCrouching = false;

    internal bool IsAttacking = false;
    internal bool IsSlapAttacking = false;
    internal bool IsSlamAttacking = false;
    internal bool IsSpinAttacking = false;
    internal bool IsPounceAttacking = false;

    internal bool HasDartToken = true;

    internal bool HasGeneralJumpToken = true;
    internal bool IsJumping = false;
    internal bool HasHighJumpToken = false;
    internal bool IsHighJumping = false;
    internal bool IsLongLeaping = false;
    internal bool HasSingleJumpToken = false;
    internal bool IsSingleJumping = false;
    internal bool IsSingleLeaping = false;
    internal bool HasDoubleJumpToken = false;
    internal bool IsDoubleJumping = false;
    internal bool IsDoubleLeaping = false;
    internal bool HasTripleJumpToken = false;
    internal bool IsTripleJumping = false;
    internal bool IsTripleLeaping = false;
    internal bool IsPounceJumping = false;
}
