using Unity.Entities;


public class EnemyCountSystem : ComponentSystem
{
    private int count = 0;

    protected override void OnUpdate()
    {
        count = 0;

        Entities.WithAny<EnemyTag, PlayerComponent>().WithAll<HealthComponent>().ForEach((Entity entity) =>
        {
            count += 1;
        });

        if (count == 1)
        {
            Manager.SetStatusText("THE GATES ARE OPEN");

            Entities.WithAll<Gate>().ForEach((Entity entity) =>
            {
                PostUpdateCommands.DestroyEntity(entity);
            });
        }
    }
}
