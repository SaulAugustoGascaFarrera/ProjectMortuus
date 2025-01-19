using Unity.Burst;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

partial struct ActiveAnimationTestSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<AnimationDataHolderTest>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        AnimationDataHolderTest animationDataHolderTest = SystemAPI.GetSingleton<AnimationDataHolderTest>();

        foreach ((RefRW<ActiveAnimationTest> activeAnimationTest, RefRW<MaterialMeshInfo> meshMaterialInfo) in SystemAPI.Query<RefRW<ActiveAnimationTest>, RefRW<MaterialMeshInfo>>())
        {
            //if (!activeAnimationTest.ValueRO.animationDataTestBlobAssetReference.IsCreated)
            //{
            //    //activeAnimationTest.ValueRW.animationDataTestBlobAssetReference = animationDataHolderTest.soldierIdle;
            //    activeAnimationTest.ValueRW.activeAnimationIndex = 0;
            //}


            if (Input.GetKeyDown(KeyCode.T))
            {
                activeAnimationTest.ValueRW.activeAnimationIndex = 0;
            }

            //if (Input.GetKeyDown(KeyCode.Y))
            //{
            //    activeAnimationTest.ValueRW.activeAnimationIndex = 1;
            //}


            ref AnimationDataTest animationDataTest = ref animationDataHolderTest.animationDataTestBlobAssetArray.Value[activeAnimationTest.ValueRO.activeAnimationIndex];

            activeAnimationTest.ValueRW.frameTimer += SystemAPI.Time.DeltaTime;

            if(activeAnimationTest.ValueRO.frameTimer > animationDataTest.frameTimerMax)
            {
                activeAnimationTest.ValueRW.frameTimer -= animationDataTest.frameTimerMax;

                activeAnimationTest.ValueRW.frame = (activeAnimationTest.ValueRO.frame + 1) % animationDataTest.frameMax;

                //switch(activeAnimationTest.ValueRO.frame)
                //{
                //    default:
                //    case 0:
                //        meshMaterialInfo.ValueRW.MeshID = activeAnimationTest.ValueRO.animationDataTestBlobAssetReference.Value.batchMeshIdBlobArray[activeAnimationTest.ValueRO.frame];
                //        break;
                //    case 1:
                //        meshMaterialInfo.ValueRW.MeshID = activeAnimationTest.ValueRO.animationDataTestBlobAssetReference.Value.batchMeshIdBlobArray[activeAnimationTest.ValueRO.frame];
                //        break;
                //}

                meshMaterialInfo.ValueRW.MeshID = animationDataTest.batchMeshIdBlobArray[activeAnimationTest.ValueRO.frame];

            }

            

        }
    }

    
}
