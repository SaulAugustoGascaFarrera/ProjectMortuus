using Unity.Burst;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

partial struct ActiveAnimationSystem : ISystem
{

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<AnimationDataHolder>();
    }


    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {

        AnimationDataHolder animationDataHolder = SystemAPI.GetSingleton<AnimationDataHolder>();

        foreach((RefRW<ActiveAnimation> activeAnimation,RefRW<MaterialMeshInfo> meshMaterialInfo) in SystemAPI.Query<RefRW<ActiveAnimation>,RefRW<MaterialMeshInfo>>())
        {

            //if(!activeAnimation.ValueRO.animationDataBlobAssetReference.IsCreated)
            //{
            //    activeAnimation.ValueRW.animationDataBlobAssetReference = animationDataHolder.soldierIdle;
            //}

            if (Input.GetKeyDown(KeyCode.T))
            {
                //activeAnimation.ValueRW.activeAnimationType = AnimationDataSO.AnimationType.SoldierIdle;
                activeAnimation.ValueRW.nextAnimationType = AnimationDataSO.AnimationType.SoldierIdle;
            }

            if (Input.GetKeyDown(KeyCode.Y))
            {
                //activeAnimation.ValueRW.activeAnimationType = AnimationDataSO.AnimationType.SoldierWalk;
                activeAnimation.ValueRW.nextAnimationType = AnimationDataSO.AnimationType.SoldierWalk;
            }

            ref AnimationData animationData = ref animationDataHolder.animationDataBlobArrayBlobAssetReference.Value[(int)activeAnimation.ValueRO.activeAnimationType];

            activeAnimation.ValueRW.frameTimer += SystemAPI.Time.DeltaTime;

            if(activeAnimation.ValueRW.frameTimer > animationData.frameTimerMax)
            {
                activeAnimation.ValueRW.frameTimer -= animationData.frameTimerMax;

                activeAnimation.ValueRW.frame = (activeAnimation.ValueRO.frame + 1) % animationData.frameMax;


                meshMaterialInfo.ValueRW.MeshID =  animationData.batchMeshIdBlobArray[activeAnimation.ValueRO.frame];
            }
        }
    }

    
}
