using Unity.Entities;
 
[GenerateAuthoringComponent]
public struct SpawnEnemiesComponent : IComponentData
{
    public Entity RangeEnemyPrefab;
    public int RangeEnemyAmount;

    public Entity MeleeEnemyPrefab;
    public int MeleeEnemyAmount;

    public Entity FlyingEnemyPrefab;
    public int FlyingEnemyAmount;
}
