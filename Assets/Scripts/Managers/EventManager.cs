using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    [Header("Pause Events")]
    [Space(10)]
    [SerializeField] public GameEvent OnPause;
    [SerializeField] public GameEvent OnResume;
    [Space(10)]
    [Header("Attack Hitbox Events")]
    [Space(10)]
    [SerializeField] public GameEvent OnEnableRightSlapHitbox;
    [SerializeField] public GameEvent OnDisableRightSlapHitbox;
    [SerializeField] public GameEvent OnEnableLeftSlapHitbox;
    [SerializeField] public GameEvent OnDisableLeftSlapHitbox;
    [Space(10)]
    [SerializeField] public GameEvent OnEnableSlamHitbox;
    [SerializeField] public GameEvent OnDisableSlamHitbox;
    [Space(10)]
    [SerializeField] public GameEvent OnEnableSpinHitbox;
    [SerializeField] public GameEvent OnDisableSpinHitbox;
    [Space(10)]
    [SerializeField] public GameEvent OnEnableRightPounceHitbox;
    [SerializeField] public GameEvent OnDisableRightPounceHitbox;
    [SerializeField] public GameEvent OnEnableLeftPounceHitbox;
    [SerializeField] public GameEvent OnDisableLeftPounceHitbox;
}
