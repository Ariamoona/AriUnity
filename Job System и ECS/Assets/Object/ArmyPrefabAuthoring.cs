using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class ArmyPrefabAuthoring : MonoBehaviour
{
    public float DefaultSpeed = 2f;
    public float DefaultRadius = 5f;
    public Mesh Mesh;
    public Material Material;
}

public class ArmyPrefabBaker : Baker<ArmyPrefabAuthoring>
{
    public override void Bake(ArmyPrefabAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new LocalTransform
        {
            Position = float3.zero,
            Rotation = quaternion.identity,
            Scale = 1f
        });
        AddComponent(entity, new LocalToWorld());

        AddComponent(entity, new MoveSpeed { Value = authoring.DefaultSpeed });
        AddComponent(entity, new Radius { Value = authoring.DefaultRadius });
        AddComponent(entity, new CircleCenter { Value = float3.zero });

        var renderMeshDescription = new RenderMeshDescription(
            authoring.Mesh,
            authoring.Material,
           
            receiveShadows: true
        );

        RenderMeshUtility.AddComponents(
            entity,
            this, 
            renderMeshDescription,
            default, 
            MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0)
        );
    }
}