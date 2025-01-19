using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public class AnimationDataHolderTestAuthoring : MonoBehaviour
{
    public AnimationDataSO soldierIdle;
    //public AnimationDataSO soldierWalk;

    public class Baker : Baker<AnimationDataHolderTestAuthoring>
    {
        public override void Bake(AnimationDataHolderTestAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AnimationDataHolderTest animationDataHolderTest = new AnimationDataHolderTest();


            EntitiesGraphicsSystem entitiesGraphicsSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<EntitiesGraphicsSystem>();


            BlobBuilder blobBuilder = new BlobBuilder(Allocator.Temp);

            ref BlobArray<AnimationDataTest> animationDataTestBlobArray = ref blobBuilder.ConstructRoot<BlobArray<AnimationDataTest>>();


            BlobBuilderArray<AnimationDataTest> animationDataTestBlobBuilderArray = blobBuilder.Allocate(ref animationDataTestBlobArray, 2);


            animationDataTestBlobBuilderArray[0].frameTimerMax = authoring.soldierIdle.frameTimerMax;
            animationDataTestBlobBuilderArray[0].frameMax = authoring.soldierIdle.meshArray.Length;


            BlobBuilderArray<BatchMeshID> batchMeshIdBlobBuilderArray = blobBuilder.Allocate(ref animationDataTestBlobBuilderArray[0].batchMeshIdBlobArray, authoring.soldierIdle.meshArray.Length);


            for(int i=0;i<batchMeshIdBlobBuilderArray.Length;i++)
            {
                Mesh mesh = authoring.soldierIdle.meshArray[i];

                batchMeshIdBlobBuilderArray[i] = entitiesGraphicsSystem.RegisterMesh(mesh); 
            }

            animationDataHolderTest.animationDataTestBlobAssetArray = blobBuilder.CreateBlobAssetReference<BlobArray<AnimationDataTest>>(Allocator.Persistent);

            blobBuilder.Dispose();


            AddBlobAsset(ref animationDataHolderTest.animationDataTestBlobAssetArray,out Unity.Entities.Hash128 hash128);

            AddComponent(entity, animationDataHolderTest);
        }
    }
}


public struct AnimationDataHolderTest : IComponentData
{
    //public BlobAssetReference<AnimationDataTest> soldierIdle;
    //public BlobAssetReference<AnimationDataTest> soldierWalk;

    public BlobAssetReference<BlobArray<AnimationDataTest>> animationDataTestBlobAssetArray;
}

public struct AnimationDataTest
{
    public int frameMax;
    public float frameTimerMax;
    public BlobArray<BatchMeshID> batchMeshIdBlobArray;
}
