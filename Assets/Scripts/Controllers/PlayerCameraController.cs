using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraController : MonoBehaviour
{
    private PlayerValues _playerValuesObject;

    private CameraTarget _cameraTarget;

    private Vector2 _lookInput;
    private Vector2 _movementInput;
    private Vector3 _currentVelocity;

    private void Start()
    {
        //init fields
        _playerValuesObject = DataManager.Instance.PlayerValuesObject;
        _cameraTarget = GetComponentInChildren<CameraTarget>();
        _lookInput = Vector2.zero;
        _movementInput = Vector2.zero;
        _currentVelocity = Vector3.zero;
    }

    private void OnEnable()
    {
        //reset input
        _lookInput = Vector2.zero;
        _movementInput = Vector2.zero;
    }

    private void Update()
    {
        if (_movementInput == Vector2.zero)
        {
            //look with camera
            _cameraTarget.transform.localPosition = Vector3.SmoothDamp(_cameraTarget.transform.localPosition, new Vector3(_lookInput.x * _playerValuesObject.HorizontalLookDistance, _lookInput.y * _playerValuesObject.VerticalLookDistance, 0.0f), ref _currentVelocity, _playerValuesObject.LookDampingTime);
        }
        else
        {
            //can't look while moving
            _cameraTarget.transform.localPosition = Vector3.SmoothDamp(_cameraTarget.transform.localPosition, Vector3.zero, ref _currentVelocity, _playerValuesObject.LookDampingTime);
        }        
    }

    public void ResetCamera()
    {
        _cameraTarget.transform.localPosition = Vector3.zero;
    }

    private void OnLook(InputValue value)
    {
        _lookInput = value.Get<Vector2>();

        //deadzone
        if (_lookInput.magnitude < _playerValuesObject.JoystickDeadzone)
        {
            _lookInput = Vector2.zero;
        }

        //clamp digital input
        if (_lookInput.magnitude > 1.0f)
        {
            _lookInput.Normalize();
        }
    }

    private void OnMovement(InputValue value)
    {
        _movementInput = value.Get<Vector2>();

        //deadzone
        if (_movementInput.magnitude < _playerValuesObject.JoystickDeadzone)
        {
            _movementInput = Vector2.zero;
        }
    }
}
