using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class SelfDestructSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.WithAll<SelfDestructComponent>().ForEach((Entity entity, ref SelfDestructComponent timer) => 
        {
            var dt = Time.DeltaTime;
            timer.ElapsedTime += dt;

            if (timer.TargetDuration < timer.ElapsedTime)
            {
                PostUpdateCommands.DestroyEntity(entity);
            }
        });

        Entities.WithAll<SelfDestructComponent>().ForEach((Entity entity, ref SelfDestructComponent timer, ref AnimatedCharacterComponent rig) =>
        {
            var dt = Time.DeltaTime;
            timer.ElapsedTime += dt;

            if (timer.TargetDuration < timer.ElapsedTime)
            {
                var animator = EntityManager.GetComponentObject<Animator>(rig.animatorEntity);
                GameObject.Destroy(animator.gameObject);

                PostUpdateCommands.DestroyEntity(entity);
            }
        });
    }
}
