using Reflex.Core;
using UnityEngine;

public class LevelInstaller : MonoBehaviour, IInstaller
{
    private ContainerBuilder _containerBuilder;

    public void InstallBindings(ContainerBuilder containerBuilder)
    {
        _containerBuilder = containerBuilder;

        BindPlayerInput();
    }

    private void BindPlayerInput()
    {
        _containerBuilder.AddSingleton(typeof(PlayerInput));
    }
}
