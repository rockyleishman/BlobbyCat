public interface IHitPointController
{
    public abstract void RestoreHitPoints();
    public abstract void Heal(int hitPoints);
    public abstract void Damage(int hitPoints);
    public abstract void Defeat();
}