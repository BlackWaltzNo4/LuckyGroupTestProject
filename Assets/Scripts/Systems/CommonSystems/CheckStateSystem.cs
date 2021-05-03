using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
 
public class CheckStateSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.WithAll<PlayerComponent, StateComponent>().ForEach((Entity entity, ref PhysicsVelocity velocity, ref StateComponent state) => 
        {
            if (math.length(velocity.Linear) > .1f) state.Value = State.isMoving;
            else state.Value = State.isIdle;
        });
    }
}
