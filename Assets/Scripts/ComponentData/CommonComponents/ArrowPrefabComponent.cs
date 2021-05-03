using Unity.Entities;
 
[GenerateAuthoringComponent]
public struct ArrowPrefabComponent : IComponentData
{
    public Entity prefab;
    public Entity marker;
    public float speed;
}