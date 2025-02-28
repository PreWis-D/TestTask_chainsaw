using UnityEngine;

[CreateAssetMenu(fileName = "EnemiesCongig", menuName = "Configs/EnemisCongig")]
public class EnemiesConfig : ScriptableObject
{
    [field: SerializeField] public EnemyPrefabData[] EnemyPrefabDatas { get; private set; }
}
