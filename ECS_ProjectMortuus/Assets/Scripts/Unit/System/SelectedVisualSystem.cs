using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

partial struct SelectedVisualSystem : ISystem
{
    

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach(RefRO<Selected> selected in SystemAPI.Query<RefRO<Selected>>().WithPresent<Selected>())
        {
            if(selected.ValueRO.OnDeselected)
            {
                RefRW<LocalTransform> localTransform = SystemAPI.GetComponentRW<LocalTransform>(selected.ValueRO.visuaEntity);
                localTransform.ValueRW.Scale = 0.0f;
            }

            if (selected.ValueRO.OnSelected)
            {
                RefRW<LocalTransform> localTransform = SystemAPI.GetComponentRW<LocalTransform>(selected.ValueRO.visuaEntity);
                localTransform.ValueRW.Scale = selected.ValueRO.showScale;
            }
        }
    }

   
}
