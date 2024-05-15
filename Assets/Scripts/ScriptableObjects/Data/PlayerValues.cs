using UnityEngine;

[CreateAssetMenu(fileName = "PlayerValuesObject", menuName = "Data/PlayerValues", order = 0)]
public class PlayerValues : ScriptableObject
{
    [Space(10.0f)]
    [SerializeField] public int HUDPixelsFromTop = 4;
    [SerializeField] public int HUDPixelsFromSide = 0;
    [SerializeField] public int HUDPixelsInBetween = 8;
    [SerializeField] public int HUD3HPLength = 60;
    [SerializeField] public int HUD4HPLength = 76;
    [SerializeField] public int HUD5HPLength = 92;
    [SerializeField] public int HUD6HPLength = 108;
    [SerializeField] public int HUD7HPLength = 124;
    [SerializeField] public int HUD8HPLength = 140;
    [SerializeField] public int HUD9HPLength = 156;
    [SerializeField] public float HUDRevealTime = 0.25f;
    [SerializeField] public float HUDHideTime = 0.5f;
    [SerializeField] public float HUDShowHPTime = 5.0f;
    [SerializeField] public float HUDShowTreatTime = 5.0f;
    [Space(10.0f)]
    [SerializeField] public float JoystickDeadzone = 0.2f;
    [Space(10.0f)]
    [SerializeField] public float HorizontalLookDistance = 6.0f;
    [SerializeField] public float VerticalLookDistance = 4.5f;
    [SerializeField] public float LookDampingTime = 0.5f;
    [Space(10.0f)]
    [SerializeField] public float DartTime = 0.25f;
    [SerializeField] public float GroundDartSpeedBoost = 2.5f;
    [SerializeField] public float GroundDartMinimumSpeed = 5.0f;
    [SerializeField] public float GroundDartMaximumSpeed = 10.0f;
    [SerializeField] public float AirDartSpeedBoost = 2.5f;
    [SerializeField] public float AirDartMinimumSpeed = 5.0f;
    [SerializeField] public float AirDartMaximumSpeed = 10.0f;
    [SerializeField] public float OverdriveDeceleration = 1.0f;
    [Space(10.0f)]
    [SerializeField] public float GroundMovementSpeed = 5.0f;
    [SerializeField] public float GroundAcceleration = 30.0f;
    [SerializeField] public float GroundDeceleration = 30.0f;
    [Space(10.0f)]
    [SerializeField] public float GroundRunMovementSpeed = 7.5f;
    [SerializeField] public float GroundRunAcceleration = 2.5f;
    [Space(10.0f)]
    [SerializeField] public float CrouchMovementSpeed = 1.0f;
    [SerializeField] public float CrouchAcceleration = 30.0f;
    [SerializeField] public float CrouchDeceleration = 30.0f;
    [SerializeField] public float CrouchOverdriveDeceleration = 5.0f;
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
    [SerializeField] public float AirRunMovementSpeed = 7.5f;
    [SerializeField] public float AirRunAcceleration = 1.25f;
    [Space(10.0f)]
    [SerializeField] public float Gravity = 20.0f;
    [SerializeField] public float NegativeTerminalVelocity = 10.0f;
    [SerializeField] public float PositiveVelocityHalflife = 0.5f;
    [Space(10.0f)]
    [SerializeField] public float GroundDetectionRange = 0.0625f;
    [SerializeField] public float GroundDetectionSpan = 0.5f;
    [SerializeField] public float Hangtime = 0.1f;
    [Space(10.0f)]
    [SerializeField] public float JumpCooldown = 0.25f;
    [SerializeField] public float DamageJumpHeight = 1.25f;
    [SerializeField] public float DamageJumpTime = 0.5f;
    [SerializeField] public float HighJumpWindow = 0.1f;
    [SerializeField] public float HighJumpHeight = 3.25f;
    [SerializeField] public float HighJumpTime = 0.5f;
    [SerializeField] public float SingleJumpHeight = 2.25f;
    [SerializeField] public float SingleJumpTime = 0.5f;
    [SerializeField] public float DoubleJumpHeight = 2.0f;
    [SerializeField] public float DoubleJumpTime = 0.5f;
    [SerializeField] public float TripleJumpHeight = 1.0f;
    [SerializeField] public float TripleJumpTime = 0.375f;
    [Space(10.0f)]
    [SerializeField] public int SlapAttackDamage = 2;
    [SerializeField] public float SlapAttackCost = 0.0f;
    [SerializeField] public float SlapAttackTime = 0.25f;
    [SerializeField] public float SlapAttackCooldown = 0.1f;
    [Space(10.0f)]
    [SerializeField] public int SpinAttackDamage = 1;
    [SerializeField] public float SpinAttackCost = 0.25f;
    [SerializeField] public float SpinAttackTime = 0.5f;
    [SerializeField] public float SpinAttackCooldown = 0.1f;
    [Space(10.0f)]
    [SerializeField] public int PounceAttackDamage = 2;
    [SerializeField] public float PounceAttackCost = 0.25f;
    [SerializeField] public float PounceAttackCooldown = 0.1f;
    [SerializeField] public float PounceSpeed = 7.5f;
    [SerializeField] public float PounceJumpHeight = 1.25f;
    [SerializeField] public float PounceJumpTime = 0.5f;
    [Space(10.0f)]
    [SerializeField] public int DamageFromWater = 1;
    [Space(10.0f)]
    [SerializeField] public float TreatSuction = 0.5f;
    [Space(10.0f)]
    [SerializeField] public Effect StepEffect;
    [SerializeField] public Effect DartSlowEffect;
    [SerializeField] public Effect DartFastEffect;
    [SerializeField] public Effect DartSuperFastEffect;
    [SerializeField] public Effect LandEffect;
    [SerializeField] public Effect HighJumpEffect;
    [SerializeField] public Effect SingleJumpEffect;
    [SerializeField] public Effect DoubleJumpEffect;
    [SerializeField] public Effect TripleJumpEffect;
    [SerializeField] public Effect PounceJumpEffect;
}
