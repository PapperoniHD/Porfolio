using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ProjectileMoveSpeedAuthoring : MonoBehaviour
{
    public float ProjectileMoveSpeed;
    public Entity entity;
    public class ProjectileMoveSpeedBaker : Baker<ProjectileMoveSpeedAuthoring>
    {
        public override void Bake(ProjectileMoveSpeedAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new ProjectileMoveSpeed { Value = authoring.ProjectileMoveSpeed });
            AddComponent<CollisionTag>(entity);
            AddComponent<CollisionComponent>(entity);
            AddComponent<DestroyComponent>(entity);
        }
    }
}

public partial struct Bullet : ISystem
{

    public void OnStartRunning(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        //new BulletDeleteJob { }.ScheduleParallel();
    }
}

[BurstCompile]
public partial struct BulletDeleteJob : IJobEntity
{
    [WriteOnly] public EntityCommandBuffer Cmd;
    [BurstCompile]
    private void Execute(ref LocalTransform transform) 
    {
        if (transform.Position.y > 10)
        {
            //Cmd.DestroyEntity(entity);
            
        }
    }
}
