using Unity.Entities;
using UnityEngine;

public class SelectedAuthoring : MonoBehaviour
{
    [Header("Selected Atts")]
    public GameObject visualGameObject;
    public float showScale;

    public class Baker : Baker<SelectedAuthoring>
    {
        public override void Bake(SelectedAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new Selected
            {
                visuaEntity = GetEntity(authoring.visualGameObject,TransformUsageFlags.Dynamic),
                showScale = authoring.showScale,
                
            });

            SetComponentEnabled<Selected>(entity,false);
        }
    }

}

public struct Selected : IComponentData,IEnableableComponent
{
    public Entity visuaEntity;
    public bool OnSelected;
    public bool OnDeselected;
    public float showScale;
}
