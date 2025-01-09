using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

partial struct FindTargetSystem : ISystem
{
    

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        PhysicsWorldSingleton physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();  

        NativeList<DistanceHit> distanceHitsList = new NativeList<DistanceHit>(Allocator.Temp);

        

        foreach((RefRW<LocalTransform> localTransform,RefRW<Target> target,RefRW<FindTarget> findTarget) in SystemAPI.Query< RefRW<LocalTransform>,RefRW<Target>,RefRW<FindTarget>>())
        {


            distanceHitsList.Clear();


            CollisionFilter collisionFilter = new CollisionFilter
            {
                BelongsTo = ~0u,
                CollidesWith = 1u << 7,
                GroupIndex = 0
            }; 

            if(physicsWorldSingleton.OverlapSphere(localTransform.ValueRO.Position,findTarget.ValueRO.range,ref distanceHitsList, collisionFilter))
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
                        target.ValueRW.targetEntity = distanceHit.Entity;
                    }
 
               }
            }

        }
    }

    
}
