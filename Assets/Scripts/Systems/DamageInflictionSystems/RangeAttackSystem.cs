using System.Numerics;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class RangeAttackSystem : ComponentSystem
{   
    protected override void OnUpdate()
    {
        Entities.WithAll<ArrowPrefabComponent, HealthComponent>().WithNone<UpdateArrowMarker, WaitForCountdown>().ForEach((Entity entity, ref ArrowPrefabComponent arrowPrefab, ref AttackCooldownComponent cooldown, ref StateComponent state, ref Translation translation) =>
        {
            var target = EntityManager.GetBuffer<TargetBuffer>(entity);
            var rotation = EntityManager.GetComponentData<Rotation>(entity);
            var localToWorld = EntityManager.GetComponentData<LocalToWorld>(arrowPrefab.marker);

            if (state.Value == State.isMoving) return;

            var dt = Time.DeltaTime;
            cooldown.ElapsedTime += dt;

            if (target.Length == 0) return;

            var targetTranslation = EntityManager.GetComponentData<Translation>(target[0].Value);

            if (cooldown.ElapsedTime < cooldown.TargetDuration) return;
            else
            {
                if (math.distance(translation.Value, targetTranslation.Value) <= 20f)
                {
                    var forward = math.normalize(math.forward(rotation.Value));

                    cooldown.ElapsedTime = 0;
                    Entity bullet = EntityManager.Instantiate(arrowPrefab.prefab);
                    EntityManager.SetComponentData(bullet, new Translation { Value = localToWorld.Position });
                    EntityManager.SetComponentData(bullet, new Rotation { Value = UnityEngine.Quaternion.LookRotation(targetTranslation.Value - translation.Value) });
                    EntityManager.AddComponentData(bullet, new ArrowComponent { speed = math.normalize(targetTranslation.Value - translation.Value) * arrowPrefab.speed });

                    if (EntityManager.HasComponent<PlayerComponent>(entity))
                    {
                        EntityManager.AddComponentData(bullet, new PlayerArrow { });
                    }

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
