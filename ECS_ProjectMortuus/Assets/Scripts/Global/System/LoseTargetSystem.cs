using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct LoseTargetSystem : ISystem
{
    

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach((RefRO<LocalTransform> localTransform,RefRO<LoseTarget> loseTarget,RefRW<Target> target,RefRO<TargetOveride> targetOverride) in SystemAPI.Query<RefRO<LocalTransform>,RefRO<LoseTarget>,RefRW<Target>, RefRO<TargetOveride>>())
        {
            if(!SystemAPI.Exists(target.ValueRO.targetEntity))
            {
                continue;
            }

            if (SystemAPI.Exists(targetOverride.ValueRO.targetEntity))
            {
                continue;
            }

            LocalTransform targetLocalTransform = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.targetEntity);

            float targetDistance = math.distance(localTransform.ValueRO.Position,targetLocalTransform.Position);

            if(targetDistance > loseTarget.ValueRO.loseTargetDistance)
            {
                //target is too far, reset it
                target.ValueRW.targetEntity = Entity.Null;
            }

        }
    }

   
}
