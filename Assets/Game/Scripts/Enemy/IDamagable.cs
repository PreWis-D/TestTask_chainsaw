using System;

public interface IDamagable
{
    public float CurrentHealth { get; }

    public event Action<IDamagable> Died;
    public event Action<float> DamageTaked;

    void TakeDamage(float damage);
}