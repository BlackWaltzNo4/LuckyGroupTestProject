using Unity.Entities;

public struct SelfDestructComponent : IComponentData
{
    public float ElapsedTime;
    public float TargetDuration;
}
