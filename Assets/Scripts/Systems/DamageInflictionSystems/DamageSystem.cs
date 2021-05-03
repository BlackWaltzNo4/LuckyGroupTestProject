using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using UnityEngine;

public class DamageSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.WithAny<EnemyRangeType, EnemyMeleeType, EnemyFlyingType, PlayerComponent>().ForEach((Entity entity, ref HealthComponent health, DynamicBuffer<DamageBuffer> damageBuffer) =>
        {
            var damageArray = damageBuffer.ToNativeArray(Allocator.Temp);
            damageBuffer.Clear();

            if (damageArray.Length == 0) return;

            foreach (var damage in damageArray)
            {
                health.Value = health.Value - damage.Value;
            }

            if (EntityManager.HasComponent<PlayerComponent>(entity)) Manager.SetHP(health.Value);
        });

        Entities.WithAny<EnemyRangeType, EnemyMeleeType>().ForEach((Entity entity, ref HealthComponent health) => 
        {
            //var animator = EntityManager.GetComponentObject<Animator>(character.animatorEntity);

            if (health.Value <= 0)
            {
                if (EntityManager.HasComponent<AnimatedCharacterComponent>(entity))
                {
                    var animator = EntityManager.GetComponentObject<Animator>(EntityManager.GetComponentData<AnimatedCharacterComponent>(entity).animatorEntity);
                    animator.SetBool("isDead", true);
                }
                Manager.IncreaseGold(2);

                EntityManager.RemoveComponent<HealthComponent>(entity);
                EntityManager.AddComponentData(entity, new SelfDestructComponent { TargetDuration = 3f});
            }
        });

        Entities.WithAny<PlayerComponent>().ForEach((Entity entity, ref HealthComponent health) =>
        {
            //var animator = EntityManager.GetComponentObject<Animator>(character.animatorEntity);

            if (health.Value <= 0)
            {
                if (EntityManager.HasComponent<AnimatedCharacterComponent>(entity))
                {
                    var animator = EntityManager.GetComponentObject<Animator>(EntityManager.GetComponentData<AnimatedCharacterComponent>(entity).animatorEntity);
                    animator.SetBool("isDead", true);
                }
                Manager.IncreaseGold(2);

                EntityManager.RemoveComponent<HealthComponent>(entity);
                //EntityManager.AddComponentData(entity, new SelfDestructComponent { TargetDuration = 3f });
            }
        });

        Entities.WithAny<EnemyFlyingType>().ForEach((Entity entity, ref HealthComponent health, ref PhysicsVelocity velocity, ref PhysicsGravityFactor factor, ref Translation translation) =>
        {
            //var animator = EntityManager.GetComponentObject<Animator>(character.animatorEntity);

            if (health.Value <= 0)
            {
                if (EntityManager.HasComponent<AnimatedCharacterComponent>(entity))
                {
                    var animator = EntityManager.GetComponentObject<Animator>(EntityManager.GetComponentData<AnimatedCharacterComponent>(entity).animatorEntity);
                    animator.SetBool("isDead", true);
                }
                Manager.IncreaseGold(2);

                var targetBuffer = EntityManager.GetBuffer<TargetBuffer>(entity);
                var targetPosition = EntityManager.GetComponentData<Translation>(targetBuffer[0].Value);
                velocity.Linear = math.normalize(translation.Value - targetPosition.Value) * 40f;
                velocity.Angular = new float3(5f, 1f, 4f);

                factor.Value = 6f;

                EntityManager.RemoveComponent<HealthComponent>(entity);
                EntityManager.AddComponentData(entity, new SelfDestructComponent { TargetDuration = 6f });
            }
        });
    }
}
