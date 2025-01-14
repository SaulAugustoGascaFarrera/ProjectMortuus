using Unity.Entities;
using UnityEngine;

public class TargetOverrideAuthoring : MonoBehaviour
{
    public class Baker : Baker<TargetOverrideAuthoring>
    {
        public override void Bake(TargetOverrideAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new TargetOveride { });
        }
    }
}

public struct TargetOveride : IComponentData 
{
    public Entity targetEntity;
}
