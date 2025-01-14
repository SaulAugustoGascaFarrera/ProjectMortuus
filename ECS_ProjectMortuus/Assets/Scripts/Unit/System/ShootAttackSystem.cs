using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct ShootAttackSystem : ISystem
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

        foreach((RefRW<LocalTransform> localTransform,RefRO<Target> target,RefRW<ShootAttack> shootAttack,RefRW<UnitMover> unitMover,Entity entity) in SystemAPI.Query<RefRW<LocalTransform>,RefRO<Target> ,RefRW<ShootAttack>,RefRW<UnitMover>>().WithDisabled<MoveOverride>().WithEntityAccess())
        {
            if(!SystemAPI.Exists(target.ValueRO.targetEntity))
            {
                continue;
            }

            LocalTransform targetLocation = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.targetEntity);


            if(math.distance(localTransform.ValueRO.Position,targetLocation.Position) > shootAttack.ValueRO.attackDistance)
            {
                //too far,move close
                unitMover.ValueRW.targetPosition = targetLocation.Position;
                continue;
            }
            else
            {
                //close enough
                unitMover.ValueRW.targetPosition = localTransform.ValueRO.Position;
            }


            float3 aimDirection = targetLocation.Position - localTransform.ValueRO.Position; 

            aimDirection = math.normalize(aimDirection);    


            localTransform.ValueRW.Rotation = math.slerp(localTransform.ValueRO.Rotation,quaternion.LookRotation(aimDirection,math.up()),unitMover.ValueRO.rotationSpeed * SystemAPI.Time.DeltaTime);


            shootAttack.ValueRW.timer -= SystemAPI.Time.DeltaTime;

            if (shootAttack.ValueRO.timer > 0.0f)
            {
                continue;
            }

            shootAttack.ValueRW.timer = shootAttack.ValueRO.timerMax;

            RefRW<TargetOveride> enemyTargetOverride  = SystemAPI.GetComponentRW<TargetOveride>(target.ValueRO.targetEntity);

            if (enemyTargetOverride.ValueRO.targetEntity == Entity.Null)
            {
                enemyTargetOverride.ValueRW.targetEntity = entity;
            }

            Entity bulletEntity = state.EntityManager.Instantiate(entitiesReferences.bulletPrefabEntity);
            float3 bulletSpawnPoint = localTransform.ValueRO.TransformPoint(shootAttack.ValueRO.bulletSpawnPointLocation);
            SystemAPI.SetComponent(bulletEntity,LocalTransform.FromPosition(bulletSpawnPoint));

            RefRW<Bullet> bullet = SystemAPI.GetComponentRW<Bullet>(bulletEntity);
            shootAttack.ValueRW.damageAmount = bullet.ValueRO.damageAmount;


            RefRW<Target> bulletTarget = SystemAPI.GetComponentRW<Target>(bulletEntity);
            bulletTarget.ValueRW.targetEntity = target.ValueRO.targetEntity;    

        }
    }

}
