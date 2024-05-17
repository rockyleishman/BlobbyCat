using System.Collections;
using UnityEngine;

public class HitPointController : MonoBehaviour, IHitPointController
{
    [SerializeField] public int MaxHitPoints = 1;
    private int _currentHitPoints;
    [SerializeField] public float PostDamageInvincibilityTime = 0.25f;
    private bool _invincible;
    [SerializeField] public Effect DeathEffect;

    private void Start()
    {
        //init fields
        _currentHitPoints = MaxHitPoints;
        _invincible = false;
    }

    public void RestoreHitPoints()
    {
        _currentHitPoints = MaxHitPoints;
    }

    public void Heal(int hitPoints)
    {
        _currentHitPoints += hitPoints;

        if (_currentHitPoints > MaxHitPoints)
        {
            _currentHitPoints = MaxHitPoints;
        }
        else if (_currentHitPoints <= 0)
        {
            _currentHitPoints = 0;
            Defeat();
        }
    }

    public void Damage(int hitPoints)
    {
        if (!_invincible)
        {
            _currentHitPoints -= hitPoints;

            if (_currentHitPoints > MaxHitPoints)
            {
                _currentHitPoints = MaxHitPoints;
            }
            else if (_currentHitPoints <= 0)
            {
                _currentHitPoints = 0;
                Defeat();
            }

            //i frames
            StartCoroutine(OnInvincibility());
        }        
    }

    public void Defeat()
    {
        //drop collectables
        CollectableContainer container = GetComponent<CollectableContainer>();
        if (container != null)
        {
            container.DropCollectables();
        }

        //play death effect
        if (DeathEffect != null)
        {
            PoolManager.Instance.Spawn(DeathEffect.name, transform.position, transform.rotation);
        }

        //destroy
        _invincible = false;
        Destroy(gameObject);
    }

    public int GetCurrentHitPoints()
    {
        return _currentHitPoints;
    }

    private IEnumerator OnInvincibility()
    {
        _invincible = true;

        yield return new WaitForSeconds(PostDamageInvincibilityTime);

        _invincible = false;
    }
}
