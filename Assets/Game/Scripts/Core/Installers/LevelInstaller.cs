using Reflex.Core;
using UnityEngine;

public class LevelInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private EnemiesConfig _enemiesConfig;
    [SerializeField] private UIHandler _uiHandlerPrefab;

    private ContainerBuilder _containerBuilder;

    public void InstallBindings(ContainerBuilder containerBuilder)
    {
        _containerBuilder = containerBuilder;

        BindPlayerInput();
        BindEnemiesConfig();
        BindUIHandler();
    }

    private void BindPlayerInput()
    {
        _containerBuilder.AddSingleton(typeof(PlayerInput));
    }

    private void BindEnemiesConfig()
    {
        _containerBuilder.AddSingleton(_enemiesConfig);
    }
    
    private void BindUIHandler()
    {
        var uiHadler = Instantiate(
            _uiHandlerPrefab
            , Vector3.zero
            , Quaternion.identity
            , null);

        _containerBuilder
            .AddSingleton(uiHadler);
    }
}
