using Unity.VisualScripting;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public LayerMask mask;
    public static MouseManager Instance;


    private void Awake()
    {
        if(Instance != null)
        {
            return;
        }

        Instance = this;
    }


    private void Update()
    {
        //if(Input.GetMouseButtonDown(1))
        //{
        //    //GetMousePosition();
        //    Debug.Log("fsdgdfdgfdgfdgf");
        //}
    }


    public Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, 1 << 6))
        {
            //Debug.Log(raycastHit.point);

            return raycastHit.point;
        }


        return Vector3.zero;
    }

}
