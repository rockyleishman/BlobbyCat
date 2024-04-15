using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    [Space(10)]
    [Header("Attack Hitbox Events")]
    [Space(10)]
    [SerializeField] public GameEvent OnEnableRightSlapHitbox;
    [SerializeField] public GameEvent OnDisableRightSlapHitbox;
    [SerializeField] public GameEvent OnEnableLeftSlapHitbox;
    [SerializeField] public GameEvent OnDisableLeftSlapHitbox;
    [Space(10)]
    [SerializeField] public GameEvent OnEnableRightSpinHitbox;
    [SerializeField] public GameEvent OnDisableRightSpinHitbox;
    [SerializeField] public GameEvent OnEnableLeftSpinHitbox;
    [SerializeField] public GameEvent OnDisableLeftSpinHitbox;
    [Space(10)]
    [SerializeField] public GameEvent OnEnableRightPounceHitbox;
    [SerializeField] public GameEvent OnDisableRightPounceHitbox;
    [SerializeField] public GameEvent OnEnableLeftPounceHitbox;
    [SerializeField] public GameEvent OnDisableLeftPounceHitbox;
}
