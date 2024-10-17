using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public readonly partial struct EnemyMoveAspect : IAspect
{
    public readonly Entity entity;

    private readonly RefRW<LocalTransform> _transform;

    public void Move(float deltaTime)
    {
        Debug.Log("Moving");
    }
}
