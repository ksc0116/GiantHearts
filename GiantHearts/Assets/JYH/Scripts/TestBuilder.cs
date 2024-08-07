using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBuilder : MonoBehaviour
{
    [SerializeField]
    private GameObject StartGenerator;
    [SerializeField]
    private GameObject endReceiver;

    public List<GameObject> Conveyor;

    private LogisticsTransfer selected;

    // Start is called before the first frame update
    void Start()
    {
        selected = StartGenerator.GetComponent<LogisticsTransfer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameObject belt = Instantiate(Conveyor[0], selected.transform.position, Quaternion.identity);
            selected.ConnectNextBelt(belt.GetComponent<LogisticsTransfer>());
            selected = belt.GetComponent<LogisticsTransfer>();
        }

        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameObject belt = Instantiate(Conveyor[1], selected.transform.position, Quaternion.identity);
            selected.ConnectNextBelt(belt.GetComponent<LogisticsTransfer>());
            selected = belt.GetComponent<LogisticsTransfer>();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GameObject belt = Instantiate(Conveyor[2], selected.transform.position, Quaternion.identity);
            selected.ConnectNextBelt(belt.GetComponent<LogisticsTransfer>());
            selected = belt.GetComponent<LogisticsTransfer>();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            GameObject belt = Instantiate(Conveyor[3], selected.transform.position, Quaternion.identity);
            selected.ConnectNextBelt(belt.GetComponent<LogisticsTransfer>());
            selected = belt.GetComponent<LogisticsTransfer>();
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            GameObject belt = Instantiate(Conveyor[4], selected.transform.position, Quaternion.identity);
            selected.ConnectNextBelt(belt.GetComponent<LogisticsTransfer>());
            selected = belt.GetComponent<LogisticsTransfer>();
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            GameObject belt = Instantiate(Conveyor[5], selected.transform.position, Quaternion.identity);
            selected.ConnectNextBelt(belt.GetComponent<LogisticsTransfer>());
            selected = belt.GetComponent<LogisticsTransfer>();
        }



    }
}
