using Unity.Entities;
using Unity.Jobs;
using Unity.Physics.Systems;
using Unity.Physics;
using Unity.Mathematics;

public class ArrowDamageSystem : JobComponentSystem
{
    struct CollisionEventSystemJob : ITriggerEventsJob
    {
        public ComponentDataFromEntity<ArrowComponent> arrowRef;
        public ComponentDataFromEntity<PlayerArrow> playerArrowRef;
        public ComponentDataFromEntity<HealthComponent> healthRef;
        public ComponentDataFromEntity<EnemyTag> enemyRef;
        public BufferFromEntity<DamageBuffer> damageBufferRef;

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity hitEntity, arrowEntity;

            if (arrowRef.HasComponent(triggerEvent.EntityA)) 
            {
                hitEntity = triggerEvent.EntityB;
                arrowEntity = triggerEvent.EntityA;
            } 
            else if (arrowRef.HasComponent(triggerEvent.EntityB)) 
            {
                hitEntity = triggerEvent.EntityA;
                arrowEntity = triggerEvent.EntityB;
            }
            else return;


            var arrow = arrowRef[arrowEntity];
            arrow.isDestroyed = true;
            arrow.isBounced = true;
            arrowRef[arrowEntity] = arrow;

            if (healthRef.HasComponent(hitEntity))
            {
                if (enemyRef.HasComponent(hitEntity))
                {
                    if (playerArrowRef.HasComponent(arrowEntity))
                    {
                        var damageBuffer = damageBufferRef[hitEntity];
                        damageBuffer.Add(new DamageBuffer { Value = 1 });
                    }
                }
                else
                {
                    var damageBuffer = damageBufferRef[hitEntity];
                    damageBuffer.Add(new DamageBuffer { Value = 1 });
                }
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
        job.arrowRef = GetComponentDataFromEntity<ArrowComponent>(isReadOnly: false);
        job.playerArrowRef = GetComponentDataFromEntity<PlayerArrow>(isReadOnly: false);
        job.healthRef = GetComponentDataFromEntity<HealthComponent>(isReadOnly: false);
        job.enemyRef = GetComponentDataFromEntity<EnemyTag>(isReadOnly: false);
        job.damageBufferRef = GetBufferFromEntity<DamageBuffer>(isReadOnly: false);
        var jobResult = job.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorldSystem.PhysicsWorld, inputDeps);
 
        var commandBuffer = endSimulationCommandBuffer.CreateCommandBuffer().AsParallelWriter();
        var result = Entities.ForEach((Entity entity, int entityInQueryIndex, ref ArrowComponent arrow) => 
        {
            if (arrow.isDestroyed)
            {
                commandBuffer.AddComponent(entityInQueryIndex, entity, new SelfDestructComponent { TargetDuration = .1f });
                commandBuffer.RemoveComponent<ArrowComponent>(entityInQueryIndex, entity);
            }

            if (arrow.isBounced)
            {
                commandBuffer.RemoveComponent<ArrowComponent>(entityInQueryIndex, entity);
                commandBuffer.SetComponent(entityInQueryIndex, entity, new PhysicsVelocity { Linear = new float3(0f, 2f, 0f), Angular = new float3(5f, 1f, 1f) });
            }
        }).Schedule(jobResult);
 
        endSimulationCommandBuffer.AddJobHandleForProducer(result);
        return result;
    }
}
