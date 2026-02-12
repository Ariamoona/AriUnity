using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine.Jobs;

[BurstCompile]
public struct RotatingCircleJob : IJobParallelForTransform
{
    public float deltaTime;
    public float radius;
    public float speed;
    public float3 center;

    public void Execute(int index, TransformAccess transform)
    {
        float3 position = transform.position;

        float3 relativePos = position - center;
        float angle = math.atan2(relativePos.z, relativePos.x);

        angle += speed * deltaTime;

        float3 newPos = center;
        newPos.x += radius * math.cos(angle);
        newPos.z += radius * math.sin(angle);

        transform.position = newPos;

        float3 forward = new float3(math.cos(angle), 0, math.sin(angle));
        quaternion rotation = quaternion.LookRotation(forward, new float3(0, 1, 0));
        transform.rotation = rotation;
    }
}