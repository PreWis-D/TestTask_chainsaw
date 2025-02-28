using SpawnerExample;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] private HealthBar _healthBar;

    private EnemyMover _mover;
    private EnemyStateMachine _stateMachine;

    public float CurrentHealth {  get; private set; }
    public int Reward {  get; private set; }

    public event Action<IDamagable> Died;
    public event Action<float> DamageTaked;

    public void Init(List<SpawnPoint> spawnPoints, int currentPointIndex, EnemyPrefabData data)
    {
        CurrentHealth = data.StartHealth;
        Reward = data.Reward;
        _healthBar.Init(this);

        _mover = new EnemyMover(transform);
        _stateMachine = new EnemyStateMachine(this, _mover, data, spawnPoints, currentPointIndex);
    }

    private void Update()
    {
        _stateMachine.Update();
    }

    public void TakeDamage(float damage)
    {
        if (CurrentHealth <= 0)
            return;

        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            Died?.Invoke(this);
        }
        else
        {
            DamageTaked?.Invoke(damage);
        }
    }

    private void OnDestroy()
    {
        _stateMachine.OnDestroy();
    }
}