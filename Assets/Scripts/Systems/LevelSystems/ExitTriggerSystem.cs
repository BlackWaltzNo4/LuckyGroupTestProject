using Unity.Entities;
using Unity.Jobs;
using Unity.Physics.Systems;
using Unity.Physics;
using Unity.Mathematics;
using System.Diagnostics;

public class ExitTriggerSystem : JobComponentSystem
{
    bool levelComplete = false;

    struct CollisionEventSystemJob : ITriggerEventsJob
    {
        public ComponentDataFromEntity<PlayerComponent> playerRef;
        public ComponentDataFromEntity<ExitTrigger> exitTriggerRef;

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity hitEntity, triggerEntity;

            if (exitTriggerRef.HasComponent(triggerEvent.EntityA)) 
            {
                hitEntity = triggerEvent.EntityB;
                triggerEntity = triggerEvent.EntityA;
            } 
            else if (exitTriggerRef.HasComponent(triggerEvent.EntityB)) 
            {
                hitEntity = triggerEvent.EntityA;
                triggerEntity = triggerEvent.EntityB;
            }
            else return;

            if (exitTriggerRef.HasComponent(triggerEntity) && playerRef.HasComponent(hitEntity))
            {
                var player = playerRef[hitEntity];
                player.levelComplete = true;
                playerRef[hitEntity] = player;
            }

        }
    }
 
    BuildPhysicsWorld buildPhysicsWorldSystem;
    StepPhysicsWorld stepPhysicsWorld;
    EndSimulationEntityCommandBufferSystem endSimulationCommandBuffer;
 
    protected override void OnCreate()
    {
        buildPhysicsWorldSystem = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        endSimulationCommandBuffer = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }
 
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new CollisionEventSystemJob();        
        job.playerRef = GetComponentDataFromEntity<PlayerComponent>(isReadOnly: false);
        job.exitTriggerRef = GetComponentDataFromEntity<ExitTrigger>(isReadOnly: false);
        var jobResult = job.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorldSystem.PhysicsWorld, inputDeps);
        endSimulationCommandBuffer.AddJobHandleForProducer(jobResult);

        var commandBuffer = endSimulationCommandBuffer.CreateCommandBuffer().AsParallelWriter();
        var result = Entities.ForEach((Entity entity, int entityInQueryIndex, ref PlayerComponent player) =>
        {
            if (player.levelComplete)
            {
                commandBuffer.AddComponent(entityInQueryIndex, entity, new LevelComplete { });
            }

        }).Schedule(jobResult);

        endSimulationCommandBuffer.AddJobHandleForProducer(result);
        return result;
    }
}
