using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class ZigzagBallAuthoring : MonoBehaviour
{
    [Header("Movement Settings")]
    public float ForwardSpeed = 5f;
    public float Amplitude = 3f;
    public float Frequency = 2f;

    [Header("Visual")]
    public Mesh Mesh;
    public Material Material;
}

public class ZigzagBallBaker : Baker<ZigzagBallAuthoring>
{
    public override void Bake(ZigzagBallAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new LocalTransform
        {
            Position = authoring.transform.position,
            Rotation = quaternion.identity,
            Scale = 1f
        });
        AddComponent(entity, new LocalToWorld());

        AddComponent(entity, new ZigzagMovement
        {
            ForwardSpeed = authoring.ForwardSpeed,
            Amplitude = authoring.Amplitude,
            Frequency = authoring.Frequency,
            StartTime = 0f,
            StartPosition = authoring.transform.position
        });

        var renderMeshArray = new RenderMeshArray(
            new Material[] { authoring.Material },
            new Mesh[] { authoring.Mesh }
        );
        AddSharedComponentManaged(entity, renderMeshArray);

        AddComponent(entity, MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0));
    }
}