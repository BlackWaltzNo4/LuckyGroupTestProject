using Unity.Entities;
using UnityEngine;
using UnityEngine.AI;

public class ObjectsAuthoring : MonoBehaviour
{
    public Controller controller;
    public NavMeshAgent agent;
    public Transform meleeEnemyRig;
    public Transform rangeEnemyRig;
    public Transform flyingEnemyRig;

    void Awake()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        world.GetOrCreateSystem<PlayerMovementSystem>().SetupController(controller);
        world.GetOrCreateSystem<UpdateNavMeshAgentSystem>().SetNavMeshAgent(agent);
        world.GetOrCreateSystem<UpdateRigSystem>().SetTransform(meleeEnemyRig, rangeEnemyRig, flyingEnemyRig);
    }
}
