using UnityEngine;

[CreateAssetMenu(fileName = "PlayerValuesObject", menuName = "Data/PlayerValues", order = 0)]
public class PlayerValues : ScriptableObject
{
    [Header("HUD")]
    [SerializeField] public int HUDPixelsFromTop = 4;
    [SerializeField] public int HUDPixelsFromSide = 0;
    [SerializeField] public int HUDPixelsInBetween = 8;
    [Space(10.0f)]
    [SerializeField] public int HUD3HPLength = 64;
    [SerializeField] public int HUD4HPLength = 80;
    [SerializeField] public int HUD5HPLength = 96;
    [SerializeField] public int HUD6HPLength = 112;
    [SerializeField] public int HUD7HPLength = 128;
    [SerializeField] public int HUD8HPLength = 144;
    [SerializeField] public int HUD9HPLength = 160;
    [Space(10.0f)]
    [SerializeField] public float SlowestCountTime = 0.2f;
    [SerializeField] public int SlowestCountDifference = 5;
    [SerializeField] public float FastestCountTime = 0.02f;
    [SerializeField] public int FastestCountDifference = 50;
    [Space(10.0f)]
    [SerializeField] public float HUDRevealTime = 0.1f;
    [SerializeField] public float HUDHideTime = 0.5f;
    [Space(10.0f)]
    [SerializeField] public float HUDShowHPTime = 5.0f;
    [SerializeField] public float HUDShowTreatTime = 5.0f;

    [Header("Joystick")]
    [SerializeField] public float JoystickDeadzone = 0.2f;

    [Header("Healing")]
    [SerializeField] public int MaxSubHitPoints = 50;

    [Header("Camera Controls")]
    [SerializeField] public float HorizontalLookDistance = 6.0f;
    [SerializeField] public float VerticalLookDistance = 4.5f;
    [SerializeField] public float LookDampingTime = 0.5f;

    [Header("Dart")]
    [SerializeField] public Effect DartSlowEffect;
    [SerializeField] public Effect DartFastEffect;
    [SerializeField] public Effect DartSuperFastEffect;
    [Space(10.0f)]
    [SerializeField] public float DartTime = 0.25f;
    [SerializeField] public float GroundDartSpeedBoost = 2.5f;
    [SerializeField] public float GroundDartMinimumSpeed = 5.0f;
    [SerializeField] public float GroundDartMaximumSpeed = 10.0f;
    [SerializeField] public float AirDartSpeedBoost = 2.5f;
    [SerializeField] public float AirDartMinimumSpeed = 5.0f;
    [SerializeField] public float AirDartMaximumSpeed = 10.0f;
    [SerializeField] public float OverdriveDeceleration = 1.0f;

    [Header("Catnip")]
    [SerializeField] public Effect CatnipJumpEffect;
    [SerializeField] public float MaxCatnipTime = 10.0f;

    [Header("Movement")]
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
    [SerializeField] public float LiquidMovementSpeed = 2.5f;//remove
    [SerializeField] public float LiquidAcceleration = 7.5f;//remove
    [SerializeField] public float LiquidDeceleration = 7.5f;//remove
    [Space(10.0f)]
    [SerializeField] public float AirMovementSpeed = 5.0f;
    [SerializeField] public float AirAcceleration = 15.0f;
    [SerializeField] public float AirDeceleration = 15.0f;
    [Space(10.0f)]
    [SerializeField] public float AirRunMovementSpeed = 7.5f;
    [SerializeField] public float AirRunAcceleration = 1.25f;

    [Header("Fall")]
    [SerializeField] public float Gravity = 20.0f;
    [SerializeField] public float NegativeTerminalVelocity = 10.0f;
    [SerializeField] public float PositiveVelocityHalflife = 0.5f;

    [Header("Ground")]
    [SerializeField] public Effect LandEffect;
    [Space(10.0f)]
    [SerializeField] public float GroundDetectionRange = 0.0625f;
    [SerializeField] public float AlmostGroundDetectionRange = 0.75f;
    [SerializeField] public float Hangtime = 0.1f;
    [SerializeField] public float GroundedGravity = 10.0f;
    [SerializeField] public float WallBumpDistance = 0.0625f;

    [Header("Jump")]
    [SerializeField] public float JumpCooldown = 0.25f;
    [Space(10.0f)]
    [SerializeField] public Effect HighJumpEffect;
    [SerializeField] public float HighJumpWindow = 0.1f;
    [SerializeField] public float HighJumpHeight = 3.25f;
    [SerializeField] public float HighJumpTime = 0.5f;
    [Space(10.0f)]
    [SerializeField] public Effect SingleJumpEffect;
    [SerializeField] public float SingleJumpHeight = 2.25f;
    [SerializeField] public float SingleJumpTime = 0.5f;
    [Space(10.0f)]
    [SerializeField] public Effect DoubleJumpEffect;
    [SerializeField] public float DoubleJumpHeight = 2.125f;
    [SerializeField] public float DoubleJumpTime = 0.5f;
    [Space(10.0f)]
    [SerializeField] public Effect TripleJumpEffect;
    [SerializeField] public float TripleJumpHeight = 2.125f;
    [SerializeField] public float TripleJumpTime = 0.5f;

    [Header("Attack: Slap")]
    [SerializeField] public int SlapAttackDamage = 2;
    [SerializeField] public float SlapAttackCost = 0.0f;
    [SerializeField] public float SlapAttackTime = 0.25f;
    [SerializeField] public float SlapAttackCooldown = 0.1f;

    [Header("Attack: Slam")]
    [SerializeField] public int SlamAttackDamage = 2;
    [SerializeField] public float SlamAttackCost = 0.0f;
    [SerializeField] public float SlamAttackCooldown = 0.1f;
    [SerializeField] public float SlamAttackGravity = 40.0f;
    [SerializeField] public float SlamAttackNegativeTerminalVelocity = 15.0f;

    [Header("Attack: Spin")]
    [SerializeField] public int SpinAttackDamage = 1;
    [SerializeField] public float SpinAttackCost = 0.25f;
    [SerializeField] public float SpinAttackTime = 0.5f;
    [SerializeField] public float SpinAttackGravity = 5.0f;
    [SerializeField] public float SpinAttackCooldown = 0.1f;

    [Header("Attack: Pounce")]
    [SerializeField] public int PounceAttackDamage = 4;
    [SerializeField] public float PounceAttackCost = 0.25f;
    [SerializeField] public float PounceAttackCooldown = 0.1f;
    [SerializeField] public float PounceSpeed = 7.5f;
    [Space(10.0f)]
    [SerializeField] public Effect PounceJumpEffect;
    [SerializeField] public float PounceJumpHeight = 1.25f;
    [SerializeField] public float PounceJumpTime = 0.5f;

    [Header("Damage")]
    [SerializeField] public float PostDamageInvincibilityTime = 1.0f;
    [SerializeField] public Effect DamageJumpEffect;
    [SerializeField] public float DamageJumpHeight = 0.75f;
    [SerializeField] public float DamageJumpTime = 0.125f;
    [Space(10.0f)]
    [SerializeField] public int DamageFromFallingBoxes = 1;
    [SerializeField] public int DamageFromWater = 1;

    [Header("Collectables")]
    [SerializeField] public float CollectableDelay = 0.05f;
    [SerializeField] public float CollectableSuction = 0.1f;
    [SerializeField] public float CollectableMaxSpeed = 20.0f;
}
