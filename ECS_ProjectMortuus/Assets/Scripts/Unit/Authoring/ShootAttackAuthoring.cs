using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class ShootAttackAuthoring : MonoBehaviour
{

    public float timerMax;
    public Transform bulletSpawnTransform;
    public float attackDistance;
    public class Baker : Baker<ShootAttackAuthoring>
    {
        public override void Bake(ShootAttackAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity,new ShootAttack 
            { 
                timerMax = authoring.timerMax,
                bulletSpawnPointLocation = authoring.bulletSpawnTransform.localPosition,
                attackDistance = authoring.attackDistance
            });
        }
    }
}

public struct ShootAttack : IComponentData
{
    public float timer;
    public float timerMax;
    public int damageAmount;
    public float3 bulletSpawnPointLocation;
    public float attackDistance;

}
