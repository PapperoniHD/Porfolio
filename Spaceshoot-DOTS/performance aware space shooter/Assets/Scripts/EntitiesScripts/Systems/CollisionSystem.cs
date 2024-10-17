using System.Diagnostics;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;
namespace Systems
{
    [BurstCompile]
    public partial struct CollisionSystem : ISystem
    {

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            
            state.RequireForUpdate<EndVariableRateSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<SimulationSingleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();
            state.Dependency = new CollisionEvent
            {
                CollisionLookup = SystemAPI.GetComponentLookup<CollisionComponent>(),
                DestroyLookup = SystemAPI.GetComponentLookup<DestroyComponent>(),
                ECB = ecb.CreateCommandBuffer(state.WorldUnmanaged)
            }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
            
        }

    }

    [BurstCompile]
    public struct CollisionEvent : ICollisionEventsJob
    {

        [ReadOnly] public ComponentLookup<CollisionComponent> CollisionLookup;
        [ReadOnly] public ComponentLookup<DestroyComponent> DestroyLookup;
        public EntityCommandBuffer ECB;

        [BurstCompile]
        public void Execute(Unity.Physics.CollisionEvent collisionEvent)
        {
            if (!CollisionLookup.HasComponent(collisionEvent.EntityA) || !CollisionLookup.HasComponent(collisionEvent.EntityB)) return;
            
            if (DestroyLookup.HasComponent(collisionEvent.EntityA))
            {
                ECB.DestroyEntity(collisionEvent.EntityA);
                //DestroyLookup.
            }

            if (DestroyLookup.HasComponent(collisionEvent.EntityB))
            {
                ECB.DestroyEntity(collisionEvent.EntityB);
            }
        }
    }
}
