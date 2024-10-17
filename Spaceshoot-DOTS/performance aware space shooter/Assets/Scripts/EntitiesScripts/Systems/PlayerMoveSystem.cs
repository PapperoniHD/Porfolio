using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[UpdateBefore(typeof(TransformSystemGroup))]
public partial struct PlayerMoveSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var deltatime = SystemAPI.Time.DeltaTime;
        new PlayerMoveJob
        {
            DeltaTime = deltatime
        }.Schedule();
    }   
}

[BurstCompile]
public partial struct PlayerMoveJob : IJobEntity
{
    public float DeltaTime;

    [BurstCompile]
    private void Execute(ref LocalTransform transform, in InputComponent moveInput, PlayerMoveSpeed moveSpeed)
    {
        transform.Position.x += moveInput.Value.x * moveSpeed.Value * DeltaTime;
        
  
        if (math.lengthsq(moveInput.Value) > float.Epsilon)
        {
            //var forward = new float2(moveInput.Value.x, moveInput.Value.y);
            //var forward = moveInput.Value.x;
            //transform.RotateX(moveInput.Value.y * moveSpeed.Value * DeltaTime);
        }
    }
}
