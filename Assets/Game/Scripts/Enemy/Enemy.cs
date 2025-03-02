using Assets.Game.Scripts.Enemy;
using DG.Tweening;
using SpawnerExample;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _model;
    [SerializeField] private ParticleSystem _trailPrefab;
    [SerializeField] private SpriteRenderer _renderer;

    private EnemyMover _mover;
    private EnemyAnimator _animatorController;
    private EnemyStateMachine _stateMachine;
    private ParticleController _bloodVFX;
    private ParticleController _explodeVFX;
    private RewardLoot _rewardLoot;
    private MoneyType _moneyType;
    private Tween _tween;

    public float CurrentHealth { get; private set; }
    public float AlertRadius { get; private set; }
    public int Reward { get; private set; }

    public event Action<IDamagable> Died;
    public event Action<float> DamageTaked;

    public void Init(List<SpawnPoint> spawnPoints, int currentPointIndex, EnemyPrefabData data)
    {
        CurrentHealth = data.StartHealth;
        Reward = data.Reward;
        _healthBar.Init(this);

        AlertRadius = data.AlertRadius;
        _bloodVFX = data.BloodVFX;
        _explodeVFX = data.ExplodeVFX;
        _rewardLoot = data.RewardLoot;
        _moneyType = data.MoneyType;

        var trail = PoolManager.GetPool(_trailPrefab, transform.position);
        _mover = new EnemyMover(transform, _model, trail);
        _animatorController = new EnemyAnimator(_animator);
        _stateMachine = new EnemyStateMachine(this, _mover, _animatorController, data, spawnPoints, currentPointIndex);

        AnimateSpawn();
    }

    public void TakeDamage(float damage)
    {
        if (CurrentHealth <= 0)
            return;

        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            Died?.Invoke(this);

            _tween.Kill();
            _tween = transform.DOScale(Vector3.one * 1.5f, 0.25f).SetEase(Ease.InOutBack);
            _tween.OnComplete(() =>
            {
                _mover.DeactivateTrail();
                PoolManager.GetPool(_explodeVFX, transform.position);
                DropLoot();
                PoolManager.SetPool(this);
            });
        }
        else
        {
            DamageTaked?.Invoke(damage);
            PoolManager.GetPool(_bloodVFX, transform.position);
        }
    }

    public void Panic()
    {
        _stateMachine.EnterPanic();
    }

    private void DropLoot()
    {
        var randomLootCount = UnityEngine.Random.Range(1, Reward + 1);

        for (int i = 0; i < randomLootCount; i++)
        {
            var rewardLoot = PoolManager.GetPool(_rewardLoot, transform.position);
            rewardLoot.Spawn(Reward / randomLootCount, _moneyType);
        }
    }

    private void AnimateSpawn()
    {
        transform.localScale = Vector3.zero;
        _tween.Kill();
        _tween = transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }

    private void Update()
    {
        _stateMachine.Update();
    }

    private void OnDestroy()
    {
        _tween.Kill();
    }
}