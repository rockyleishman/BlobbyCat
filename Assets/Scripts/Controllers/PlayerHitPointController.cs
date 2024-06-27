using System.Collections;
using UnityEngine;

public class PlayerHitPointController : MonoBehaviour, IHitPointController
{
    private PlayerValues _playerValuesObject;
    private PlayerStatus _playerStatusObject;

    private Animator _animator;

    private bool _invincible;

    private void Start()
    {
        //init fields
        _playerValuesObject = DataManager.Instance.PlayerValuesObject;
        _playerStatusObject = DataManager.Instance.PlayerStatusObject;
        _animator = GetComponent<Animator>();
        _invincible = false;

        RestoreHitPoints();
    }

    private void OnEnable()
    {
        _invincible = false;
    }

    public void RestoreHitPoints()
    {
        _playerStatusObject.CurrentHitPoints = _playerStatusObject.MaxHitPoints;
        HUDManager.Instance.UpdateHP();
        StartCoroutine(OnInvincibility());
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

        CheckSubHeal();

        //update HUD
        HUDManager.Instance.UpdateHP();

        //TODO: animate heal////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    }

    public void Damage(int hitPoints)
    {
        if (!_invincible)
        {
            _playerStatusObject.CurrentHitPoints -= hitPoints;

            if (_playerStatusObject.CurrentHitPoints > _playerStatusObject.MaxHitPoints)
            {
                _playerStatusObject.CurrentHitPoints = _playerStatusObject.MaxHitPoints;
            }
            else if (_playerStatusObject.CurrentHitPoints <= 0)
            {
                _playerStatusObject.CurrentHitPoints = 0;
                _invincible = true;
                Defeat();
                return;
            }

            //i frames
            StartCoroutine(OnInvincibility());

            //update HUD
            HUDManager.Instance.UpdateHP();

            //animate damage
            _animator.SetTrigger("Hurt");

            //damage jump
            _playerStatusObject.TriggerDamageJumping = true;

            //heal if full sub hit points
            CheckSubHeal();
        }
    }

    public void GainSubHitPoint()
    {
        if (_playerStatusObject.CurrentSubHitPoints < _playerValuesObject.MaxSubHitPoints)
        {
            _playerStatusObject.CurrentSubHitPoints++;
        }

        CheckSubHeal();
    }

    private void CheckSubHeal()
    {
        if (_playerStatusObject.CurrentSubHitPoints == _playerValuesObject.MaxSubHitPoints && _playerStatusObject.CurrentHitPoints < _playerStatusObject.MaxHitPoints)
        {
            _playerStatusObject.CurrentSubHitPoints = 0;
            Heal(1);
            HUDManager.Instance.UpdateTreatCount(false);
        }
    }

    public void Defeat()
    {
        //animate HP
        HUDManager.Instance.UpdateHP();

        //zero sub hit points
        _playerStatusObject.CurrentSubHitPoints = 0;

        //TODO: reset level stuff based on last major checkpoint//////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //player death
        GameManager.Instance.DeathRespawn();

        //TODO: animate death//////////////////////////////////////////////////////////////////////////////////////////////////////////////

    }

    private IEnumerator OnInvincibility()
    {
        _invincible = true;

        yield return new WaitForSeconds(_playerValuesObject.PostDamageInvincibilityTime);

        _invincible = false;
    }
}
