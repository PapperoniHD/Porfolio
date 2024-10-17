using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour
{
    public float MoveSpeed;
    public GameObject ProjectilePrefab;
}

public class PlayerAuthoringBaker : Baker<PlayerAuthoring>
{
    public override void Bake(PlayerAuthoring authoring)
    {
        var playerEntity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent<PlayerTag>(playerEntity);
        AddComponent<InputComponent>(playerEntity);
        AddComponent<CollisionTag>(playerEntity);    

        AddComponent<FireProjectileTag>(playerEntity);
        SetComponentEnabled<FireProjectileTag>(playerEntity, false);

        AddComponent(playerEntity, new PlayerMoveSpeed
        {
            Value = authoring.MoveSpeed
        });
        AddComponent(playerEntity, new ProjectilePrefab
        {
            Value = GetEntity(authoring.ProjectilePrefab, TransformUsageFlags.Dynamic)
        });
    }
}
