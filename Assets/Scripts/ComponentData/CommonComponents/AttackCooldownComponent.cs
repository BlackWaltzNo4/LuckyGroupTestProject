using Unity.Entities;

[GenerateAuthoringComponent]
public struct AttackCooldownComponent : IComponentData
{
    public float ElapsedTime;
    public float TargetDuration;
}
