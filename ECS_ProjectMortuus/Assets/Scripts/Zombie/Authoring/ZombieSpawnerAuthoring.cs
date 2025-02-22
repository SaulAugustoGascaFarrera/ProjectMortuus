using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class ZombieSpawnerAuthoring : MonoBehaviour
{

    public float timerMax;
    public float randomWalkingDistanceMin;
    public float randomWalkingDistanceMax;
    public Transform spawnPointGameObject;
    public class Baker : Baker<ZombieSpawnerAuthoring>
    {
        public override void Bake(ZombieSpawnerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new ZombieSpawner 
            { 
                timerMax = authoring.timerMax,
                randomWalkingDistanceMin = authoring.randomWalkingDistanceMin,
                randomWalkingDistanceMax = authoring.randomWalkingDistanceMax,
                spawnPointLocation = authoring.spawnPointGameObject.localPosition,
            });
        }
    }
}

public struct ZombieSpawner : IComponentData
{
    public float timer;
    public float timerMax;
    public float randomWalkingDistanceMin;
    public float randomWalkingDistanceMax;
    public float3 spawnPointLocation;
}
