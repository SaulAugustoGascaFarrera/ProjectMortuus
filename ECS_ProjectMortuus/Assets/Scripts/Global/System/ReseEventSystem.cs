using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(LateSimulationSystemGroup),OrderLast = true)]
partial struct ReseEventSystem : ISystem
{
    

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        new ResetShootAttackEventsJob().ScheduleParallel();

        new ResetHealthEventsJob().ScheduleParallel();

        new ResetSelectedEventsJob().ScheduleParallel();
    }

   
}


[BurstCompile]
public partial struct ResetShootAttackEventsJob : IJobEntity
{
    public void Execute(ref ShootAttack shootAttack)
    {
        shootAttack.onShoot.isTriggered = false;
    }
}

[BurstCompile]
public partial struct ResetHealthEventsJob : IJobEntity
{
    public void Execute(ref Health health)
    {
       health.onHealthChange = false;
    }
}

[BurstCompile]
//[WithPresent(typeof(Selected))]
[WithOptions(EntityQueryOptions.IgnoreComponentEnabledState)]
public partial struct ResetSelectedEventsJob : IJobEntity
{
    public void Execute(ref Selected selected)
    {
        selected.OnSelected = false;
        selected.OnDeselected = false;
    }
}
