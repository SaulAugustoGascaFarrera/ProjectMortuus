using Unity.Burst;
using Unity.Entities;

partial struct TestUnitAttack : ISystem
{
    

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach((RefRW<Unit> unit,RefRO<Target> target) in SystemAPI.Query<RefRW<Unit>,RefRO<Target>>())
        {

            if(!SystemAPI.Exists(target.ValueRO.targetEntity))
            {
                continue;
            }

            

            unit.ValueRW.timer -= SystemAPI.Time.DeltaTime;

            if(unit.ValueRW.timer > 0.0f)
            {
                continue;
            }

            unit.ValueRW.timer = unit.ValueRO.timerMax;

            

            RefRW<Health> targetHealth = SystemAPI.GetComponentRW<Health>(target.ValueRO.targetEntity);
            targetHealth.ValueRW.onHealthChange = true;
            targetHealth.ValueRW.healthAmount -= 20;


        }
    }

}
