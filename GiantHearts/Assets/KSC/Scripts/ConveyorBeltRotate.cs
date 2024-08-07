using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBeltRotate : MonoBehaviour
{
    [SerializeField]
    int rotateDir;
    float rotateSpeed = 16.0f;
    void Update()
    {
        transform.Rotate(new Vector3(0,0,1* rotateDir * rotateSpeed*Time.deltaTime));        
    }
}
