using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceDoorTest : MonoBehaviour
{
    public GameObject door0;
    public GameObject door1;
    public GameObject door2_1;
    public GameObject door2_2;
    public GameObject door3;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F6))
        {
            door0.GetComponent<DoorLight>().Correct();
        }
        if (Input.GetKeyDown(KeyCode.F7))
        {
            door1.GetComponent<DoorLight>().Correct();
        }
        if (Input.GetKeyDown(KeyCode.F8))
        {
            door2_1.GetComponent<DoorLight>().Correct();
        }
        if (Input.GetKeyDown(KeyCode.F9))
        {
            door2_2.GetComponent<DoorLight>().Correct();
        }
        if (Input.GetKeyDown(KeyCode.F10))
        {
            door3.GetComponent<DoorLight>().Correct();
        }
    }
}
