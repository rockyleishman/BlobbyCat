using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameStatusObject", menuName = "Data/GameStatus", order = 0)]
public class GameStatus : ScriptableObject
{
    //UNLOCKABLE MOVES
    internal bool unlockedDash;
    internal bool unlockedClimb;
    internal bool unlockedLiquidCat;
    internal bool unlockedChonkMode;
    internal bool unlockedDoubleJump;
}
