using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct ZombieSpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EntitiesReferences>();
    }    

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntitiesReferences entitiesReferences = SystemAPI.GetSingleton<EntitiesReferences>();

        EntityCommandBuffer entityCommandBuffer = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach((RefRW<ZombieSpawner> zombieSpawner,RefRW<LocalTransform> localTransform) in SystemAPI.Query<RefRW<ZombieSpawner>,RefRW<LocalTransform>>())
        {
            zombieSpawner.ValueRW.timer += SystemAPI.Time.DeltaTime;

            if(zombieSpawner.ValueRO.timer < zombieSpawner.ValueRO.timerMax)
            {
                continue;
            }

            zombieSpawner.ValueRW.timer = 0.0f;


            Entity zombieEntity = state.EntityManager.Instantiate(entitiesReferences.zombiePrefabEntity);
            //SystemAPI.SetComponent(zombieEntity, LocalTransform.FromPosition(localTransform.ValueRO.Position + new Unity.Mathematics.float3(4.0f,0.0f,-6.0f)));
            float3 spawnPosition = localTransform.ValueRO.TransformPoint(zombieSpawner.ValueRO.spawnPointLocation);
            SystemAPI.SetComponent(zombieEntity, LocalTransform.FromPosition(spawnPosition));

            entityCommandBuffer.AddComponent(zombieEntity, new RandomWalking
            {
                originPosition = localTransform.ValueRO.Position,
                targetPosition = localTransform.ValueRO.Position,
                distanceMin = zombieSpawner.ValueRO.randomWalkingDistanceMin,
                distanceMax = zombieSpawner.ValueRO.randomWalkingDistanceMax,
                random = new Unity.Mathematics.Random((uint) zombieEntity.Index)
            });
        }
    }

    
}
