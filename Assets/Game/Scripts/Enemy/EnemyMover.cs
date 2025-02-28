using UnityEngine;

public class EnemyMover
{
    private readonly Transform _enemyTransform;
    private readonly float _stopDistance = 0.1f;
    
    private float _speed = 1f;

    public Transform Target { get; private set; }

    public EnemyMover(Transform transform)
    {
        _enemyTransform = transform;
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    public void SetTarget(Transform transform)
    {
        Target = transform;
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
    }
}