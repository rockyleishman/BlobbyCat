using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPauseController : MonoBehaviour
{
    private PlayerInput _playerInput;

    private bool _isConflictingInputEnabled;

    private void Awake()
    {
        //init fields
        _playerInput = GetComponent<PlayerInput>();
        _isConflictingInputEnabled = true;

        //disable menu controls
        _playerInput.actions.FindActionMap("Menu").Disable();
    }

    public void OnPause()
    {
        if (_isConflictingInputEnabled)
        {
            //trigger event
            EventManager.Instance.OnPause.TriggerEvent(Vector3.zero);

            //disable conflicting inputs
            _isConflictingInputEnabled = false;

            //pause game
            Time.timeScale = 0.0f;

            //change player controls
            _playerInput.SwitchCurrentActionMap("Menu");

            //show pause menu
            MenuManager.Instance.ShowPauseMenu();
        }
    }

    private void OnPause2()
    {
        if(_isConflictingInputEnabled)
        {
            //trigger event
            EventManager.Instance.OnPause.TriggerEvent(Vector3.zero);

            //disable conflicting inputs
            _isConflictingInputEnabled = false;

            //pause game
            Time.timeScale = 0.0f;

            //change player controls
            _playerInput.SwitchCurrentActionMap("Menu");

            //show inventory menu
            MenuManager.Instance.ShowInventoryMenu();
        }
    }

    public void OnResume()
    {
        //hide menu
        MenuManager.Instance.HideMenu();

        //change player controls
        _playerInput.SwitchCurrentActionMap("Gameplay");

        //resume game
        Time.timeScale = 1.0f;

        //trigger event
        EventManager.Instance.OnResume.TriggerEvent(Vector3.zero);

        //delay conflicting input enabling
        StartCoroutine(OnResumeDelay());
    }

    private IEnumerator OnResumeDelay()
    {
        //delay
        yield return null;
        
        //enable inputs
        _isConflictingInputEnabled = true;
    }

    private void OnBack()
    {
        switch (MenuManager.Instance.GetCurrentMenu())
        {
            case MenuName.Pause:
                {
                    OnResume();
                }
                break;

            case MenuName.Inventory:
                {
                    MenuManager.Instance.ShowPauseMenu();
                }
                break;

            case MenuName.Settings:
                {
                    MenuManager.Instance.ShowPauseMenu();
                }
                break;

            case MenuName.Debug:
                {
                    MenuManager.Instance.ShowPauseMenu();
                }
                break;

            case MenuName.None:
            default:
                throw new InvalidInputActionException(); //OnBack() should not be executed if there is no active menu to go back from
        }
    }
}
