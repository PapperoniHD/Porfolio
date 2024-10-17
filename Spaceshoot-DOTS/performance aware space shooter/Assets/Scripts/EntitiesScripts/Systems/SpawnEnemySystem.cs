using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct SpawnEnemySystem : ISystem
{
    float time;
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EnemySpawnProperties>();
    }
    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        
        time++;
        //Debug.Log(time.ToString());
        if (time > 5000)
        {
            time = 0;
            var enemyEntity = SystemAPI.GetSingletonEntity<EnemySpawnProperties>();
            var enemyAspect = SystemAPI.GetAspect<SpawnAspect>(enemyEntity);

            var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

            for (var i = 0; i < enemyAspect.NumberEnemiesToSpawn; i++)
            {
                var newEnemy = ecb.Instantiate(enemyAspect.EnemyPrefab);
                var newEnemyTransform = enemyAspect.GetRandomSpawnTransform();
                ecb.SetComponent(newEnemy, newEnemyTransform);
            }

            ecb.Playback(state.EntityManager);
        }
        //state.Enabled = false;
        
    }
}
