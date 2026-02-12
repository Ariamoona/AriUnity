using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct ZigzagSpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<ZigzagSpawnerConfig>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.Enabled = false;

        var config = SystemAPI.GetSingleton<ZigzagSpawnerConfig>();
        if (config.Prefab == Entity.Null) return;

        int totalCount = config.Count;

        using var entities = new NativeArray<Entity>(totalCount, Allocator.Temp);
        state.EntityManager.Instantiate(config.Prefab, entities);

        float spawnWidth = config.SpawnArea.x;
        float spawnDepth = config.SpawnArea.z;

        int columns = (int)math.sqrt(totalCount);
        int rows = totalCount / columns;

        int index = 0;
        for (int x = 0; x < columns; x++)
        {
            for (int z = 0; z < rows; z++)
            {
                if (index >= totalCount) break;

                var entity = entities[index];

                float3 position = new float3(
                    x * config.Spacing - (columns * config.Spacing) / 2,
                    0,
                    z * config.Spacing
                );

                state.EntityManager.SetComponentData(entity,
                    LocalTransform.FromPosition(position));

                state.EntityManager.SetComponentData(entity, new ZigzagMovement
                {
                    ForwardSpeed = config.BaseForwardSpeed + UnityEngine.Random.Range(-config.RandomRange, config.RandomRange),
                    Amplitude = config.BaseAmplitude + UnityEngine.Random.Range(-config.RandomRange, config.RandomRange),
                    Frequency = config.BaseFrequency + UnityEngine.Random.Range(-1f, 1f),
                    StartTime = 0f,
                    StartPosition = position
                });

                index++;
            }
        }

        Debug.Log($"<color=green>✅ Создано {index} мячиков с зигзагом!</color>");
    }
}