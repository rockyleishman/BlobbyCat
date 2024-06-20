using System.Collections;
using UnityEngine;

public class HitPointController : MonoBehaviour, IHitPointController
{
    private AIController _aiController;

    [SerializeField] public int MaxHitPoints = 1;
    private int _currentHitPoints;
    [SerializeField] public float PostDamageInvincibilityTime = 0.25f;
    private bool _invincible;
    [SerializeField] public Effect DeathEffect;

    private DamageableSpriteRandomizer _damageableSpriteRandomizer;
    private bool _hasDamageableSprtieRandomizer;

    private void Start()
    {
        //init fields
        _aiController = GetComponent<AIController>();
        _currentHitPoints = MaxHitPoints;
        _invincible = false;
        _damageableSpriteRandomizer = GetComponent<DamageableSpriteRandomizer>();
        _hasDamageableSprtieRandomizer = _damageableSpriteRandomizer != null;
    }

    public void RestoreHitPoints()
    {
        _currentHitPoints = MaxHitPoints;

        ChangeDamageableSprite();
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

        ChangeDamageableSprite();
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

            ChangeDamageableSprite();

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
        if (_aiController != null)
        {
            _aiController.Hurt(PostDamageInvincibilityTime);
        }

        yield return new WaitForSeconds(PostDamageInvincibilityTime);

        _invincible = false;
    }

    private void ChangeDamageableSprite()
    {
        if (_hasDamageableSprtieRandomizer)
        {
            if (_currentHitPoints / MaxHitPoints > _damageableSpriteRandomizer.DamagedSpriteHealth)
            {
                _damageableSpriteRandomizer.HealSprite();
            }
            else
            {
                _damageableSpriteRandomizer.DamageSprite();
            }
        }
    }
}
