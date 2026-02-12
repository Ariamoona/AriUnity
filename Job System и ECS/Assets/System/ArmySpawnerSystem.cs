using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct ArmySpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<SpawnerConfig>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.Enabled = false;

        var config = SystemAPI.GetSingleton<SpawnerConfig>();
        if (config.Prefab == Entity.Null)
        {
            Debug.LogError("❌ Prefab не назначен!");
            return;
        }

        int totalCount = config.GridWidth * config.GridHeight;

        using var entities = new NativeArray<Entity>(totalCount, Allocator.Temp);
        state.EntityManager.Instantiate(config.Prefab, entities);

        int index = 0;
        for (int x = 0; x < config.GridWidth; x++)
        {
            for (int y = 0; y < config.GridHeight; y++)
            {
                var entity = entities[index];

                float3 position = new float3(
                    x * config.Spacing,
                    0,
                    y * config.Spacing
                );

                state.EntityManager.SetComponentData(entity,
                    LocalTransform.FromPosition(position));
                state.EntityManager.SetComponentData(entity,
                    new CircleCenter { Value = position });

                float speed = config.BaseSpeed + UnityEngine.Random.Range(-1f, 1f);
                float radius = config.BaseRadius + UnityEngine.Random.Range(-2f, 2f);

                state.EntityManager.SetComponentData(entity,
                    new MoveSpeed { Value = math.max(0.5f, speed) });

                state.EntityManager.SetComponentData(entity,
                    new Radius { Value = math.max(1f, radius) });

                index++;
            }
        }

        Debug.Log($"<color=green>✅ Создано {totalCount} сущностей!</color>");
    }
}