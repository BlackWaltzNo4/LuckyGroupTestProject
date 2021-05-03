using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class MeleeAttackSystem : ComponentSystem
{   
    protected override void OnUpdate()
    {
        Entities.WithAll<EnemyMeleeType>().ForEach((Entity entity, ref AttackCooldownComponent cooldown, ref StateComponent state, ref Translation translation) =>
        {
            var target = EntityManager.GetBuffer<TargetBuffer>(entity);

            if (state.Value == State.isMoving) return;

            var dt = Time.DeltaTime;
            cooldown.ElapsedTime += dt;

            if (target.Length == 0) return;

            var targetTranslation = EntityManager.GetComponentData<Translation>(target[0].Value);
            var targetDamageBuffer = EntityManager.GetBuffer<DamageBuffer>(target[0].Value);

            if (cooldown.ElapsedTime < cooldown.TargetDuration) return;
            else
            {
                if (math.distance(translation.Value, targetTranslation.Value) <= 2f)
                {
                    targetDamageBuffer.Add(new DamageBuffer { Value = 5 });
                    cooldown.ElapsedTime = 0;

                    if (EntityManager.HasComponent<AnimatedCharacterComponent>(entity))
                    {
                        var animator = EntityManager.GetComponentObject<Animator>(EntityManager.GetComponentData<AnimatedCharacterComponent>(entity).animatorEntity);
                        animator.SetBool("Attack", true);
                    }
                }
            }
        });
    }
}
