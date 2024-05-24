using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionController : MonoBehaviour
{
    internal ActionTrigger CurrentActionTrigger;

    private void OnAction()
    {
        if (CurrentActionTrigger != null)
        {
            CurrentActionTrigger.OnAction();
        }
    }
}
