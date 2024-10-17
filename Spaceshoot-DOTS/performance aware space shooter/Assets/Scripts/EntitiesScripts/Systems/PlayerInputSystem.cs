using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;

[UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
public partial class PlayerInputSystem : SystemBase
{
    private MovementAction _movementActions;
    private Entity _playerEntity;

    protected override void OnCreate()
    {
        RequireForUpdate<PlayerTag>();
        RequireForUpdate<InputComponent>();

        _movementActions = new MovementAction();
    }

    protected override void OnStartRunning()
    {
        _movementActions.Enable();
        _movementActions.ActionMap.Shoot.performed += OnPlayerShoot;

        _playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
    }

    protected override void OnUpdate()
    {
        var curMoveInput = _movementActions.ActionMap.Movement.ReadValue<Vector2>();

        SystemAPI.SetSingleton(new InputComponent { Value = curMoveInput });
    }

    protected override void OnStopRunning()
    {
        _movementActions.Disable();
        _playerEntity = Entity.Null;
        _movementActions.ActionMap.Shoot.performed -= OnPlayerShoot;

        _playerEntity = Entity.Null;
    }


    private void OnPlayerShoot(InputAction.CallbackContext obj)
    {
        if (!SystemAPI.Exists(_playerEntity)) return;
        SystemAPI.SetComponentEnabled<FireProjectileTag>(_playerEntity, true);
    }

}
