using SpawnerExample;
using UnityEngine;

public class LevelEnvironment : MonoBehaviour
{
    [field: SerializeField] public Transform PlayerSpawnPoint { get; private set; }
    [field: SerializeField] public SpawnZone[] SpawnZones { get; private set; }

    public void Init(EnemiesConfig enemiesConfig)
    {
        for (int i = 0; i < SpawnZones.Length; i++)
            SpawnZones[i].Init(enemiesConfig);
    }
}
