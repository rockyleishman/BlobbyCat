using UnityEngine;

[CreateAssetMenu(fileName = "PlayerValuesObject", menuName = "Data/PlayerValues", order = 0)]
public class PlayerValues : ScriptableObject
{
    [SerializeField] public float HorizontalLookDistance = 6.0f;
    [SerializeField] public float VerticalLookDistance = 4.5f;
    [SerializeField] public float LookDampingTime = 0.5f;
    [Space(10.0f)]
    [SerializeField] public float GroundMovementSpeed = 5.0f;
    [SerializeField] public float GroundAcceleration = 30.0f;
    [SerializeField] public float GroundDeceleration = 30.0f;
    [Space(10.0f)]
    [SerializeField] public float GroundRunMovementSpeed = 7.5f;
    [SerializeField] public float GroundRunAcceleration = 30.0f;
    [SerializeField] public float GroundRunDeceleration = 30.0f;
    [Space(10.0f)]
    [SerializeField] public float CrouchMovementSpeed = 2.5f;
    [SerializeField] public float CrouchAcceleration = 15.0f;
    [SerializeField] public float CrouchDeceleration = 15.0f;
    [Space(10.0f)]
    [SerializeField] public float SlideDeceleration = 2.5f;
    [Space(10.0f)]
    [SerializeField] public float LiquidMovementSpeed = 2.5f;
    [SerializeField] public float LiquidAcceleration = 7.5f;
    [SerializeField] public float LiquidDeceleration = 7.5f;
    [Space(10.0f)]
    [SerializeField] public float AirMovementSpeed = 5.0f;
    [SerializeField] public float AirAcceleration = 15.0f;
    [SerializeField] public float AirDeceleration = 15.0f;
    [Space(10.0f)]
    [SerializeField] public float AirRunMovementSpeed = 6.25f;
    [SerializeField] public float AirRunAcceleration = 3.125f;
    [SerializeField] public float AirRunDeceleration = 3.125f;
    [Space(10.0f)]
    [SerializeField] public float Gravity = 5.0f;
    [SerializeField] public float NegativeTerminalVelocity = 10.0f;
    [SerializeField] public float PositiveVelocityHalflife = 0.5f;
    [Space(10.0f)]
    [SerializeField] public float GroundDetectionRange = 0.1f;
    [SerializeField] public float GroundDetectionSpan = 0.5f;
    [SerializeField] public float Hangtime = 0.1f;
    [Space(10.0f)]
    [SerializeField] public float JumpCooldown = 0.25f;
    [SerializeField] public float DamageJumpHeight = 1.5f;
    [SerializeField] public float DamageJumpTime = 0.4f;
    [SerializeField] public float HighJumpWindow = 0.1f;
    [SerializeField] public float HighJumpHeight = 3.5f;
    [SerializeField] public float HighJumpTime = 0.5f;
    [SerializeField] public float SingleJumpHeight = 2.5f;
    [SerializeField] public float SingleJumpTime = 0.5f;
    [SerializeField] public float DoubleJumpHeight = 2.0f;
    [SerializeField] public float DoubleJumpTime = 0.45f;
    [SerializeField] public float TripleJumpHeight = 1.0f;
    [SerializeField] public float TripleJumpTime = 0.35f;
    [Space(10.0f)]
    [SerializeField] public int SlapAttackDamage = 2;
    [SerializeField] public float SlapAttackCost = 0.0f;
    [SerializeField] public float SlapAttackTime = 0.25f;
    [SerializeField] public float SlapAttackCooldown = 0.1f;
    [Space(10.0f)]
    [SerializeField] public float SpinAttackDamage = 1;
    [SerializeField] public float SpinAttackCost = 0.25f;
    [SerializeField] public float SpinAttackTime = 0.5f;
    [SerializeField] public float SpinAttackCooldown = 0.1f;
    [Space(10.0f)]
    [SerializeField] public float PounceAttackDamage = 2;
    [SerializeField] public float PounceAttackCost = 0.25f;
    [SerializeField] public float PounceAttackCooldown = 0.1f;
    [SerializeField] public float PounceSpeed = 7.5f;
    [SerializeField] public float PounceJumpHeight = 1.25f;
    [SerializeField] public float PounceJumpTime = 0.5f;
}
