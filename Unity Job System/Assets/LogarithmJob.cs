using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

[BurstCompile]
public struct LogarithmJob : IJob
{
    public float randomNumber;
    public NativeArray<float> result;

    public void Execute()
    {
        float value = math.log(randomNumber);

        for (int i = 0; i < 1000; i++)
        {
            value += math.log(randomNumber * 0.999f) * 0.0001f;
        }

        result[0] = value;
    }
}