using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct BulletMoverSystem : ISystem
{
    

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach((RefRW<LocalTransform> localTransform,RefRO<Target> target,RefRO<Bullet> bullet,Entity entity) in SystemAPI.Query<RefRW<LocalTransform>,RefRO<Target>,RefRO<Bullet>>().WithEntityAccess())
        {
            if(!SystemAPI.Exists(target.ValueRO.targetEntity))
            {
                entityCommandBuffer.DestroyEntity(entity);  
                continue;
            }

            RefRO<LocalTransform> targetLocalTransform = SystemAPI.GetComponentRO<LocalTransform>(target.ValueRO.targetEntity);
            ShootVictim targetHitPosition = SystemAPI.GetComponent<ShootVictim>(target.ValueRO.targetEntity);
            float3 targetPositin = targetLocalTransform.ValueRO.TransformPoint(targetHitPosition.hitLocalPosition);

           float distanceBefore = math.distancesq(localTransform.ValueRO.Position, targetLocalTransform.ValueRO.Position);

           

            float3 moveDirection = targetPositin - localTransform.ValueRO.Position;

            moveDirection = math.normalize(moveDirection);

            localTransform.ValueRW.Position += moveDirection * bullet.ValueRO.movementSpeed * SystemAPI.Time.DeltaTime;


            float distanceAfter = math.distancesq(localTransform.ValueRO.Position, targetLocalTransform.ValueRO.Position);

            if(distanceAfter > distanceBefore)
            {
                localTransform.ValueRW.Position = targetLocalTransform.ValueRO.Position;
            }

            float distanceToDestroy = 0.2f;

            if (math.distancesq(localTransform.ValueRO.Position, targetLocalTransform.ValueRO.Position) < distanceToDestroy)
            {
                RefRW<Health> targetHealth = SystemAPI.GetComponentRW<Health>(target.ValueRO.targetEntity);
                targetHealth.ValueRW.healthAmount -= bullet.ValueRO.damageAmount;
                targetHealth.ValueRW.onHealthChange = true;

                entityCommandBuffer.DestroyEntity(entity);
            }

        }
    }

    
}


//public partial struct BulletMoverJob : IJobEntity
//{
//    public void Execute(ref Entity entity)
//    {
        
//    }
//}
