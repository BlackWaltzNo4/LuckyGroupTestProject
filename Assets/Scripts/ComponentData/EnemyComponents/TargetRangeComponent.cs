using Unity.Entities;

[GenerateAuthoringComponent]
public struct TargetRangeComponent : IComponentData
{
    public float minimalRange;
    public float maximalRange;
}
