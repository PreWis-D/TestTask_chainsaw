using System;
using UnityEngine;

[Serializable]
public struct EnemyPrefabData
{
    [field: SerializeField] public EnemyType Type { get; private set; }
    [field: SerializeField] public MoneyType MoneyType { get; private set; }
    [field: SerializeField] public Enemy Prefab { get; private set; }
    [field: SerializeField] public ParticleController BloodVFX { get; private set; }
    [field: SerializeField] public ParticleController ExplodeVFX { get; private set; }
    [field: SerializeField] public RewardLoot RewardLoot { get; private set; }
    [field: SerializeField] public float StartHealth { get; private set; }
    [field: SerializeField] public int Reward { get; private set; }
    [field: SerializeField] public float IdleTime { get; private set; }
    [field: SerializeField] public float PanicTime { get; private set; }
    [field: SerializeField] public float IdleMoveSpeed { get; private set; }
    [field: SerializeField] public float PanicMoveSpeed { get; private set; }
    [field: SerializeField] public float AlertRadius { get; private set; }
}