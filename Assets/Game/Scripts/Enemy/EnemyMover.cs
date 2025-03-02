using UnityEngine;
using static UnityEngine.ParticleSystem;

public class EnemyMover
{
    private readonly Transform _enemyTransform;
    private readonly Transform _model;
    private readonly ParticleSystem _trail;
    private readonly float _stopDistance = 0.1f;

    private Vector3 _trailOffset = new Vector3(0, -0.515f, 0);
    private float _speed = 1f;

    public Transform Target { get; private set; }

    public EnemyMover(Transform transform, Transform model, ParticleSystem trail)
    {
        _enemyTransform = transform;
        _model = model;
        _trail = trail;
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    public void SetTarget(Transform transform)
    {
        Target = transform;
    }

    public void DeactivateTrail()
    {
        PoolManager.SetPool(_trail);
    }

    public void Update()
    {
        if (Target)
            Move();
    }

    private void Move()
    {
        var direction = Target.position - _enemyTransform.position;
        var distance = Vector3.Distance(_enemyTransform.position, Target.position);

        if (distance < _stopDistance)
            Target = null;
        else
            _enemyTransform.position = Vector2.MoveTowards(
                _enemyTransform.position, Target.position, _speed * Time.deltaTime);

        _trail.transform.position = _enemyTransform.position + _trailOffset;

        var rotation = new Vector3(0, direction.x < 0 ? 180 : 0, 0);
        _model.eulerAngles = rotation;
    }
}