using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

partial struct FindTargetSystem : ISystem
{
    

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        PhysicsWorldSingleton physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();  

        CollisionWorld collisionWorld = physicsWorldSingleton.CollisionWorld;

        NativeList<DistanceHit> distanceHitsList = new NativeList<DistanceHit>(Allocator.Temp);

       

        foreach((RefRW<LocalTransform> localTransform,RefRW<Target> target,RefRW<FindTarget> findTarget,RefRO<TargetOveride> targetOverride) in SystemAPI.Query< RefRW<LocalTransform>,RefRW<Target>,RefRW<FindTarget>, RefRO<TargetOveride>>())
        {

            findTarget.ValueRW.timer -= SystemAPI.Time.DeltaTime;

            if(findTarget.ValueRO.timer > 0.0f)
            {
                continue;
            }

            findTarget.ValueRW.timer = findTarget.ValueRO.timerMax;


            if(targetOverride.ValueRO.targetEntity != Entity.Null)
            {
                target.ValueRW.targetEntity = targetOverride.ValueRO.targetEntity;
                continue;
            }

            distanceHitsList.Clear();


            CollisionFilter collisionFilter = new CollisionFilter
            {
                BelongsTo = ~0u,
                CollidesWith = 1u << 7,
                GroupIndex = 0
            }; 


            Entity closestTargetEntity = Entity.Null;

            float closestTargetDistance = float.MaxValue;

            float currentTargetDistanceOffset = 0.0f;

            if(target.ValueRO.targetEntity != Entity.Null)
            {
                closestTargetEntity = target.ValueRO.targetEntity;

                LocalTransform targetLocalTransform = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.targetEntity);

                closestTargetDistance = math.distance(localTransform.ValueRO.Position, targetLocalTransform.Position);

                currentTargetDistanceOffset = 2.0f;
            }


            if(collisionWorld.OverlapSphere(localTransform.ValueRO.Position,findTarget.ValueRO.range,ref distanceHitsList, collisionFilter))
            {
               foreach(DistanceHit distanceHit in distanceHitsList)
               {
                    if(!SystemAPI.Exists(distanceHit.Entity) || !SystemAPI.HasComponent<Faction>(distanceHit.Entity))
                    {
                        continue;
                    }

                    Unit targetUnit = SystemAPI.GetComponent<Unit>(distanceHit.Entity);
                    
                    if(targetUnit.faction == findTarget.ValueRO.targetFaction)
                    {
                        if(closestTargetEntity == Entity.Null)
                        {
                            //target.ValueRW.targetEntity = distanceHit.Entity;
                            closestTargetEntity = distanceHit.Entity;

                            closestTargetDistance = distanceHit.Distance;

                        }
                        else
                        {
                           if(distanceHit.Distance + currentTargetDistanceOffset < closestTargetDistance)
                           {
                                closestTargetEntity = distanceHit.Entity;

                                closestTargetDistance = distanceHit.Distance;
                           }
                        }

                        //break;
                    }
 
               }
            }

            if(closestTargetEntity != Entity.Null)
            {
                target.ValueRW.targetEntity = closestTargetEntity;
            }
            

        }
    }

    
}
