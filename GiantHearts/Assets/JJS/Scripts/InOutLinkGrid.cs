using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InOutLinkGrid : MonoBehaviour
{
    public Gridsystem gridsystem;
    void Start()
    {
        GameObject gridObject = gridsystem.GetGridObject(this.gameObject.transform.position);
        gridObject.GetComponent<GridObject>().platfrom = this.gameObject;
        gridObject.GetComponent<GridObject>().ItemType = GridObject.ObjectType.ItemWay;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
