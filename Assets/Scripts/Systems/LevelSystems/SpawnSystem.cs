using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class SpawnSystem : ComponentSystem
{
    private Unity.Mathematics.Random rnd = new Unity.Mathematics.Random(19111993);

    protected override void OnUpdate()
    {
        Entities.WithAll<SpawnEnemiesComponent>().ForEach((Entity entity, ref SpawnEnemiesComponent enemy, ref Translation translation) =>
        {
            Entity player = Entity.Null;

            Entities.WithAll<PlayerComponent>().ForEach((Entity entity) =>
            {
                player = entity;
            });

            if (enemy.RangeEnemyPrefab != null)
            {
                for (int i = 0; i < enemy.RangeEnemyAmount; i++)
                {
                    Entity enemyEntity = EntityManager.Instantiate(enemy.RangeEnemyPrefab);
                    EntityManager.SetComponentData(enemyEntity, new Translation { Value = translation.Value +  new float3(rnd.NextFloat(-10f, 10f), 1f, rnd.NextFloat(-10f, 10f)) });
                    var buffer = EntityManager.AddBuffer<TargetBuffer>(enemyEntity);
                    buffer.Add(new TargetBuffer { Value = player });
                }
            }

            if (enemy.MeleeEnemyPrefab != null)
            {
                for (int i = 0; i < enemy.MeleeEnemyAmount; i++)
                {
                    Entity enemyEntity = EntityManager.Instantiate(enemy.MeleeEnemyPrefab);
                    EntityManager.SetComponentData(enemyEntity, new Translation { Value = translation.Value + new float3(rnd.NextFloat(-10f, 10f), 1f, rnd.NextFloat(-10f, 10f)) });
                    var buffer = EntityManager.AddBuffer<TargetBuffer>(enemyEntity);
                    buffer.Add(new TargetBuffer { Value = player });
                }
            }

            if (enemy.FlyingEnemyPrefab != null)
            {
                for (int i = 0; i < enemy.FlyingEnemyAmount; i++)
                {
                    Entity enemyEntity = EntityManager.Instantiate(enemy.FlyingEnemyPrefab);
                    EntityManager.SetComponentData(enemyEntity, new Translation { Value = translation.Value + new float3(rnd.NextFloat(-10f, 10f), 11f, rnd.NextFloat(-10f, 10f)) });
                    var buffer = EntityManager.AddBuffer<TargetBuffer>(enemyEntity);
                    buffer.Add(new TargetBuffer { Value = player });
                }
            }

            EntityManager.RemoveComponent<SpawnEnemiesComponent>(entity);
        });
    }
}
