using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

public class PlayerMovementSystem : ComponentSystem
{
    private Controller controller;

    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        float2 input = new float2(controller.GetVector.x, controller.GetVector.y);// = new float2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Entities.WithAll<PlayerComponent, HealthComponent>().WithNone<WaitForCountdown>().ForEach((ref MoveComponent move, ref Translation translation, ref LocalToWorld transform, ref PhysicsVelocity velocity, ref Rotation rotation) =>
        {
            float3 direction = new float3(input.x * move.movementSpeed * deltaTime, 0f, input.y * move.movementSpeed * deltaTime);

            velocity.Linear += new float3(direction.x, 0.0f, direction.z);

            if (direction.x != 0) rotation.Value = quaternion.LookRotation(direction, math.up());

            Manager.SetPlayerPosition(translation.Value);
        });
    }

    public void SetupController(Controller controller)
    {
        this.controller = controller;
    }
}
