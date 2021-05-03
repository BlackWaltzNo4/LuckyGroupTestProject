using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class CloseRangeDamageSystem : ComponentSystem
{   
    protected override void OnUpdate()
    {
        Entities.WithAll<EnemyTag>().WithNone<ImpactBounceCooldownComponent>().ForEach((Entity entity, ref Translation translation) =>
        {
            var target = EntityManager.GetBuffer<TargetBuffer>(entity);

            var targetTranslation = EntityManager.GetComponentData<Translation>(target[0].Value);
            var targetDamageBuffer = EntityManager.GetBuffer<DamageBuffer>(target[0].Value);

            if (!EntityManager.HasComponent<ImpactBounceComponent>(target[0].Value))
            {
                if (math.distance(translation.Value, targetTranslation.Value) <= 1.2f)
                {
                    targetDamageBuffer.Add(new DamageBuffer { Value = 1 });
                    PostUpdateCommands.AddComponent(target[0].Value, new ImpactBounceComponent { Value = translation });
                    PostUpdateCommands.AddComponent(entity, new ImpactBounceCooldownComponent { TargetDuration = 1f });
                }
            }
        });
    }
}
