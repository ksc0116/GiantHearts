using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogisticsObject : MonoBehaviour
{

    public enum LogisticsObjectType
    {
        None,
        Coal,
        Copper,
        Iron,
        Meteorite,
        Mythril,
        MythrilAndCoal
    }

    public float locationOffset;

    public LogisticsObjectType type;

    private ObjectPoolSystem objectPool;

    void Start()
    {
        objectPool = Manager.Instance.objectPool;
    }
    private void OnDisable()
    {
        if(objectPool != null)
        {
            objectPool.GiveBackObject(this.gameObject);
        }
    }
}
