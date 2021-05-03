using Unity.Entities;

public struct ImpactBounceCooldownComponent : IComponentData
{
    public float ElapsedTime;
    public float TargetDuration;
}
