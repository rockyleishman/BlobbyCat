using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    public GameEvent OnEnableRightAttackHitbox;
    public GameEvent OnDisableRightAttackHitbox;
    public GameEvent OnEnableLeftAttackHitbox;
    public GameEvent OnDisableLeftAttackHitbox;
}
