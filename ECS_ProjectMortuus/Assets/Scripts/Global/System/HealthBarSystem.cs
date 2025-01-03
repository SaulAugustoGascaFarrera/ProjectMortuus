using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
partial struct HealthBarSystem : ISystem
{
    

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        Vector3 cameraForward = Vector3.zero;

        if(cameraForward != null)
        {
            cameraForward = Camera.main.transform.forward;
        }


        foreach ((RefRO<HealthBar> healthBar,RefRW<LocalTransform> localTransform) in SystemAPI.Query<RefRO<HealthBar>,RefRW<LocalTransform>>())
        {

            LocalTransform parentLocalTransform = SystemAPI.GetComponent<LocalTransform>(healthBar.ValueRO.healthEntity);

            if(localTransform.ValueRO.Scale == 1.0f)
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

            if(normalizedHealth == 1.0f)
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
