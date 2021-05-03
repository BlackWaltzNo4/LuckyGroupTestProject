using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class FlyingEnemyMovementSystem : ComponentSystem
{
    private Unity.Mathematics.Random rnd = new Unity.Mathematics.Random(19111993);

    protected override void OnUpdate()
    {
        var dt = Time.DeltaTime;

        Entities.WithAll<FlyingEnemyLogicComponent, HealthComponent>().WithNone<WaitForCountdown>().ForEach((Entity entity, ref Translation translation, ref Rotation rotation, ref FlyingEnemyLogicComponent logic, ref PhysicsVelocity velocity) =>
        {
            if (math.distance(translation.Value, logic.destination.Value) < 2f)
            {
                if (logic.isTargetingPlayer)
                {
                    logic.destination = new Translation { Value = new float3(rnd.NextFloat(-20f, 20f), rnd.NextFloat(10f, 20f), rnd.NextFloat(-20f, 20f)) };
                    logic.isTargetingPlayer = false;
                    logic.rotationSpeed = 3f;
                }
                else
                {
                    var targetBuffer = EntityManager.GetBuffer<TargetBuffer>(entity);
                    var targetPosition = EntityManager.GetComponentData<Translation>(targetBuffer[0].Value);
                    logic.destination = new Translation { Value = targetPosition.Value + new float3(0f, .5f, 0f) };

                    logic.isTargetingPlayer = true;
                    logic.rotationSpeed = 1f;
                }
            }

            if (logic.isTargetingPlayer)
            {
                if (math.distance(translation.Value, logic.destination.Value) > 10f)
                {
                    var targetBuffer = EntityManager.GetBuffer<TargetBuffer>(entity);
                    var targetPosition = EntityManager.GetComponentData<Translation>(targetBuffer[0].Value);
                    logic.destination = new Translation { Value = targetPosition.Value + new float3(0f, .5f, 0f) };
                }
            }

            logic.rotationSpeed += .01f;

            var dest = Quaternion.LookRotation(logic.destination.Value - translation.Value);
            var smooth = Quaternion.Slerp(rotation.Value, dest, dt * logic.rotationSpeed);
            //var _rotation = quaternion.LookRotation(logic.destination.Value, math.up());
            rotation = new Rotation { Value = smooth };

            velocity.Linear = math.normalize(math.forward(smooth)) * 10f;
        });
    }
}
