using Unity.Entities;
using UnityEngine;

public class FrameCounterProvider : MonoBehaviour { }

public class FrameCounterBaker : Baker<FrameCounterProvider>
{
    public override void Bake(FrameCounterProvider authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);
        AddComponent(entity, new FrameCounter { Value = 0 });
    }
}