using Unity.Entities;
using Unity.Transforms;

[GenerateAuthoringComponent]
public struct FlyingEnemyLogicComponent : IComponentData 
{
    public Translation destination;
    public bool isTargetingPlayer;
    public float rotationSpeed;
}
