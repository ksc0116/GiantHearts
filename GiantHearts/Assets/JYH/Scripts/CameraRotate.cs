using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    // KSC
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float rotateWeight;

    private float curPosX;
    private int width;
    private float mouseXDelta;
    private float curRotaetY;

    // UI Object
    public GameObject mainMeunUIObject;

    void Start()
    {
        width = Screen.width;
    }

    void Update()
    {
        // MainMeun UI 비활성화일 경우에만
        if (mainMeunUIObject.activeSelf == false)
        {
            if (Input.GetMouseButtonDown(1))
            {
                curPosX = Input.mousePosition.x;
            }

            if (Input.GetMouseButton(1))
            {
                mouseXDelta = curPosX - Input.mousePosition.x;
                curPosX = Input.mousePosition.x;
                ObjectRotate();
                //Debug.Log(mouseXDelta);
            }

            if (Input.GetMouseButtonUp(1))
            {
                curPosX = 0;
                mouseXDelta = 0;
            }
        }
    }

    private void ObjectRotate()
    {
        if (mouseXDelta > 0)
        {
            curRotaetY -= (mouseXDelta*rotateWeight);
        }
        else if (mouseXDelta < 0)
        {
            curRotaetY -= (mouseXDelta * rotateWeight);
        }
       
        transform.rotation = Quaternion.Euler(0, curRotaetY, 0);
    }
}
