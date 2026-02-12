using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct CircleMovementSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<MoveSpeed>();
        state.RequireForUpdate<Radius>();
        state.RequireForUpdate<CircleCenter>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        float elapsedTime = (float)SystemAPI.Time.ElapsedTime;

        var job = new CircleMovementJob
        {
            DeltaTime = deltaTime,
            ElapsedTime = elapsedTime
        };

        state.Dependency = job.ScheduleParallel(state.Dependency);
    }
}

[BurstCompile]
public partial struct CircleMovementJob : IJobEntity
{
    public float DeltaTime;
    public float ElapsedTime;

    void Execute(ref LocalTransform transform, in MoveSpeed speed, in Radius radius, in CircleCenter center)
    {
        float angle = ElapsedTime * speed.Value;

        float3 offset = new float3(
            radius.Value * math.cos(angle),
            0,
            radius.Value * math.sin(angle)
        );

        transform.Position = center.Value + offset;

        float3 direction = math.normalize(offset);
        if (!math.any(math.isnan(direction)) && math.lengthsq(direction) > 0.01f)
        {
            transform.Rotation = quaternion.LookRotation(direction, new float3(0, 1, 0));
        }
    }
}