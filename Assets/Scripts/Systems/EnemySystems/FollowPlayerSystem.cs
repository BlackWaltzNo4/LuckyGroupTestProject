using UnityEngine;
using UnityEngine.AI;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class FollowPlayerSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        float3 targetPosition = float3.zero;
        Entities.ForEach((Entity entity, ref LocalToWorld transform, ref FollowTargetComponent tag) => 
        {
            targetPosition = transform.Position;
        });

        Entities.WithNone<UpdateNavMeshAgent>().ForEach((Entity entity, ref NavMeshAgentComponent agent, ref Rotation rotation, ref Parent parent) => 
        {
            if (!EntityManager.HasComponent<HealthComponent>(parent.Value)) return;
            if (EntityManager.HasComponent<WaitForCountdown>(parent.Value)) return;

            var navMeshAgent = EntityManager.GetComponentObject<NavMeshAgent>(entity);
            var range = EntityManager.GetComponentData<TargetRangeComponent>(parent.Value);
            var translation = EntityManager.GetComponentData<Translation>(parent.Value);

            if (navMeshAgent != null)
            {
                navMeshAgent.enabled = true;

                var distance = math.distance(translation.Value, targetPosition);
                var direction = math.normalize(translation.Value - targetPosition);

                if (distance > range.maximalRange)
                {
                    navMeshAgent.SetDestination(targetPosition + direction * range.maximalRange);
                }
                else if (distance < range.minimalRange)
                {
                    navMeshAgent.SetDestination(targetPosition + direction * range.minimalRange);
                }
                else
                {
                    navMeshAgent.SetDestination(translation.Value);
                }

                EntityManager.SetComponentData(agent.moveEntity, new Translation { Value = navMeshAgent.transform.position });
                //EntityManager.SetComponentData(agent.moveEntity, new Rotation { Value = navMeshAgent.transform.rotation });

                float velocity = navMeshAgent.velocity.magnitude / navMeshAgent.speed;
                if (velocity < .2f)
                {
                    navMeshAgent.transform.rotation = rotation.Value;
                    PostUpdateCommands.SetComponent(parent.Value, new StateComponent { Value = State.isIdle });
                }
                else
                {
                    EntityManager.SetComponentData(agent.moveEntity, new Rotation { Value = navMeshAgent.transform.rotation });
                    PostUpdateCommands.SetComponent(parent.Value, new StateComponent { Value = State.isMoving });//state.Value = State.isIdle;
                }
            }
        });
    }
}
