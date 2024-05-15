using UnityEngine;

public class PlayerHitPointController : MonoBehaviour, IHitPointController
{
    private PlayerStatus _playerStatusObject;

    private Animator _animator;

    private void Start()
    {
        //init fields
        _playerStatusObject = DataManager.Instance.PlayerStatusObject;
        _animator = GetComponent<Animator>();

        RestoreHitPoints();
    }

    public void RestoreHitPoints()
    {
        _playerStatusObject.CurrentHitPoints = _playerStatusObject.MaxHitPoints;
        HUDManager.Instance.UpdateHP();
    }

    public void Heal(int hitPoints)
    {
        _playerStatusObject.CurrentHitPoints += hitPoints;

        if (_playerStatusObject.CurrentHitPoints > _playerStatusObject.MaxHitPoints)
        {
            _playerStatusObject.CurrentHitPoints = _playerStatusObject.MaxHitPoints;
        }
        else if (_playerStatusObject.CurrentHitPoints <= 0)
        {
            _playerStatusObject.CurrentHitPoints = 0;
            Defeat();
            return;
        }

        HUDManager.Instance.UpdateHP();

        //animate heal
    }

    public void Damage(int hitPoints)
    {
        _playerStatusObject.CurrentHitPoints -= hitPoints;

        if (_playerStatusObject.CurrentHitPoints > _playerStatusObject.MaxHitPoints)
        {
            _playerStatusObject.CurrentHitPoints = _playerStatusObject.MaxHitPoints;
        }
        else if (_playerStatusObject.CurrentHitPoints <= 0)
        {
            _playerStatusObject.CurrentHitPoints = 0;
            Defeat();
            return;
        }

        HUDManager.Instance.UpdateHP();

        //animate damage
    }

    public void Defeat()
    {
        HUDManager.Instance.UpdateHP();

        //TODO: player death
        Debug.Log("PLAYER DIED");

        //animate death
    }

}