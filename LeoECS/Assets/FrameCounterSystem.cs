using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct FrameCounterSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<FrameCounter>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var job = new FrameCounterJob();
        state.Dependency = job.ScheduleParallel(state.Dependency);
    }
}

[BurstCompile]
public partial struct FrameCounterJob : IJobEntity
{
    public void Execute(ref FrameCounter counter)
    {
        counter.Value++; 
    }
}