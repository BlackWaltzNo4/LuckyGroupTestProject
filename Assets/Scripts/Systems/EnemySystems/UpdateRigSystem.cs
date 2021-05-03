using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine.AI;

public class UpdateRigSystem : ComponentSystem
{
    private Transform meleeRig;
    private Transform rangeRig;
    private Transform flyingRig;

    protected override void OnUpdate()
    {
        Entities.WithAll<UpdateMeleeRig>().ForEach((Entity entity, ref LocalToWorld localToWorld) =>
        {
            var parent = EntityManager.GetComponentData<Parent>(entity);
            var translation = EntityManager.GetComponentData<Translation>(parent.Value);
            var rotation = EntityManager.GetComponentData<Rotation>(parent.Value);

            var currenTransform = GameObject.Instantiate(meleeRig);
            currenTransform.transform.position = translation.Value;
            currenTransform.transform.rotation = rotation.Value;
            EntityManager.AddComponentObject(entity, currenTransform);

            var _animator = currenTransform.GetComponent<Animator>();
            EntityManager.AddComponentObject(entity, _animator);

            EntityManager.RemoveComponent<UpdateMeleeRig>(entity);
        });

        Entities.WithAll<UpdateRangeRig>().ForEach((Entity entity, ref LocalToWorld localToWorld) =>
        {
            var parent = EntityManager.GetComponentData<Parent>(entity);
            var translation = EntityManager.GetComponentData<Translation>(parent.Value);
            var rotation = EntityManager.GetComponentData<Rotation>(parent.Value);

            var currenTransform = GameObject.Instantiate(rangeRig);
            currenTransform.transform.position = translation.Value;
            currenTransform.transform.rotation = rotation.Value;
            EntityManager.AddComponentObject(entity, currenTransform);

            var _animator = currenTransform.GetComponent<Animator>();
            EntityManager.AddComponentObject(entity, _animator);

            EntityManager.RemoveComponent<UpdateRangeRig>(entity);
        });

        Entities.WithAll<UpdateFlyingRig>().ForEach((Entity entity, ref LocalToWorld localToWorld) =>
        {
            var parent = EntityManager.GetComponentData<Parent>(entity);
            var translation = EntityManager.GetComponentData<Translation>(parent.Value);
            var rotation = EntityManager.GetComponentData<Rotation>(parent.Value);

            var currenTransform = GameObject.Instantiate(flyingRig);
            currenTransform.transform.position = translation.Value;
            currenTransform.transform.rotation = rotation.Value;
            EntityManager.AddComponentObject(entity, currenTransform);

            var _animator = currenTransform.GetComponent<Animator>();
            EntityManager.AddComponentObject(entity, _animator);

            EntityManager.RemoveComponent<UpdateFlyingRig>(entity);
        });
    }

    public void SetTransform(Transform meleeRig, Transform rangeRig, Transform flyingRig)
    {
        this.meleeRig = meleeRig;
        this.rangeRig = rangeRig;
        this.flyingRig = flyingRig;
    }
}
