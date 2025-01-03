using Unity.Entities;
using UnityEngine;

public class EntitiesReferencesAuthoring : MonoBehaviour
{

    public GameObject zombiePrefabGameObject;
    public class Baker : Baker<EntitiesReferencesAuthoring>
    {
        public override void Bake(EntitiesReferencesAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new EntitiesReferences
            {
                zombiePrefabEntity = GetEntity(authoring.zombiePrefabGameObject,TransformUsageFlags.Dynamic),
            });
        }
    }
}


public struct EntitiesReferences : IComponentData
{
    public Entity zombiePrefabEntity;
}