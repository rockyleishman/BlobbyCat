using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightController : MonoBehaviour
{
    private PlayerStatus _playerStatusObject;

    private void Start()
    {
        //init fields
        _playerStatusObject = DataManager.Instance.PlayerStatusObject;
    }

    private void Update()
    {
        transform.position = _playerStatusObject.Player.transform.position;
    }
}
