using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAt : MonoBehaviour
{
    [SerializeField]
    private Vector3 lookTarget;
    void Update()
    {
        transform.LookAt(lookTarget);
    }
}
