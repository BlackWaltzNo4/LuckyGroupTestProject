using Unity.Entities;

[GenerateAuthoringComponent]
public struct AnimatedCharacterComponent : IComponentData
{
    public Entity animatorEntity;
    public float velocity;
}
