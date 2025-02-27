using System;
using UnityEditor;
using UnityEngine;

public class Chainsaw : MonoBehaviour
{
    private Transform _playerTransform;
    private JoystickVirtual _joystick;

    public void Init(Transform playerTransform, JoystickVirtual joystickVirtual)
    {
        _playerTransform = playerTransform;
        _joystick = joystickVirtual;
    }

    public void Update()
    {
        HandleDirection();
    }

    private void HandleDirection()
    {

    }
}