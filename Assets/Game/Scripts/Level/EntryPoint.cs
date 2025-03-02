using Reflex.Attributes;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private CamController _camController;
    [SerializeField] private Player _playerPrefab;

    private UIHandler _uiHandler;
    private LevelEnvironmentsConfig _levelConfig;
    private EnemiesConfig _enemiesConfig;
    private PlayerConfig _playerConfig;

    [Inject]
    private void Construct(UIHandler uIHandler, LevelEnvironmentsConfig levelEnvironmentsConfig, EnemiesConfig enemiesConfig,
        PlayerConfig playerConfig)
    {
        _uiHandler = uIHandler;
        _levelConfig = levelEnvironmentsConfig;
        _enemiesConfig = enemiesConfig;
        _playerConfig = playerConfig;
    }

    private void Start()
    {
        var level = PoolManager.GetPool(_levelConfig.Environments[0], Vector3.zero);
        level.Init(_enemiesConfig);

        var player = PoolManager.GetPool(_playerPrefab, level.PlayerSpawnPoint.position);
        player.Init(_uiHandler, _playerConfig);

        _camController.Init(player, level);
    }
}
