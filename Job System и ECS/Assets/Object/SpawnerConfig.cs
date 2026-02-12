using Unity.Entities;
using Unity.Mathematics;

public struct SpawnerConfig : IComponentData
{
    public Entity Prefab;
    public int GridWidth;
    public int GridHeight;
    public float Spacing;
    public float BaseSpeed;
    public float BaseRadius;
}