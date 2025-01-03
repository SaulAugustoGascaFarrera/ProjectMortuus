using Unity.Entities;
using UnityEngine;

public class UnitAuthoring : MonoBehaviour
{
    public class Baker : Baker<UnitAuthoring>
    {
        public override void Bake(UnitAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new Unit { timerMax = 1.2f });
        }
    }
}

public struct Unit : IComponentData
{
    public float timer;
    public float timerMax;
}
