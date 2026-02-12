using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct ZigzagMovementSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<ZigzagMovement>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        float elapsedTime = (float)SystemAPI.Time.ElapsedTime;

        var job = new ZigzagJob
        {
            DeltaTime = deltaTime,
            ElapsedTime = elapsedTime
        };

        state.Dependency = job.ScheduleParallel(state.Dependency);
    }
}

[BurstCompile]
public partial struct ZigzagJob : IJobEntity
{
    public float DeltaTime;
    public float ElapsedTime;

    void Execute(ref LocalTransform transform, ref ZigzagMovement movement)
    {
        if (movement.StartTime == 0f)
        {
            movement.StartTime = ElapsedTime;
        }

        float time = ElapsedTime - movement.StartTime;

        float forward = time * movement.ForwardSpeed;

        float zigzag = movement.Amplitude * math.sin(time * movement.Frequency);

        transform.Position = movement.StartPosition + new float3(
            zigzag,
            0f,
            forward
        );
    }
}