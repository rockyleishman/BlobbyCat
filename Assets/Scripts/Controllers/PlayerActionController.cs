using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionController : MonoBehaviour
{
    internal ActionTrigger CurrentActionTrigger;

    private bool _isConflictingInputEnabled;

    private void Start()
    {
        _isConflictingInputEnabled = true;
    }

    private void OnAction()
    {
        if (CurrentActionTrigger != null && _isConflictingInputEnabled)
        {
            CurrentActionTrigger.OnAction();
        }
    }

    public void OnPause()
    {
        //disable conflicting inputs
        _isConflictingInputEnabled = false;
    }

    public void OnResume()
    {
        StartCoroutine(OnResumeDelay());
    }

    private IEnumerator OnResumeDelay()
    {
        //delay
        yield return null;

        //enable inputs
        _isConflictingInputEnabled = true;
    }
}
