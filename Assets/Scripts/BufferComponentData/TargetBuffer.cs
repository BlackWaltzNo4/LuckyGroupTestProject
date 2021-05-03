using Unity.Entities;

[InternalBufferCapacity(16)]
[GenerateAuthoringComponent]
public struct TargetBuffer : IBufferElementData
{
    public Entity Value;
}
