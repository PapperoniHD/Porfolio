using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
public readonly partial struct SpawnAspect : IAspect
{
    public readonly Entity Entity;

    //private readonly LocalTransform localTransform;

    private readonly RefRO<EnemySpawnProperties> _enemyspawnProperties;
    private readonly RefRW<SpawnRandom> _spawnRandom;

    public int NumberEnemiesToSpawn => _enemyspawnProperties.ValueRO.NumberOfSpawns;
    public Entity EnemyPrefab => _enemyspawnProperties.ValueRO.EnemyPrefab;

    public LocalTransform GetRandomSpawnTransform()
    {
        return new LocalTransform
        {
           
            Position = GetRandomPos(),
            Rotation = quaternion.identity,
            Scale = 1,
        };
    }

    private float3 GetRandomPos()
    {
        float3 randomPosition;

        randomPosition = new float3(_spawnRandom.ValueRW.Value.NextFloat(-7, 7), _spawnRandom.ValueRW.Value.NextFloat(10, 20),0);

        //randomPosition = new float3(0, 1, 0);
        return randomPosition;
    }

}

