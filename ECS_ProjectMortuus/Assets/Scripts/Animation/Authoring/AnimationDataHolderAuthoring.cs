using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public class AnimationDataHolderAuthoring : MonoBehaviour
{

    public AnimationDataListSO animationDataListSO;

    public class Baker : Baker<AnimationDataHolderAuthoring>
    {
        public override void Bake(AnimationDataHolderAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);


            EntitiesGraphicsSystem entitiesGraphicsSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<EntitiesGraphicsSystem>();

            AnimationDataHolder animationDataHolder = new AnimationDataHolder();

            
            BlobBuilder blobBuilder = new BlobBuilder(Allocator.Temp);

            ref BlobArray<AnimationData> animationDataBlobArray = ref blobBuilder.ConstructRoot<BlobArray<AnimationData>>();


            BlobBuilderArray<AnimationData> animationDataBlobBuilderArray = blobBuilder.Allocate(ref animationDataBlobArray,System.Enum.GetValues(typeof(AnimationDataSO.AnimationType)).Length);

            int index = 0;

            foreach(AnimationDataSO.AnimationType animationType in System.Enum.GetValues(typeof(AnimationDataSO.AnimationType)))
            {
                AnimationDataSO animationDataSO = authoring.animationDataListSO.GetAnimationDataSO(animationType);

                BlobBuilderArray<BatchMeshID> batchMeshIdBlobBuilderArray = blobBuilder.Allocate(ref animationDataBlobBuilderArray[index].batchMeshIdBlobArray, animationDataSO.meshArray.Length);

                animationDataBlobBuilderArray[index].frameTimerMax = animationDataSO.frameTimerMax;
                animationDataBlobBuilderArray[index].frameMax = animationDataSO.meshArray.Length;

                for(int i=0;i< batchMeshIdBlobBuilderArray.Length;i++)
                {
                    Mesh mesh = animationDataSO.meshArray[i];

                    batchMeshIdBlobBuilderArray[i] = entitiesGraphicsSystem.RegisterMesh(mesh);
                }

                index++;    

            }

           
            animationDataHolder.animationDataBlobArrayBlobAssetReference = blobBuilder.CreateBlobAssetReference<BlobArray<AnimationData>>(Allocator.Persistent);
            

            blobBuilder.Dispose();

            AddBlobAsset(ref animationDataHolder.animationDataBlobArrayBlobAssetReference, out Unity.Entities.Hash128 objectHash);

            //{
            //    BlobBuilder blobBuilder = new BlobBuilder(Allocator.Temp);

            //    ref AnimationData animationData = ref blobBuilder.ConstructRoot<AnimationData>();

            //    animationData.frameMax = authoring.soldierWalk.meshArray.Length;
            //    animationData.frameTimerMax = authoring.soldierWalk.frameTimerMax;

            //    BlobBuilderArray<BatchMeshID> blobBuilderArray = blobBuilder.Allocate(ref animationData.batchMeshIdBlobArray, authoring.soldierWalk.meshArray.Length);


            //    for (int i = 0; i < blobBuilderArray.Length; i++)
            //    {
            //        Mesh mesh = authoring.soldierWalk.meshArray[i];

            //        blobBuilderArray[i] = entitiesGraphicsSystem.RegisterMesh(mesh);
            //    }

            //    animationDataHolder.soldierWalk = blobBuilder.CreateBlobAssetReference<AnimationData>(Allocator.Persistent);

            //    blobBuilder.Dispose();

            //    AddBlobAsset(ref animationDataHolder.soldierWalk, out Unity.Entities.Hash128 objectHash);

            //}

            AddComponent(entity, animationDataHolder);
        }
    }
}

public struct AnimationDataHolder : IComponentData
{
    //public BlobAssetReference<AnimationData> soldierIdle;
    //public BlobAssetReference<AnimationData> soldierWalk;
    public BlobAssetReference<BlobArray<AnimationData>> animationDataBlobArrayBlobAssetReference;
}

public struct AnimationData
{
    public int frameMax;
    public float frameTimerMax;
    public BlobArray<BatchMeshID> batchMeshIdBlobArray;
}
