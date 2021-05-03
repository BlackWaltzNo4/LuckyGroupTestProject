using Unity.Entities;

public class CountdownSystem : ComponentSystem
{
    private float elapsedTime = 0f;
    private float targetDuration = 3f;

    protected override void OnUpdate()
    {
        if (elapsedTime >= targetDuration) return;

        elapsedTime += Time.DeltaTime;
        Manager.SetCountdownText($"{(int)targetDuration - (int)elapsedTime}");

        if (elapsedTime >= targetDuration)        
        {
            Manager.SetCountdownText("");
            Manager.SetStatusText("FIGHT!");

            Entities.WithAll<WaitForCountdown>().ForEach((Entity entity) =>
            {
                PostUpdateCommands.RemoveComponent<WaitForCountdown>(entity);
            });
        }
    }
}
