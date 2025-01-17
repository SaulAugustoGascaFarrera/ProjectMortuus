using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public class AnimationDataHolderAuthoring : MonoBehaviour
{

    public AnimationDataSO soldierIdle;
    public AnimationDataSO soldierWalk;

    public class Baker : Baker<AnimationDataHolderAuthoring>
    {
        public override void Bake(AnimationDataHolderAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);


            EntitiesGraphicsSystem entitiesGraphicsSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<EntitiesGraphicsSystem>();

            AnimationDataHolder animationDataHolder = new AnimationDataHolder();

            {
                BlobBuilder blobBuilder = new BlobBuilder(Allocator.Temp);

                ref AnimationData animationData = ref blobBuilder.ConstructRoot<AnimationData>();

                animationData.frameTimerMax = authoring.soldierIdle.frameTimerMax;
                animationData.frameMax = authoring.soldierIdle.meshArray.Length;

                BlobBuilderArray<BatchMeshID> blobBuilderArray = blobBuilder.Allocate<BatchMeshID>(ref animationData.batchMeshIdBlobArray, authoring.soldierIdle.meshArray.Length);


                for(int i=0;i<blobBuilderArray.Length;i++)
                {
                    Mesh mesh = authoring.soldierIdle.meshArray[i];

                    blobBuilderArray[i] = entitiesGraphicsSystem.RegisterMesh(mesh);
                }

                animationDataHolder.soldierIdle = blobBuilder.CreateBlobAssetReference<AnimationData>(Allocator.Persistent);

                blobBuilder.Dispose();

                AddBlobAsset(ref animationDataHolder.soldierIdle, out Unity.Entities.Hash128 objectHash);
            }

            {
                BlobBuilder blobBuilder = new BlobBuilder(Allocator.Temp);

                ref AnimationData animationData = ref blobBuilder.ConstructRoot<AnimationData>();

                animationData.frameMax = authoring.soldierWalk.meshArray.Length;
                animationData.frameTimerMax = authoring.soldierWalk.frameTimerMax;

                BlobBuilderArray<BatchMeshID> blobBuilderArray = blobBuilder.Allocate(ref animationData.batchMeshIdBlobArray, authoring.soldierWalk.meshArray.Length);


                for (int i = 0; i < blobBuilderArray.Length; i++)
                {
                    Mesh mesh = authoring.soldierWalk.meshArray[i];

                    blobBuilderArray[i] = entitiesGraphicsSystem.RegisterMesh(mesh);
                }

                animationDataHolder.soldierWalk = blobBuilder.CreateBlobAssetReference<AnimationData>(Allocator.Persistent);

                blobBuilder.Dispose();

                AddBlobAsset(ref animationDataHolder.soldierWalk, out Unity.Entities.Hash128 objectHash);

            }

            AddComponent(entity, animationDataHolder);
        }
    }
}

public struct AnimationDataHolder : IComponentData
{
    public BlobAssetReference<AnimationData> soldierIdle;
    public BlobAssetReference<AnimationData> soldierWalk;
}

public struct AnimationData
{
    public int frameMax;
    public float frameTimerMax;
    public BlobArray<BatchMeshID> batchMeshIdBlobArray;
}
