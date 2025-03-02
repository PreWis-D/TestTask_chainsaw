using System;
using UnityEngine;

public class PlayerMovement
{
    private readonly Transform _playerTransform;
    private readonly JoystickVirtual _joystick;
    private readonly ParticleSystem _trail;
    private readonly float _edgeStartMove = 0.2f;
    private readonly float _resistancePercentage;
    private readonly float _startSpeed;
    private readonly Vector3 _trailOffset = new Vector3(0, -0.515f, 0);

    private Vector3 _currentMovement;
    private float _currentSpeed;

    public bool IsMovePressed { get; private set; }

    public PlayerMovement(Transform transform, JoystickVirtual joystickVirtual, ParticleSystem trail, PlayerConfig playerConfig)
    {
        _playerTransform = transform ?? throw new ArgumentNullException(nameof(transform));
        _joystick = joystickVirtual ?? throw new ArgumentNullException(nameof(joystickVirtual));
        _trail = trail ?? throw new ArgumentNullException(nameof(trail));

        _startSpeed = playerConfig.Speed;
        _resistancePercentage = playerConfig.ResistancePercentage;
    }

    public void FixedUpdate()
    {
        CheckMove();
        SetCurrentMovement();
        Move();
    }

    public void TurnResistance(bool isResistance)
    {
        _currentSpeed = isResistance ? _startSpeed * (1 - _resistancePercentage * 0.01f) : _startSpeed;
    }

    private void Move()
    {
        Vector3 direction = GetDirection();

        if (direction != Vector3.zero)
            Rotate(direction);

        Vector3 newPosition = _playerTransform.position + direction;

        _playerTransform.position = newPosition;
        _trail.transform.position = newPosition + _trailOffset;
    }

    private void Rotate(Vector3 direction)
    {
        var rotation = new Vector3(0, direction.x < 0 ? 180 : 0, 0);
        _playerTransform.eulerAngles = rotation;
    }

    private Vector3 GetDirection()
    {
        return new Vector3(_currentMovement.x, _currentMovement.y, 0) * _currentSpeed * Time.deltaTime;
    }

    private void CheckMove()
    {
        IsMovePressed =
            _joystick.Direction.x > _edgeStartMove
            || _joystick.Direction.x < -_edgeStartMove
            || _joystick.Direction.y > _edgeStartMove
            || _joystick.Direction.y < -_edgeStartMove;
    }

    private void SetCurrentMovement()
    {
        _currentMovement.x = _joystick.Direction.x;
        _currentMovement.y = _joystick.Direction.y;
        _currentMovement.Normalize();
    }
}
