using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

partial struct ZombieSpawnerSystem : ISystem
{
    

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntitiesReferences entitiesReferences = SystemAPI.GetSingleton<EntitiesReferences>();   


        foreach((RefRW<ZombieSpawner> zombieSpawner,RefRW<LocalTransform> localTransform) in SystemAPI.Query<RefRW<ZombieSpawner>,RefRW<LocalTransform>>())
        {
            zombieSpawner.ValueRW.timer += SystemAPI.Time.DeltaTime;

            if(zombieSpawner.ValueRO.timer < zombieSpawner.ValueRO.timerMax)
            {
                continue;
            }

            zombieSpawner.ValueRW.timer = 0.0f;


            Entity zombieEntity = state.EntityManager.Instantiate(entitiesReferences.zombiePrefabEntity);
            SystemAPI.SetComponent(zombieEntity, LocalTransform.FromPosition(localTransform.ValueRO.Position + new Unity.Mathematics.float3(4.0f,0.0f,-6.0f)));
        }
    }

    
}
