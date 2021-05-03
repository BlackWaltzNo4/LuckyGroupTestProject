using Unity.Entities;
 
[GenerateAuthoringComponent]
public struct StateComponent : IComponentData
{
    public State Value;
}
