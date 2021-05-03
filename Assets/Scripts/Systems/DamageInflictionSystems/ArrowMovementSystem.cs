using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
 
public class ArrowMovementSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        JobHandle job = Entities.ForEach((ref ArrowComponent bullet, ref PhysicsVelocity velocity) => {
                velocity.Linear = bullet.speed;
            }).Schedule(inputDeps);
 
        return job;
    }
}
