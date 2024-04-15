using UnityEngine;

public class EnemyHitPointController : MonoBehaviour, IHitPointController
{
    [SerializeField] public int MaxHitPoints = 1;
    private int _currentHitPoints;

    private void Start()
    {
        RestoreHitPoints();
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
    }

    public void Defeat()
    {
        Destroy(gameObject);
    }

    public int GetCurrentHitPoints()
    {
        return _currentHitPoints;
    }
}
