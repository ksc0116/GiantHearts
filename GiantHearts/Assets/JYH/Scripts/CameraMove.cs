using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // JYH
    /*[SerializeField]
    private Vector3 lookLocation = new(0, 7.5f, 0);
    [SerializeField]
    private float rotationSpeed = 10;

    private Vector3[] targetLocations = new Vector3[4];
    private int targetLocationIndex;

    void Start()
    {
        transform.LookAt(lookLocation);

        float x = transform.position.x;
        float y = transform.position.y;
        float z = transform.position.z;
        targetLocations[0] = new Vector3(x, y, z);
        targetLocations[1] = new Vector3(-x, y, z);
        targetLocations[2] = new Vector3(-x, y, -z);
        targetLocations[3] = new Vector3(x, y, -z);

        targetLocationIndex = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            targetLocationIndex = (targetLocationIndex + 1) % targetLocations.Length;
        }

        Vector3 center = new(0, targetLocations[targetLocationIndex].y, 0);
        Vector3 origin = transform.position - center;
        Vector3 target = targetLocations[targetLocationIndex] - center;

        transform.position = Vector3.Slerp(origin, target, Time.deltaTime * rotationSpeed);
        transform.position += center;
        transform.LookAt(lookLocation);
    }*/

    // KSC
    [SerializeField]
    private float rotationSpeed;
    private float curPosX;
    new private Camera camera;
    private int width;
    private float mouseXDelta;

    void Start()
    {
        camera = Camera.main; 
        width = Screen.width;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            curPosX = Input.mousePosition.x;
        }

        if(Input.GetMouseButton(1))
        {
            //mouseXDelta += Input.GetAxis("Mouse X");
            //Debug.Log(mouseXDelta);
            //Debug.Log(Input.mousePosition.x);
            
            mouseXDelta = Mathf.Abs(curPosX - Input.mousePosition.x);
            Debug.Log(mouseXDelta);
        }

        if (Input.GetMouseButtonUp(1))
        {
            curPosX = 0;
            mouseXDelta = 0;
        }
    }

}
