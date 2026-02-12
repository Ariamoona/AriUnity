using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class ZigzagSpawnerAuthoring : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject Prefab;
    public int Count = 1000;
    public float Spacing = 2f;
    public Vector3 SpawnArea = new Vector3(20, 0, 0);

    [Header("Movement Settings")]
    public float BaseForwardSpeed = 5f;
    public float BaseAmplitude = 3f;
    public float BaseFrequency = 2f;
    public float RandomRange = 2f;
}

public class ZigzagSpawnerBaker : Baker<ZigzagSpawnerAuthoring>
{
    public override void Bake(ZigzagSpawnerAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);

        AddComponent(entity, new ZigzagSpawnerConfig
        {
            Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic),
            Count = authoring.Count,
            Spacing = authoring.Spacing,
            SpawnArea = authoring.SpawnArea,
            BaseForwardSpeed = authoring.BaseForwardSpeed,
            BaseAmplitude = authoring.BaseAmplitude,
            BaseFrequency = authoring.BaseFrequency,
            RandomRange = authoring.RandomRange
        });
    }
}