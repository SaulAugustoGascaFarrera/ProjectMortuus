using Unity.Entities;
using UnityEngine;

public class ZombieAuthoring : MonoBehaviour
{
    public float timerMax;
    public class Baker : Baker<ZombieAuthoring>
    {
        public override void Bake(ZombieAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new Zombie { timerMax = authoring.timerMax });
        }
    }
}

public struct Zombie : IComponentData
{
    public float timer;
    public float timerMax;
}
