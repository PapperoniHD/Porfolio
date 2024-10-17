using Unity.Entities;
using Unity.Mathematics;


public struct EnemySpawnProperties : IComponentData
{
    public float2 FieldDimension;
    public int NumberOfSpawns;
    public Entity EnemyPrefab;
}
