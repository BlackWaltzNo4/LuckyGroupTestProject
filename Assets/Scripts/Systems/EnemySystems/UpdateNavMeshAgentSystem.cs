using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine.AI;

public class UpdateNavMeshAgentSystem : ComponentSystem
{
    private NavMeshAgent agent;

    protected override void OnUpdate()
    {
        Entities.WithAll<UpdateNavMeshAgent>().ForEach((Entity entity, ref LocalToWorld localToWorld) =>
        {
            var parent = EntityManager.GetComponentData<Parent>(entity);
            var translation = EntityManager.GetComponentData<Translation>(parent.Value);
            var rotation = EntityManager.GetComponentData<Rotation>(parent.Value);

            var currenAgent = GameObject.Instantiate(agent);
            currenAgent.transform.position = translation.Value;
            currenAgent.transform.rotation = rotation.Value;

            EntityManager.AddComponentObject(entity, currenAgent);

            EntityManager.RemoveComponent<UpdateNavMeshAgent>(entity);
        });
    }

    public void SetNavMeshAgent(NavMeshAgent agent)
    {
        this.agent = agent;
    }
}
