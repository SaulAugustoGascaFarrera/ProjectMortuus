using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public class ActiveAnimationAuthoring : MonoBehaviour
{

    public AnimationDataSO.AnimationType nextAnimationType;
    public class Baker : Baker<ActiveAnimationAuthoring>
    {
        public override void Bake(ActiveAnimationAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);


            EntitiesGraphicsSystem entitiesGraphicsSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<EntitiesGraphicsSystem>();

            AddComponent(entity, new ActiveAnimation
            {
               nextAnimationType = authoring.nextAnimationType,
            });

        }
    }
}


public struct ActiveAnimation : IComponentData
{
    public int frame;
    public float frameTimer;
    //public BlobAssetReference<AnimationData> animationDataBlobAssetReference;
    //public int activeAnimationIndex;
    public AnimationDataSO.AnimationType activeAnimationType;
    public AnimationDataSO.AnimationType nextAnimationType;
    
}