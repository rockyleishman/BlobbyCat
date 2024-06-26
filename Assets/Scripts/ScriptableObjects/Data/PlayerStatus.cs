using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatusObject", menuName = "Data/PlayerStatus", order = 0)]
public class PlayerStatus : ScriptableObject
{
    //PLAYER
    internal Player Player;

    //CHECKPOINT
    internal MajorCheckpoint CurrentMajorCheckpoint;
    internal MinorCheckpoint CurrentMinorCheckpoint;

    //HEALTH
    internal int CurrentSubHitPoints = 0;
    internal int CurrentHitPoints = 3;
    internal int MaxHitPoints = 3;

    //ORIENTATION
    internal bool IsFacingRight = true;
    
    //GROUND
    internal bool IsGrounded = false;
    internal bool IsAlmostGrounded = false;

    //CROUCH
    internal bool IsCrouching = false;

    //ATTACK
    internal bool IsAttacking = false;
    internal bool IsSlapAttacking = false;
    internal bool IsSlamAttacking = false;
    internal bool IsSpinAttacking = false;
    internal bool IsPounceAttacking = false;
    
    //DART
    internal bool HasDartToken = true;

    //CATNIP
    internal bool HasCatnipToken = false;

    //JUMP
    internal bool HasGeneralJumpToken = true;
    internal bool IsJumping = false;

    internal bool HasHighJumpToken = false;
    internal bool IsHighJumping = false;

    internal bool HasSingleJumpToken = false;
    internal bool IsSingleJumping = false;

    internal bool HasDoubleJumpToken = false;
    internal bool IsDoubleJumping = false;

    internal bool IsTripleJumping = false;

    internal bool IsPounceJumping = false;

    internal bool TriggerDamageJumping = false;
    internal bool IsDamageJumping = false;
}
