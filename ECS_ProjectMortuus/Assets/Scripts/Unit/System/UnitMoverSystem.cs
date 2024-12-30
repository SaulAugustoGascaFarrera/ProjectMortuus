using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct UnitMoverSystem : ISystem
{
    

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach ((RefRW<LocalTransform> localTransform, RefRW<Unit> unit, RefRW<UnitMover> unitMover) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Unit>, RefRW<UnitMover>>().WithAll<Selected>())
        {
            float3 moveDirection = new float3(3, 0, 0);

            moveDirection = math.normalize(moveDirection);

            localTransform.ValueRW.Position += moveDirection * unitMover.ValueRO.movementSpeed * SystemAPI.Time.DeltaTime;


            localTransform.ValueRW.Rotation = math.slerp(localTransform.ValueRO.Rotation, quaternion.LookRotation(moveDirection, math.up()), unitMover.ValueRO.rotationSpeed * SystemAPI.Time.DeltaTime);

        }
    }

    
}

[BurstCompile]
public partial struct MoveJob : IJobEntity
{
    public void Execute()
    {

    }
}

