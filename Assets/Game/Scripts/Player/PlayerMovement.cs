using System;
using UnityEngine;

public class PlayerMovement
{
    private readonly Transform _playerTransform;
    private readonly JoystickVirtual Joystick;

    private Vector3 _currentMovement;
    private float _edgeStartMove = 0.2f;
    private float _speed = 5f;

    public bool IsMovePressed { get; private set; }

    public PlayerMovement(Transform transform, JoystickVirtual joystickVirtual)
    {
        _playerTransform = transform ?? throw new ArgumentNullException(nameof(transform));
        Joystick = joystickVirtual ?? throw new ArgumentNullException(nameof(joystickVirtual));
    }

    public void FixedUpdate()
    {
        CheckMove();
        SetCurrentMovement();
        Move();
    }

    private void Move()
    {
        Vector3 direction = GetDirection();

        if (direction != Vector3.zero)
            Rotate(direction);

        Vector3 newPosition = _playerTransform.position + direction;

        _playerTransform.position = newPosition;
    }

    private void Rotate(Vector3 direction)
    {
        var rotation = new Vector3(0, direction.x < 0 ? 180 : 0, 0);
        _playerTransform.eulerAngles = rotation;
    }

    private Vector3 GetDirection()
    {
        return new Vector3(_currentMovement.x, _currentMovement.y, 0) * _speed * Time.deltaTime;
    }

    private void CheckMove()
    {
        IsMovePressed =
            Joystick.Direction.x > _edgeStartMove
            || Joystick.Direction.x < -_edgeStartMove
            || Joystick.Direction.y > _edgeStartMove
            || Joystick.Direction.y < -_edgeStartMove;
    }

    private void SetCurrentMovement()
    {
        _currentMovement.x = Joystick.Direction.x;
        _currentMovement.y = Joystick.Direction.y;
        _currentMovement.Normalize();
    }
}
