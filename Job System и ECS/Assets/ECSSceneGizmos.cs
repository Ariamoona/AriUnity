using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class ECSSceneGizmos : MonoBehaviour
{
    [Header("Gizmos Settings")]
    public bool ShowGizmos = true;
    public int MaxGizmosToDraw = 1000;
    public float GizmoSize = 0.2f;
    public Color GizmoColor = Color.cyan;

    void OnDrawGizmos()
    {
        if (!ShowGizmos || !Application.isPlaying) return;

        var world = World.DefaultGameObjectInjectionWorld;
        if (world == null) return;

        var entityManager = world.EntityManager;

        EntityQuery query = entityManager.CreateEntityQuery(
            ComponentType.ReadOnly<LocalTransform>()
        );

        var entities = query.ToEntityArray(Unity.Collections.Allocator.Temp);
        int drawCount = math.min(entities.Length, MaxGizmosToDraw);

        Gizmos.color = GizmoColor;

        for (int i = 0; i < drawCount; i++)
        {
            var transform = entityManager.GetComponentData<LocalTransform>(entities[i]);
            Gizmos.DrawWireCube(transform.Position, Vector3.one * GizmoSize);
        }

        entities.Dispose();
        query.Dispose();
    }
}