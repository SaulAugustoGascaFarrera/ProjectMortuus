using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

partial struct UnitMoverSystem : ISystem
{


    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        //foreach ((RefRW<LocalTransform> localTransform, RefRW<Unit> unit, RefRW<UnitMover> unitMover) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Unit>, RefRW<UnitMover>>().WithAll<Selected>())
        //{
        //    float3 moveDirection = unitMover.ValueRO.targetPosition - localTransform.ValueRO.Position;

        //    moveDirection = math.normalize(moveDirection);

        //    localTransform.ValueRW.Position += moveDirection * unitMover.ValueRO.movementSpeed * SystemAPI.Time.DeltaTime;


        //    localTransform.ValueRW.Rotation = math.slerp(localTransform.ValueRO.Rotation, quaternion.LookRotation(moveDirection, math.up()), unitMover.ValueRO.rotationSpeed * SystemAPI.Time.DeltaTime);

        //}

        UnitMoveJob unitMoveJob = new UnitMoveJob { 
            deltaTime = SystemAPI.Time.DeltaTime,   
        };

        unitMoveJob.ScheduleParallel();

    }

    
}

[BurstCompile]
public partial struct UnitMoveJob : IJobEntity
{
    public float deltaTime;
    public void Execute(ref LocalTransform localTransform,in UnitMover unitMover,ref PhysicsVelocity physicsVelocity)
    {
        float3 moveDirection = unitMover.targetPosition - localTransform.Position;

        float reachedDistance = 2.0f;

        if(math.lengthsq(moveDirection) > reachedDistance)
        {
            moveDirection = math.normalize(moveDirection);

            //localTransform.Position += moveDirection * unitMover.movementSpeed * deltaTime;

            localTransform.Rotation = math.slerp(localTransform.Rotation,quaternion.LookRotation(moveDirection,math.up()),unitMover.rotationSpeed * deltaTime);


            physicsVelocity.Linear = moveDirection * unitMover.movementSpeed;
            physicsVelocity.Angular = float3.zero;
        }
        else
        {
            physicsVelocity.Linear = float3.zero;
            physicsVelocity.Angular = float3.zero;
            return;
        }

    }
}

