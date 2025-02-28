using SpawnerExample;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    private readonly Enemy _enemy;
    private readonly EnemyMover _mover;
    private readonly float _idleTime;
    private readonly float _panicTime;
    private readonly float _idleMoveSpeed;
    private readonly float _panicMoveSpeed;
    private readonly List<SpawnPoint> _spawnPoints = new List<SpawnPoint>();

    private SpawnPoint _currentPoint;
    private EnemyState _state = EnemyState.Idle;
    private float _timer = 0;

    public EnemyStateMachine(Enemy enemy, EnemyMover enemyMover, EnemyPrefabData data, List<SpawnPoint> spawnPoints, int currentPointIndex)
    {
        _enemy = enemy;
        _mover = enemyMover;
        _idleTime = data.IdleTime;
        _panicTime = data.PanicTime;
        _idleMoveSpeed = data.IdleMoveSpeed;
        _panicMoveSpeed = data.PanicMoveSpeed;

        _spawnPoints = spawnPoints;
        _currentPoint = _spawnPoints[currentPointIndex];
        _currentPoint.TakePosition();

        _enemy.DamageTaked += EnterPanic;
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
            case EnemyState.Die:
                Die();
                break;
        }
    }

    private void EnterIdle()
    {
        _timer = 0;
        _state = EnemyState.Idle;
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
    }

    private void EnterMove()
    {
        _state = EnemyState.Move;
        _mover.SetSpeed(_idleMoveSpeed);
        _mover.SetTarget(_currentPoint.transform);
    }

    private void Move()
    {
        if (_mover.Target)
            _mover.Update();
        else
            EnterIdle();
    }

    private void EnterPanic(float damage)
    {
        _timer = 0;
        _state = EnemyState.Panic;
        _mover.SetSpeed(_panicMoveSpeed);
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
        _state = EnemyState.Die;
    }

    private void Die()
    {
        PoolManager.SetPool(_enemy);
    }

    public void OnDestroy()
    {
        _enemy.DamageTaked -= EnterPanic;
        _enemy.Died -= EnterDie;
    }
}