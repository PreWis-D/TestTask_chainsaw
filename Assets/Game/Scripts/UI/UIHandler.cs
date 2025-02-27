using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private JoystickVirtual _joystickVirtual;
    [SerializeField] private Player _player;

    private void Start()
    {
        _player.Init(_joystickVirtual);
    }
}
