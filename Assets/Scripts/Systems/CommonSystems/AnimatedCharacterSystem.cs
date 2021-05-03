using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine.AI;

public class AnimatedCharacterSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.WithAny<PlayerComponent, EnemyFlyingType>().ForEach((Entity entity, ref AnimatedCharacterComponent character, ref PhysicsVelocity velocity) => 
        {
            var animator = EntityManager.GetComponentObject<Animator>(character.animatorEntity);
            character.velocity = math.length(velocity.Linear);
            animator.SetFloat("Speed", math.length(velocity.Linear));
        });

        Entities.WithNone<UpdateNavMeshAgent>().ForEach((Entity entity, ref NavMeshAgentComponent agent) =>
        {
            if (EntityManager.HasComponent<AnimatedCharacterComponent>(agent.moveEntity))
            {
                var parent = EntityManager.GetComponentData<AnimatedCharacterComponent>(agent.moveEntity);
                var animator = EntityManager.GetComponentObject<Animator>(parent.animatorEntity);
                var navMeshAgent = EntityManager.GetComponentObject<NavMeshAgent>(entity);

                var velocity = navMeshAgent.velocity.magnitude / navMeshAgent.speed;
                animator.SetFloat("Speed", velocity);
            }
        });
    }
}
