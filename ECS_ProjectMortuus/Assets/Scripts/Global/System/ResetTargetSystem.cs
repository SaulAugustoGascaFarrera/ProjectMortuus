using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;

[UpdateInGroup(typeof(SimulationSystemGroup),OrderFirst =true)]
partial struct ResetTargetSystem : ISystem
{

    private ComponentLookup<LocalTransform> localTransformComponentLookup;
    private EntityStorageInfoLookup entityStorageInfoLookup;


    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        localTransformComponentLookup = state.GetComponentLookup<LocalTransform>(true);
        entityStorageInfoLookup = state.GetEntityStorageInfoLookup();
    }



    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        //foreach ((RefRO<LocalTransform> localTransform, RefRW<Target> target) in SystemAPI.Query<RefRO<LocalTransform>, RefRW<Target>>())
        //{
        //    if (SystemAPI.Exists(target.ValueRO.targetEntity))
        //    {
        //        if (!SystemAPI.Exists(target.ValueRO.targetEntity) || !SystemAPI.HasComponent<LocalTransform>(target.ValueRO.targetEntity))
        //        {
        //            target.ValueRW.targetEntity = Entity.Null;
        //        }
        //    }

        //}

        //foreach (RefRW<TargetOveride> targetOverride in SystemAPI.Query<RefRW<TargetOveride>>())
        //{
        //    if (SystemAPI.Exists(targetOverride.ValueRO.targetEntity))
        //    {
        //        if (!SystemAPI.Exists(targetOverride.ValueRO.targetEntity) || !SystemAPI.HasComponent<LocalTransform>(targetOverride.ValueRO.targetEntity))
        //        {
        //            targetOverride.ValueRW.targetEntity = Entity.Null;
        //        }
        //    }

        //}

        localTransformComponentLookup.Update(ref state);

        entityStorageInfoLookup.Update(ref state);

        ResetTargetJob resetTargetJob = new ResetTargetJob
        {
            localTransformCoponentLookup = localTransformComponentLookup,
            entityStorageInfoLookup = entityStorageInfoLookup,
        };

        TargetOverrideJob targetOverrideJob = new TargetOverrideJob
        {
            localTransformComponentLookUp = localTransformComponentLookup,
            entityStorageInfoLookup = entityStorageInfoLookup,
        };


        resetTargetJob.ScheduleParallel();

        targetOverrideJob.ScheduleParallel();

    }

    
}


[BurstCompile]
public partial struct ResetTargetJob : IJobEntity
{

    [ReadOnly] public ComponentLookup<LocalTransform> localTransformCoponentLookup;
    [ReadOnly] public EntityStorageInfoLookup entityStorageInfoLookup;

    public void Execute(ref Target target)
    {
        if (entityStorageInfoLookup.Exists(target.targetEntity))
        {
            if (!entityStorageInfoLookup.Exists(target.targetEntity) || !localTransformCoponentLookup.HasComponent(target.targetEntity))
            {
                target.targetEntity = Entity.Null;
            }
        }
    }
}


[BurstCompile]
public partial struct TargetOverrideJob : IJobEntity
{

    [ReadOnly] public ComponentLookup<LocalTransform> localTransformComponentLookUp;
    [ReadOnly] public EntityStorageInfoLookup entityStorageInfoLookup;
    public void Execute(ref TargetOveride targetOverride)
    {
        if(entityStorageInfoLookup.Exists(targetOverride.targetEntity))
        {
            if(!entityStorageInfoLookup.Exists(targetOverride.targetEntity) || !localTransformComponentLookUp.HasComponent(targetOverride.targetEntity))
            {

                targetOverride.targetEntity = Entity.Null;  
            }
        }
    }
}
