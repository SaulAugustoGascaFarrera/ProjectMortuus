using Unity.Burst;
using Unity.Entities;

partial struct TestZombieAttack : ISystem
{
   

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        //foreach((RefRW<Zombie> zombie,RefRO<Target> target) in SystemAPI.Query<RefRW<Zombie>,RefRO<Target>>())
        //{
        //    if(!SystemAPI.Exists(target.ValueRO.targetEntity))
        //    {
        //        continue;
        //    }

        //    zombie.ValueRW.timer -= SystemAPI.Time.DeltaTime;

        //    if(zombie.ValueRO.timer > 0.0f)
        //    {
        //        continue;
        //    }

        //    zombie.ValueRW.timer = zombie.ValueRO.timerMax;

        //    RefRW<Health> targetHealth = SystemAPI.GetComponentRW<Health>(target.ValueRO.targetEntity);
        //    targetHealth.ValueRW.onHealthChange = true;
        //    targetHealth.ValueRW.healthAmount -= 5;


        //}
    }

   
}
