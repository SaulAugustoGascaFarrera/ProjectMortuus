using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

partial struct SetupUnitMoverDefaultPositionSystem : ISystem
{
    

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer commandBuffer = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach((RefRW<UnitMover> unitMover,RefRO<SetupUnitMoverDefaultPosition> setupUnitMoverDefaultPosition,RefRW<LocalTransform> localTranform,Entity entity) in SystemAPI.Query<RefRW<UnitMover>,RefRO<SetupUnitMoverDefaultPosition>,RefRW<LocalTransform>>().WithEntityAccess())
        {
            unitMover.ValueRW.targetPosition = localTranform.ValueRO.Position;

            commandBuffer.RemoveComponent<SetupUnitMoverDefaultPosition>(entity);
        }
    }

    
}


