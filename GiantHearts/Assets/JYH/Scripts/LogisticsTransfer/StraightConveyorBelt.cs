using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightConveyorBelt : LogisticsTransfer
{
    public override void MoveLogisticsTransfer()
    {
        if(logisticsObjectlist.Count < 1)
        {
            return;
        }

        logisticsObjectlist[0].locationOffset -= speed * Time.deltaTime;

        float offset = 0.0f;
        for (int i = 0; i < logisticsObjectlist.Count; ++i)
        {
            offset += logisticsObjectlist[i].locationOffset;

            List<Vector3> points = new List<Vector3>();
            points.Add(startPositionObject.transform.position);
            points.Add(endPositionObject.transform.position);

            float bezierOffset = 1.0f - offset;

            Vector3 prevPos = logisticsObjectlist[i].transform.position;
            Vector3 nextPos = CalcBezierCurvesRecursive(points, bezierOffset);

            logisticsObjectlist[i].transform.position = nextPos;

            if (nextPos - prevPos != Vector3.zero)
            {
                logisticsObjectlist[i].transform.rotation = Quaternion.LookRotation(nextPos - prevPos);
            }

            //logisticsObjectlist[i].transform.position = Vector3.Lerp(endPositionObject.transform.position, startPositionObject.transform.position, offset);
        }
    }

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

        Quaternion q = Quaternion.LookRotation(offset);
        transform.rotation = q;
        transform.rotation *= Quaternion.Euler(0, 90, 0);

        SetInOutDir();
    }

    protected override void AlignMainInput(LogisticsTransfer mainInput)
    {
        if (mainInput == null)
        {
            return;
        }

        Vector3 offset = new Vector3(mainInput.outDirection.x, 0, mainInput.outDirection.z);

        Quaternion q = Quaternion.LookRotation(offset);
        transform.rotation = q;
        transform.rotation *= Quaternion.Euler(0, 90, 0);

        SetInOutDir();
    }
    private void SetInOutDir()
    {
        if(startPositionObject == null || endPositionObject == null)
        {
            Init();
        }
        outDirection = endPositionObject.transform.position - startPositionObject.transform.position;
        outDirection.x = Mathf.Clamp(Mathf.Round(outDirection.x), -1, 1);
        outDirection.y = Mathf.Clamp(Mathf.Round(outDirection.y), -1, 1);
        outDirection.z = Mathf.Clamp(Mathf.Round(outDirection.z), -1, 1);

        inDirection = startPositionObject.transform.position - endPositionObject.transform.position;
        inDirection.x = Mathf.Clamp(Mathf.Round(inDirection.x), -1, 1);
        inDirection.y = Mathf.Clamp(Mathf.Round(inDirection.y), -1, 1);
        inDirection.z = Mathf.Clamp(Mathf.Round(inDirection.z), -1, 1);
    }
    private void Start()
    {
        Init();
    }
}
