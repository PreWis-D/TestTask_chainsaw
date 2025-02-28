using Reflex.Attributes;
using System;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [field: SerializeField] public JoystickVirtual JoystickVirtual {  get; private set; }
}