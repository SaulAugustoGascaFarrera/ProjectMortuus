using Unity.Entities;
using UnityEngine;
using UnityEngine.Rendering;

public class ActiveAnimationTestAuthoring : MonoBehaviour
{
    public class Baker : Baker<ActiveAnimationTestAuthoring>
    {
        public override void Bake(ActiveAnimationTestAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new ActiveAnimationTest { });
        }
    }
}


public struct ActiveAnimationTest : IComponentData
{
    public int frame;
    public float frameTimer;
    //public BlobAssetReference<AnimationDataTest> animationDataTestBlobAssetReference;
    public int activeAnimationIndex;
}
