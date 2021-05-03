using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

public class ImpactBounceSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.WithAll<ImpactBounceComponent>().ForEach((Entity entity, ref Translation translation, ref PhysicsVelocity velocity, ref ImpactBounceComponent impact) =>
        {
            float3 direction = math.normalize(translation.Value - impact.Value.Value);
            direction = new float3(direction.x, 0f, direction.y);

            velocity.Linear += direction * 20f;

            PostUpdateCommands.RemoveComponent<ImpactBounceComponent>(entity);
        });
    }
}
