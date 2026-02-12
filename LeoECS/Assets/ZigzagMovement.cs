using Unity.Entities;
using Unity.Mathematics;

public struct ZigzagMovement : IComponentData
{
    public float ForwardSpeed; 
    public float Amplitude;    
    public float Frequency;     
    public float StartTime;    
    public float3 StartPosition;
}