using Unity.Entities;
using Unity.Transforms;

[InternalBufferCapacity(64)]
[GenerateAuthoringComponent]
public struct DamageBuffer : IBufferElementData
{
    public int Value;
}
