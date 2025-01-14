using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[UpdateInGroup(typeof(SimulationSystemGroup),OrderFirst =true)]
partial struct ResetTargetSystem : ISystem
{
   

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach((RefRO<LocalTransform> localTransform,RefRW<Target> target) in SystemAPI.Query<RefRO<LocalTransform>,RefRW<Target>>())
        {
            if(SystemAPI.Exists(target.ValueRO.targetEntity))
            {
                if (!SystemAPI.Exists(target.ValueRO.targetEntity) || !SystemAPI.HasComponent<LocalTransform>(target.ValueRO.targetEntity))
                {
                    target.ValueRW.targetEntity = Entity.Null;
                }
            }
           
        }

        foreach (RefRW<TargetOveride> targetOverride in SystemAPI.Query<RefRW<TargetOveride>>())
        {
            if (SystemAPI.Exists(targetOverride.ValueRO.targetEntity))
            {
                if (!SystemAPI.Exists(targetOverride.ValueRO.targetEntity) || !SystemAPI.HasComponent<LocalTransform>(targetOverride.ValueRO.targetEntity))
                {
                    targetOverride.ValueRW.targetEntity = Entity.Null;
                }
            }

        }
    }

    
}
