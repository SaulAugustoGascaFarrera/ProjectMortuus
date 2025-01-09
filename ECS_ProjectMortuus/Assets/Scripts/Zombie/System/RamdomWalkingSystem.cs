using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct RamdomWalkingSystem : ISystem
{
   

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach((RefRW<LocalTransform> localTransform,RefRW<RandomWalking> randomWalking,RefRW<UnitMover> unitMover) in SystemAPI.Query<RefRW<LocalTransform>,RefRW<RandomWalking>,RefRW<UnitMover>>())
        {
            //randomWalking.ValueRW.originPosition = localTransform.ValueRO.Position;
            //randomWalking.ValueRW.targetPosition = localTransform.ValueRO.Position;

            if (math.distancesq(localTransform.ValueRO.Position,randomWalking.ValueRO.targetPosition) < 2)
            {
                //Reached the random target position
                Random random =  randomWalking.ValueRO.random;

                float3 randomDirection = new float3(random.NextFloat(-1,+1),0.0f,random.NextFloat(-1,+1));

                randomDirection = math.normalize(randomDirection);

                


                randomWalking.ValueRW.targetPosition = randomWalking.ValueRO.originPosition + randomDirection * random.NextFloat(randomWalking.ValueRO.distanceMin,randomWalking.ValueRO.distanceMax);

                randomWalking.ValueRW.random = random;
            }
            else
            {
                //too far, move close
                unitMover.ValueRW.targetPosition = randomWalking.ValueRO.targetPosition;
            }
        }
    }

   
}
