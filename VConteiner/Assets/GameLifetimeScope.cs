using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject obstaclePrefab;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<ScoreManager>(Lifetime.Singleton);

        builder.Register<ObstacleSpawner>(Lifetime.Singleton)
            .WithParameter(obstaclePrefab);

        builder.RegisterEntryPoint<GameManager>();

    }
}