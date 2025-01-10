using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct MeleeAttackSystem : ISystem
{
    
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach((RefRW<LocalTransform> localTransform,RefRW<MeleeAttack> meleeAttack,RefRW<UnitMover> unitMover,RefRO<Target> target) in SystemAPI.Query<RefRW<LocalTransform>,RefRW<MeleeAttack>,RefRW<UnitMover>, RefRO<Target>>())
        {

            if(!SystemAPI.Exists(target.ValueRO.targetEntity))
            {
                continue;
            }

            LocalTransform targetLocalTransform = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.targetEntity);
            
            

            if(math.distancesq(targetLocalTransform.Position,localTransform.ValueRO.Position) > meleeAttack.ValueRO.distanceToAttack)
            {
                //float3 moveDirection = targetLocalTransform.Position - localTransform.ValueRO.Position;
                
                //moveDirection = math.normalize(moveDirection);

                //localTransform.ValueRW.Rotation = math.slerp(localTransform.ValueRO.Rotation, quaternion.LookRotation(moveDirection, math.up()), unitMover.ValueRO.rotationSpeed * SystemAPI.Time.DeltaTime);

                unitMover.ValueRW.targetPosition = targetLocalTransform.Position;

            }
            else
            {
                unitMover.ValueRW.targetPosition = localTransform.ValueRO.Position;

                meleeAttack.ValueRW.timer -= SystemAPI.Time.DeltaTime;

                if(meleeAttack.ValueRO.timer > 0.0f)
                {
                    continue;
                }

                meleeAttack.ValueRW.timer = meleeAttack.ValueRO.timerMax;

                RefRW<Health> targetHealth = SystemAPI.GetComponentRW<Health>(target.ValueRO.targetEntity);
                targetHealth.ValueRW.healthAmount -= meleeAttack.ValueRO.damageAmount;

            }

        }
    }

}
