using Unity.Entities;
using UnityEngine;

public class FactionAuthoring : MonoBehaviour
{
    public FactionType faction;
   public class Baker : Baker<FactionAuthoring>
    {
        public override void Bake(FactionAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new Faction 
            { 
                faction = authoring.faction,
            });
        }

    }
}

public struct Faction : IComponentData
{
    public FactionType faction;
}
