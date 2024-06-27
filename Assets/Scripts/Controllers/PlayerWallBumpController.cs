using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallBumpController : MonoBehaviour
{
    private PlayerValues _playerValuesObject;
    private PlayerStatus _playerStatusObject;

    private void Start()
    {
        //init fields
        _playerValuesObject = DataManager.Instance.PlayerValuesObject;
        _playerStatusObject = DataManager.Instance.PlayerStatusObject;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //bump player if touching ground, but not grounded
        if (!_playerStatusObject.IsGrounded && collision.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Debug.Log("bump");
            _playerStatusObject.Player.transform.position += -((Vector3)collision.GetContact(0).point - _playerStatusObject.Player.GetComponent<Collider2D>().bounds.center).normalized * _playerValuesObject.WallBumpDistance;
        }
    }
}
