using Unity.Entities;
using Unity.Mathematics;

public struct ZigzagSpawnerConfig : IComponentData
{
    public Entity Prefab;
    public int Count;
    public float Spacing;
    public float3 SpawnArea;
    public float BaseForwardSpeed;
    public float BaseAmplitude;
    public float BaseFrequency;
    public float RandomRange;
}