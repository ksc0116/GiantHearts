using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualDebug : MonoBehaviour
{
    [SerializeField] private GameObject cubeObject;

    void Update()
    {
        if((Input.GetKey(KeyCode.V)) && (Input.GetKeyDown(KeyCode.D)))
        {
            if(cubeObject.activeSelf)
            {
                cubeObject.SetActive(false);
            }
            else
            {
                cubeObject.SetActive(true);
            }
        }
    }
}
