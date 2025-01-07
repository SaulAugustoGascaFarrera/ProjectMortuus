using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct MoveOverrideSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach ((RefRW<UnitMover> unitMover, RefRO<MoveOverride> moveOverride, EnabledRefRW<MoveOverride> enabledMoveOverride, RefRO<LocalTransform> localTransform) in SystemAPI.Query<RefRW<UnitMover>, RefRO<MoveOverride>, EnabledRefRW<MoveOverride>, RefRO<LocalTransform>>())
        {


            if (math.distancesq(localTransform.ValueRO.Position, moveOverride.ValueRO.targetPosition) > 2.0f)
            {
                unitMover.ValueRW.targetPosition = moveOverride.ValueRO.targetPosition;


                

            }
            else
            {
                enabledMoveOverride.ValueRW = false;

                
            }
        }
    }

}
