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

            BlobBuilderArray<AnimationDataTest> animationDataTestBlobBuilderArray = blobBuilder.Allocate(ref animationDataTestBlobArray,2);

            animationDataTestBlobBuilderArray[0].frameTimerMax = authoring.soldierIdle.frameTimerMax;
            animationDataTestBlobBuilderArray[0].frameMax = authoring.soldierIdle.meshArray.Length;


            BlobBuilderArray<BatchMeshID> batchMeshIdBlobBuilderArray = blobBuilder.Allocate(ref animationDataTestBlobBuilderArray[0].batchMeshIdBlobArray, authoring.soldierIdle.meshArray.Length);

            for (int i=0;i< batchMeshIdBlobBuilderArray.Length;i++)
            {
                Mesh mesh = authoring.soldierIdle.meshArray[i];

                batchMeshIdBlobBuilderArray[i] = entitiesGraphicsSystem.RegisterMesh(mesh);
            }


            animationDataHolderTest.animationDataTestBlobAssetArray = blobBuilder.CreateBlobAssetReference<BlobArray<AnimationDataTest>>(Allocator.Persistent);

            AddBlobAsset(ref animationDataHolderTest.animationDataTestBlobAssetArray, out Unity.Entities.Hash128 objectHash);


            //{
            //    BlobBuilder blobBuilder1 = new BlobBuilder(Allocator.Temp);

            //    ref AnimationDataTest animationDataTest = ref blobBuilder1.ConstructRoot<AnimationDataTest>();


            //   BlobBuilderArray<BatchMeshID> batchMeshIdBlobBuilderArray1  = blobBuilder.Allocate(ref animationDataTest.batchMeshIdBlobArray, authoring.soldierIdle.meshArray.Length);


            //    for(int i=0;i<batchMeshIdBlobBuilderArray1.Length;i++)
            //    {
            //        Mesh mesh = authoring.soldierIdle.meshArray[i];

            //        batchMeshIdBlobBuilderArray1[i] = entitiesGraphicsSystem.RegisterMesh(mesh);
            //    }

            //    animationDataHolderTest.soldierIdle = blobBuilder.CreateBlobAssetReference<AnimationDataTest>(Allocator.Persistent);

            //    AddBlobAsset(ref animationDataHolderTest.soldierIdle, out Unity.Entities.Hash128 objectHash1);

            //}


            AddComponent(entity, animationDataHolderTest);
        }
    }
}


public struct AnimationDataHolderTest : IComponentData
{
    public BlobAssetReference<AnimationDataTest> soldierIdle;
    //public BlobAssetReference<AnimationDataTest> soldierWalk;

    public BlobAssetReference<BlobArray<AnimationDataTest>> animationDataTestBlobAssetArray;
}

public struct AnimationDataTest
{
    public int frameMax;
    public float frameTimerMax;
    public BlobArray<BatchMeshID> batchMeshIdBlobArray;
}
