using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
partial struct HealthBarSystem : ISystem
{

    public ComponentLookup<LocalTransform> localTransformComponentLookUp;
    public ComponentLookup<Health> healthComponentLookUp;
    public ComponentLookup<PostTransformMatrix> postTransformMatrixLookUp;
    public void OnCreate(ref SystemState state)
    {
        //localTransformComponentLookUp = state.GetComponentLookup<LocalTransform>(); 
        //healthComponentLookUp = state.GetComponentLookup<Health>(true); 
        //postTransformMatrixLookUp = state.GetComponentLookup<PostTransformMatrix>(false);
    }


    //[BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        Vector3 cameraForward = Vector3.zero;

        if(cameraForward != null)
        {
            cameraForward = Camera.main.transform.forward;
        }

        //localTransformComponentLookUp.Update(ref state);
        //healthComponentLookUp.Update(ref state);
        //postTransformMatrixLookUp.Update(ref state);

        //HealthBarJob healthBarJob = new HealthBarJob
        //{
        //    cameraForward = cameraForward,
        //    localTransformComponentLookUp = localTransformComponentLookUp,
        //    healthComponentLookUp = healthComponentLookUp,  
        //    postTransformMatrixLookUp = postTransformMatrixLookUp
        //};

        //healthBarJob.ScheduleParallel();


        foreach ((RefRO<HealthBar> healthBar, RefRW<LocalTransform> localTransform) in SystemAPI.Query<RefRO<HealthBar>, RefRW<LocalTransform>>())
        {

            LocalTransform parentLocalTransform = SystemAPI.GetComponent<LocalTransform>(healthBar.ValueRO.healthEntity);

            if (localTransform.ValueRO.Scale == 1.0f)
            {
                //health bar is visible
                localTransform.ValueRW.Rotation = parentLocalTransform.InverseTransformRotation(quaternion.LookRotation(cameraForward, math.up()));
            }


            Health health = SystemAPI.GetComponent<Health>(healthBar.ValueRO.healthEntity);

            if (!health.onHealthChange)
            {
                //health is not changed yet 
                continue;
            }

            float normalizedHealth = (float)health.healthAmount / health.healthMax;

            //UnityEngine.Debug.Log(normalizedHealth);

            if (normalizedHealth == 1.0f)
            {
                localTransform.ValueRW.Scale = 0.0f;
            }
            else
            {
                localTransform.ValueRW.Scale = 1.0f;
            }

            RefRW<PostTransformMatrix> barVisualPostTransform = SystemAPI.GetComponentRW<PostTransformMatrix>(healthBar.ValueRO.barVisualEntity);
            barVisualPostTransform.ValueRW.Value = float4x4.Scale(normalizedHealth, 1, 1);


        }
    }

   
}


[BurstCompile]
public partial struct HealthBarJob : IJobEntity
{
    [NativeDisableParallelForRestriction] public ComponentLookup<LocalTransform> localTransformComponentLookUp;
    [ReadOnly] public ComponentLookup<Health> healthComponentLookUp;
    [NativeDisableParallelForRestriction] public ComponentLookup<PostTransformMatrix> postTransformMatrixLookUp;


    public float3 cameraForward;

    public void Execute(in HealthBar healthBar,Entity entity)
    {
        RefRW<LocalTransform> localTransform = localTransformComponentLookUp.GetRefRW(entity);


        LocalTransform parentLocalTransform = localTransformComponentLookUp[healthBar.healthEntity];

        if (localTransform.ValueRO.Scale == 1.0f)
        {
            //health bar is visible
            localTransform.ValueRW.Rotation = parentLocalTransform.InverseTransformRotation(quaternion.LookRotation(cameraForward, math.up()));
        }


        Health health = healthComponentLookUp[healthBar.healthEntity];

        if (!health.onHealthChange)
        {
            //health is not changed yet 
            return;
        }

        float normalizedHealth = (float)health.healthAmount / health.healthMax;

        //UnityEngine.Debug.Log(normalizedHealth);

        if (normalizedHealth == 1.0f)
        {
            localTransform.ValueRW.Scale = 0.0f;
        }
        else
        {
            localTransform.ValueRW.Scale = 1.0f;
        }

        RefRW<PostTransformMatrix> barVisualPostTransform = postTransformMatrixLookUp.GetRefRW(healthBar.barVisualEntity);
        barVisualPostTransform.ValueRW.Value = float4x4.Scale(normalizedHealth, 1, 1);
    }
}
