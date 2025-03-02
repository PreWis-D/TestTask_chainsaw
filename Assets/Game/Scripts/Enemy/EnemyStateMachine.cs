using Assets.Game.Scripts.Enemy;
using SpawnerExample;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    private readonly Enemy _enemy;
    private readonly EnemyMover _mover;
    private readonly EnemyAnimator _animator;
    private readonly float _idleTime;
    private readonly float _panicTime;
    private readonly float _idleMoveSpeed;
    private readonly float _panicMoveSpeed;
    private readonly float _alertRaius;
    private readonly List<SpawnPoint> _spawnPoints = new List<SpawnPoint>();

    private SpawnPoint _currentPoint;
    private EnemyState _state = EnemyState.Idle;
    private float _timer = 0;

    public EnemyStateMachine(Enemy enemy, EnemyMover enemyMover, EnemyAnimator animator,
        EnemyPrefabData data, List<SpawnPoint> spawnPoints, int currentPointIndex)
    {
        _enemy = enemy;
        _mover = enemyMover;
        _animator = animator;
        _idleTime = data.IdleTime;
        _panicTime = data.PanicTime;
        _idleMoveSpeed = data.IdleMoveSpeed;
        _panicMoveSpeed = data.PanicMoveSpeed;
        _alertRaius = data.AlertRadius;

        _spawnPoints = spawnPoints;
        _currentPoint = _spawnPoints[currentPointIndex];
        _currentPoint.TakePosition();

        _enemy.DamageTaked += OnDamageTaked;
        _enemy.Died += EnterDie;
    }

    public void Update()
    {
        switch (_state)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Panic:
                Panic();
                break;
        }
    }

    private void EnterIdle()
    {
        _timer = 0;
        _state = EnemyState.Idle;
        _animator.Idle();
    }

    private void Idle()
    {
        _timer += Time.deltaTime;

        if (_timer > _idleTime)
        {
            SearchMoveTarget();
            EnterMove();
        }
    }

    private void SearchMoveTarget()
    {
        List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
        foreach (SpawnPoint spawnPoint in _spawnPoints)
            if (spawnPoint.IsEmpty)
                spawnPoints.Add(spawnPoint);

        int randomIndex = Random.Range(0, spawnPoints.Count);
        _currentPoint.DropPosition();
        _currentPoint = spawnPoints[randomIndex];
        _currentPoint.TakePosition();
        _mover.SetTarget(_currentPoint.transform);
    }

    private void EnterMove()
    {
        _state = EnemyState.Move;
        _mover.SetSpeed(_idleMoveSpeed);
        _animator.Move();
    }

    private void Move()
    {
        if (_mover.Target)
            _mover.Update();
        else
            EnterIdle();
    }

    private void OnDamageTaked(float damage)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_enemy.transform.position, _alertRaius);
        for (int i = 0; i < colliders.Length; i++)
        {
            var otherEnemy = colliders[i].transform.GetComponent<Enemy>();
            if (otherEnemy)
                otherEnemy.Panic();
        }

        EnterPanic();
    }

    public void EnterPanic()
    {
        _timer = 0;
        _mover.SetSpeed(_panicMoveSpeed);
        _state = EnemyState.Panic;
        _animator.Panic();
    }

    private void Panic()
    {
        _timer += Time.deltaTime;

        if (_timer > _panicTime)
        {
            EnterIdle();
        }
        else
        {
            if (_mover.Target)
                _mover.Update();
            else
                SearchMoveTarget();
        }
    }

    private void EnterDie(IDamagable damagable)
    {
        _enemy.DamageTaked -= OnDamageTaked;
        _enemy.Died -= EnterDie;
    }
}