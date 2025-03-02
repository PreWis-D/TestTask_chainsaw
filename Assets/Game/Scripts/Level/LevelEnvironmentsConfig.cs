using UnityEngine;

[CreateAssetMenu(fileName = "LevelEnvironmentsConfig", menuName = "Configs/LevelEnvironmentsConfig")]
public class LevelEnvironmentsConfig : ScriptableObject
{
    [field: SerializeField] public LevelEnvironment[] Environments { get; private set; }
}