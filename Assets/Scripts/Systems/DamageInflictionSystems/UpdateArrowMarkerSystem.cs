using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

public class UpdateArrowMarkerSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.WithAll<UpdateArrowMarker>().ForEach((Entity entity, ref ArrowPrefabComponent arrowPrefab, DynamicBuffer<Child> children) =>
        {

            foreach (var child in children)
            {
                if (EntityManager.HasComponent<ArrowMarker>(child.Value))
                {
                    arrowPrefab.marker = child.Value;
                }
            }

            EntityManager.RemoveComponent<UpdateArrowMarker>(entity);
        });
    }
}
