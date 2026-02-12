using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class ArmySpawnerAuthoring : MonoBehaviour
{
    public GameObject Prefab;
    public int GridWidth = 224;
    public int GridHeight = 224;
    public float Spacing = 2f;
    public float BaseSpeed = 2f;
    public float BaseRadius = 5f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 startPos = transform.position;

        for (int x = 0; x < GridWidth; x++)
        {
            for (int y = 0; y < GridHeight; y++)
            {
                Vector3 pos = startPos + new Vector3(x * Spacing, 0, y * Spacing);
                Gizmos.DrawWireCube(pos, Vector3.one * 0.5f);
            }
        }
    }
}

public class ArmySpawnerBaker : Baker<ArmySpawnerAuthoring>
{
    public override void Bake(ArmySpawnerAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);

        AddComponent(entity, new SpawnerConfig
        {
            Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic),
            GridWidth = authoring.GridWidth,
            GridHeight = authoring.GridHeight,
            Spacing = authoring.Spacing,
            BaseSpeed = authoring.BaseSpeed,
            BaseRadius = authoring.BaseRadius
        });
    }
}