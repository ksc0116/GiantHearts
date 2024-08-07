using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnConveyorBelt : LogisticsTransfer
{
    private GameObject centerObject;

    protected override void SetMainInput(LogisticsTransfer mainInput)
    {
        mainInputBelt = mainInput;

        AlignMainInput(mainInput);
    }

    public override void SetMainInputAtRigntOut(Splitter splitter)
    {
        mainInputBelt = splitter;

        if (splitter == null)
        {
            return;
        }

        Vector3 offset = new Vector3(splitter.rightOutDirection.x, 0, splitter.rightOutDirection.z);

        Vector3 setDirection = endPositionObject.transform.position - centerObject.transform.position;

        Quaternion q = Quaternion.LookRotation(offset);
        q *= Quaternion.Euler(0, 180, 0);

        transform.rotation = q;

        SetInOutDir();
    }

    protected override void AlignMainInput(LogisticsTransfer mainInput)
    {
        if (mainInput == null)
        {
            return;
        }

        Vector3 offset = new Vector3(mainInput.outDirection.x, 0, mainInput.outDirection.z);

        Vector3 setDirection = endPositionObject.transform.position - centerObject.transform.position;

        Quaternion q = Quaternion.LookRotation(offset);
        q *= Quaternion.Euler(0, 180, 0);

        transform.rotation = q;

        SetInOutDir();
    }

    private void SetInOutDir()
    {
        if (startPositionObject == null || endPositionObject == null || centerObject == null)
        {
            Init();
        }

        outDirection = centerObject.transform.position - startPositionObject.transform.position;
        outDirection.x = Mathf.Clamp(Mathf.Round(outDirection.x), -1, 1);
        outDirection.y = Mathf.Clamp(Mathf.Round(outDirection.y), -1, 1);
        outDirection.z = Mathf.Clamp(Mathf.Round(outDirection.z), -1, 1);

        inDirection = endPositionObject.transform.position - centerObject.transform.position;
        inDirection.x = Mathf.Clamp(Mathf.Round(inDirection.x), -1, 1);
        inDirection.y = Mathf.Clamp(Mathf.Round(inDirection.y), -1, 1);
        inDirection.z = Mathf.Clamp(Mathf.Round(inDirection.z), -1, 1);
    }

    public override void MoveLogisticsTransfer()
    {
        if(logisticsObjectlist.Count < 1) 
        {
            return;
        }

        logisticsObjectlist[0].locationOffset -= speed * Time.deltaTime;

        float offset = 0.0f;

        for(int i = 0; i < logisticsObjectlist.Count; i++) 
        {
            offset += logisticsObjectlist[i].locationOffset;

            Vector3 origin = startPositionObject.transform.position - centerObject.transform.position;
            Vector3 target = endPositionObject.transform.position - centerObject.transform.position;

            Vector3 prevPos = logisticsObjectlist[i].transform.position;
            Vector3 nextPos = Vector3.Slerp(target, origin, offset) + centerObject.transform.position;
            logisticsObjectlist[i].transform.position = nextPos;

            if (nextPos - prevPos != Vector3.zero)
            {
                logisticsObjectlist[i].transform.rotation = Quaternion.LookRotation(nextPos - prevPos);
            }
            //logisticsObjectlist[i].transform.position = Vector3.Lerp(endPositionObject.transform.position, startPositionObject.transform.position, offset);
        }
    }

    protected override void Init()
    {
        base.Init();
        centerObject = transform.Find("centerPosition").gameObject;
    }

    private void Start()
    {
        Init();
        SetInOutDir();
    }
}
