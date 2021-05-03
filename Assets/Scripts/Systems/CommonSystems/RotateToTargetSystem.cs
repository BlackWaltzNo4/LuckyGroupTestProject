using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;


public class RotateToTargetSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        var stateComponent = GetComponentDataFromEntity<StateComponent>();
        var translationComponent = GetComponentDataFromEntity<Translation>();

        Entities.WithAll<TargetBuffer, HealthComponent>().WithNone<EnemyFlyingType>().ForEach((Entity entity, ref Rotation rotation, DynamicBuffer<TargetBuffer> target) =>
        {
            if (target.Length == 0) return;

            var targetTranslation = translationComponent[target[0].Value];
            var playerTranslation = translationComponent[entity];
            var state = stateComponent[entity];

            if (state.Value == State.isIdle)
            {
                float3 direction = targetTranslation.Value - playerTranslation.Value;
                direction = new float3(direction.x, 0f, direction.z);
                rotation.Value = quaternion.LookRotation(direction, math.up());
            }
        });
    }
}
