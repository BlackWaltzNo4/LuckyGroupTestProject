using Unity.Entities;
using Unity.Mathematics;
 
[GenerateAuthoringComponent]
public struct ArrowComponent : IComponentData
{
    public float3 speed;
    public bool isDestroyed;
    public bool isBounced;
}
