using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
 
public class CheckLevelCompleteSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.WithAll<PlayerComponent, LevelComplete, HealthComponent>().ForEach((Entity entity) => 
        {
            Manager.LevelCompleteEvent();
            PostUpdateCommands.RemoveComponent<HealthComponent>(entity);
        });
    }
}
