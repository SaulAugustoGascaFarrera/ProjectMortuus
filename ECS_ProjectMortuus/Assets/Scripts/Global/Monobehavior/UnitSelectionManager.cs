using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class UnitSelectionManager : MonoBehaviour
{
    public static UnitSelectionManager Instance;



    public event EventHandler OnSelectionAreaStart;
    public event EventHandler OnSelectionAreaEnd;

    private Vector2 selectionStartMousePosition;


    private void Awake()
    {
        if(Instance != null)
        {
            return;
        }


        Instance = this;
    }


    // Update is called once per frame
    void Update()
    {
        SelectionManager();
    }


    public void SelectionManager()
    {
        if (Input.GetMouseButtonDown(0))
        {
            selectionStartMousePosition = Input.mousePosition;

            OnSelectionAreaStart?.Invoke(this, EventArgs.Empty);


        }

        if (Input.GetMouseButtonUp(0))
        {
            OnSelectionAreaEnd?.Invoke(this, EventArgs.Empty);

            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            EntityQuery entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<Selected>().Build(entityManager);

            NativeArray<Entity> entityArray = entityQuery.ToEntityArray(Allocator.Temp);
            NativeArray<Selected> selectedArray = entityQuery.ToComponentDataArray<Selected>(Allocator.Temp);



            for(int i=0;i<entityArray.Length;i++)
            {
                Selected selected = selectedArray[i];

                selected.OnSelected = false;
                selected.OnDeselected = true;

                entityManager.SetComponentEnabled<Selected>(entityArray[i], false);


                entityManager.SetComponentData(entityArray[i], selected);
            }



            Rect selectionAreaRect = GetSelectionAreaReact();
            float selectionAreaSize = selectionAreaRect.width * selectionAreaRect.height;
            float multipleSelectionAreaMin = 40.0f;
            bool isMultipleSelecttion = selectionAreaSize > multipleSelectionAreaMin;


            if (isMultipleSelecttion)
            {
                entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<LocalTransform, UnitMover>().WithDisabled<Selected>().Build(entityManager);

                entityArray = entityQuery.ToEntityArray(Allocator.Temp);

                NativeArray<LocalTransform> localTransformArray = entityQuery.ToComponentDataArray<LocalTransform>(Allocator.Temp);

                for (int i = 0; i < localTransformArray.Length; i++)
                {


                    LocalTransform localTransform = localTransformArray[i];

                    //Selected selected = selectedArray[i];

                    Selected selected = entityManager.GetComponentData<Selected>(entityArray[i]);

                    Vector2 unitPosition = Camera.main.WorldToScreenPoint(localTransform.Position);

                    if (selectionAreaRect.Contains(unitPosition))
                    {
                        entityManager.SetComponentEnabled<Selected>(entityArray[i], true);

                        selected.OnSelected = true;
                        selected.OnDeselected = false;

                        entityManager.SetComponentData(entityArray[i], selected);
                    }




                }
            }
            else
            {
                entityQuery = entityManager.CreateEntityQuery(typeof(PhysicsWorldSingleton));
                
                PhysicsWorldSingleton physicsWorldSingleton = entityQuery.GetSingleton<PhysicsWorldSingleton>();

                CollisionWorld collisionWorld = physicsWorldSingleton.CollisionWorld;


                UnityEngine.Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastInput raycastInput = new RaycastInput
                {
                    Start = ray.GetPoint(0),
                    End = ray.GetPoint(9999.0f), 
                    Filter = new CollisionFilter
                    {
                        GroupIndex = 0,
                        CollidesWith = 1u << 7,
                        BelongsTo = ~0u
                    }
                };


                if(collisionWorld.CastRay(raycastInput,out Unity.Physics.RaycastHit hit))
                {
                    if(entityManager.HasComponent<Selected>(hit.Entity))
                    {
                        Selected selected = entityManager.GetComponentData<Selected>(hit.Entity);

                        entityManager.SetComponentEnabled<Selected>(hit.Entity, true);

                        selected.OnSelected = true;
                        selected.OnDeselected = false;

                        entityManager.SetComponentData(hit.Entity, selected);
                    }
                }

            }

            

        }


        if (Input.GetMouseButtonDown(1))
        {

            Vector3 mousePosition = MouseManager.Instance.GetMousePosition();

            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            EntityQuery entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<UnitMover,Selected>().Build(entityManager);

            NativeArray<Entity> entityArray = entityQuery.ToEntityArray(Allocator.Temp);
            NativeArray<UnitMover> unitMoverArray = entityQuery.ToComponentDataArray<UnitMover>(Allocator.Temp);


            NativeArray<float3> movePositionArray = GenerateMovePositionArray(entityArray.Length, mousePosition);


            for (int i = 0; i < entityArray.Length; i++)
            {
                UnitMover unitMover = unitMoverArray[i];

                unitMover.targetPosition = movePositionArray[i];

                //entityManager.SetComponentData(entityArray[i], unit);    

                unitMoverArray[i] = unitMover;


                entityManager.SetComponentEnabled<Selected>(entityArray[i], true);

            }

            entityQuery.CopyFromComponentDataArray(unitMoverArray);



        }

    }


    public NativeArray<float3> GenerateMovePositionArray(int positionCount,float3 targetPosition)
    {
        NativeArray<float3> positionArray = new NativeArray<float3>(positionCount,Allocator.Temp);

        if(positionCount == 0)
        {
            return positionArray;
        }

        for(int i=0;i<positionArray.Length; i++)
        {
            float angle = i * (2.0f * Mathf.PI / positionCount);


            float3 position = new float3(math.cos(angle),0.0f,math.sin(angle)) * 5.0f + targetPosition; 

            positionArray[i] = position;
        }

        return positionArray;
    }



    public Rect GetSelectionAreaReact()
    {
        Vector2 selectionEndMousePosition = Input.mousePosition;

        Vector2 lowerLeftCorner = new Vector2(Mathf.Min(selectionStartMousePosition.x, selectionEndMousePosition.x), Mathf.Min(selectionStartMousePosition.y, selectionEndMousePosition.y));

        Vector2 upperRightCorner = new Vector2(Mathf.Max(selectionStartMousePosition.x, selectionEndMousePosition.x), Mathf.Max(selectionStartMousePosition.y, selectionEndMousePosition.y));

        Rect selectionRect = new Rect(lowerLeftCorner.x, lowerLeftCorner.y, upperRightCorner.x - lowerLeftCorner.x, upperRightCorner.y - lowerLeftCorner.y);

        return selectionRect;
    }

}
