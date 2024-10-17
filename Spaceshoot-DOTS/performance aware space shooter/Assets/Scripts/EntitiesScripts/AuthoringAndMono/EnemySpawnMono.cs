using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class EnemySpawnMono : MonoBehaviour
{
    public float2 FieldDimensions;
    public int NumberOfSpawns;
    public GameObject EnemyPrefab;
    public uint RandomSeed;
    public float Time;
}

public class EnemyBaker : Baker<EnemySpawnMono>
{
    public override void Bake(EnemySpawnMono authoring)
    {
        //TransformUsageFlags flags = new TransformUsageFlags();
        Entity entity = GetEntity(TransformUsageFlags.None);

        AddComponent(entity, new EnemySpawnProperties
        {
            FieldDimension = authoring.FieldDimensions,
            NumberOfSpawns = authoring.NumberOfSpawns,
            EnemyPrefab = GetEntity(authoring.EnemyPrefab, TransformUsageFlags.Dynamic),
        });
        AddComponent(entity, new SpawnRandom
        {
            Value = Unity.Mathematics.Random.CreateFromIndex(authoring.RandomSeed)
        });
    }
}
