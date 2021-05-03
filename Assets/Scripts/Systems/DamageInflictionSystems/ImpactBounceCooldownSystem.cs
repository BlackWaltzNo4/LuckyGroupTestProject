using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class ImpactBounceCooldownSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.WithAll<ImpactBounceCooldownComponent>().ForEach((Entity entity, ref ImpactBounceCooldownComponent timer) => 
        {
            var dt = Time.DeltaTime;
            timer.ElapsedTime += dt;

            if (timer.TargetDuration < timer.ElapsedTime)
            {
                //EntityManager.AddComponentData(entity, new SelfDestructComponent { TargetDuration = 1f});
                PostUpdateCommands.RemoveComponent<ImpactBounceCooldownComponent>(entity);
            }
        });
    }
}
