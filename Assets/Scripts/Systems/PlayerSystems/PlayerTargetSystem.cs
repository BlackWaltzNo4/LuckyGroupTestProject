using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class PlayerTargetSystem : ComponentSystem
{
    private EndFixedStepSimulationEntityCommandBufferSystem _commandBufferSystem;

    protected override void OnCreate()
    {
        _commandBufferSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<EndFixedStepSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        var commandBuffer = _commandBufferSystem.PostUpdateCommands;

        Entities.WithAll<PlayerComponent, TargetBuffer>().ForEach((Entity entity, ref Translation translation, DynamicBuffer<TargetBuffer> neighborBuffer) =>
        {
            float3 position = translation.Value;

            neighborBuffer.Clear();

            NativeList<Entity> neighborsEntityList = new NativeList<Entity>(Allocator.Temp);
            Entity closestEnemy = Entity.Null;
            float3 closestEnemyPosition = float3.zero;

            float range = 20f;

            Entities.WithAll<HealthComponent>().WithAny<EnemyMeleeType, EnemyRangeType, EnemyFlyingType>().ForEach((Entity tile, ref Translation tileTranslation) =>
            {
                float tileDistance = math.abs(math.distance(position, tileTranslation.Value));
                if (tileDistance > range) return;

                if (closestEnemy == Entity.Null)
                {
                    closestEnemy = tile;
                    closestEnemyPosition = tileTranslation.Value;
                }
                else
                {
                    if (math.distance(position, tileTranslation.Value) < math.distance(position, closestEnemyPosition))
                    {
                        //range = tileDistance;
                        closestEnemy = tile;
                        closestEnemyPosition = tileTranslation.Value;
                    }
                }
            });

            if (closestEnemy != Entity.Null) neighborsEntityList.Add(closestEnemy);

            foreach (Entity neighbor in neighborsEntityList)
            {
                if (neighborBuffer.Length < 1)
                {
                    neighborBuffer.Add(new TargetBuffer { Value = neighbor });
                }
            }
        });
    }
}
