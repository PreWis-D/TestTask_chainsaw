using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public float ResistancePercentage { get; private set; }
    [field: SerializeField] public ParticleSystem TrailPrefab { get; private set; }
}