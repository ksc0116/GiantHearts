using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Splitter : LogisticsTransfer
{
    protected GameObject centerPositionObject;
    protected GameObject rightSideEndPositionObejct;
    private bool isOutLeftSide = true;
    public Vector3 rightOutDirection;

    [SerializeField]
    protected LogisticsTransfer rightSideOutputBelt;

    public override void MoveLogisticsTransfer()
    {
        if (logisticsObjectlist.Count < 1)
        {
            return;
        }

        logisticsObjectlist[0].locationOffset -= speed * Time.deltaTime;

        float offset = 0.0f;
        bool isNowOutLeftSide = isOutLeftSide;

        for (int i = 0; i < logisticsObjectlist.Count; ++i)
        {
            offset += logisticsObjectlist[i].locationOffset;
            float bezierOffset = 1.0f - offset;

            List<Vector3> points = new List<Vector3>();
            if(isNowOutLeftSide == true)
            {
                points.Add(startPositionObject.transform.position);
                points.Add(centerPositionObject.transform.position);
                points.Add(endPositionObject.transform.position);
            }
            else 
            {
                points.Add(startPositionObject.transform.position);
                points.Add(centerPositionObject.transform.position);
                points.Add(rightSideEndPositionObejct.transform.position);
            }

            Vector3 prevPos = logisticsObjectlist[i].transform.position;
            Vector3 nextPos = CalcBezierCurvesRecursive(points, bezierOffset);
            logisticsObjectlist[i].transform.position = nextPos;

            if (nextPos - prevPos != Vector3.zero)
            {
                logisticsObjectlist[i].transform.rotation = Quaternion.LookRotation(nextPos - prevPos);
            }

            isNowOutLeftSide = !isNowOutLeftSide;
        }
    }

    public void ConnectNextBeltToRightOut(LogisticsTransfer targetBelt)
    {
        targetBelt.SetMainInputAtRigntOut(this);

        rightSideOutputBelt = targetBelt;
    }

    protected override void Init()
    {
        startPositionObject = transform.Find("startPosition").gameObject;
        endPositionObject = transform.Find("endPosition").gameObject;

        centerPositionObject = transform.Find("centerPosition").gameObject;
        rightSideEndPositionObejct = transform.Find("endPosition1").gameObject;
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

        transform.rotation = Quaternion.LookRotation(offset);
        transform.rotation *= Quaternion.Euler(0, -90, 0);

        SetInOutDir();
    }

    protected override void AlignMainInput(LogisticsTransfer mainInput)
    {
         if (mainInput == null)
        {
            return;
        }

        Vector3 offset = new Vector3(mainInput.outDirection.x, 0, mainInput.outDirection.z);

        transform.rotation = Quaternion.LookRotation(offset);
        transform.rotation *= Quaternion.Euler(0, -90, 0);

        SetInOutDir();
    }
    private void SetInOutDir()
    {
        if (startPositionObject == null || endPositionObject == null || centerPositionObject == null)
        {
            Init();
        }
        outDirection = endPositionObject.transform.position - centerPositionObject.transform.position;
        outDirection.x = Mathf.Clamp(Mathf.Round(outDirection.x), -1, 1);
        outDirection.y = Mathf.Clamp(Mathf.Round(outDirection.y), -1, 1);
        outDirection.z = Mathf.Clamp(Mathf.Round(outDirection.z), -1, 1);

        rightOutDirection = rightSideEndPositionObejct.transform.position - centerPositionObject.transform.position;
        rightOutDirection.x = Mathf.Clamp(Mathf.Round(rightOutDirection.x), -1, 1);
        rightOutDirection.y = Mathf.Clamp(Mathf.Round(rightOutDirection.y), -1, 1);
        rightOutDirection.z = Mathf.Clamp(Mathf.Round(rightOutDirection.z), -1, 1);

        inDirection = centerPositionObject.transform.position - startPositionObject.transform.position;
        inDirection.x = Mathf.Clamp(Mathf.Round(inDirection.x), -1, 1);
        inDirection.y = Mathf.Clamp(Mathf.Round(inDirection.y), -1, 1);
        inDirection.z = Mathf.Clamp(Mathf.Round(inDirection.z), -1, 1);
    }

    private void Start()
    {
        Init();
        SetInOutDir();
    }

    new protected void Update()
    {
        if (logisticsObjectlist.Count <= 0)
        {
            return;
        }
        // ������Ʈ�� ���� ��Ʈ�� �̵��� ������ �������� �ʾ����� ����.
        if (logisticsObjectlist[0].locationOffset > float.Epsilon)
        {
            return;
        }

        // ���� ��Ʈ�� ���ٸ� �ı�.
        // ��Ʈ�� �ִٸ� �ش� ��Ʈ�� ������Ʈ �߰��� �õ��ϰ� �����ϸ� ���� �����ӿ� �ٽ� �õ�.
        if (isOutLeftSide == true)
        {
            // ���� �Һ��� �ٲ�����.
            transform.Find("Classifier").transform.Find("Machine_Light_Left").GetComponent<ClassifierLight>().SetTexture(logisticsObjectlist[0].type);

            if (outputBelt != null)
            {
                logisticsObjectlist[0].transform.position = endPositionObject.transform.position;

                if (!outputBelt.AddLogisticsObject(logisticsObjectlist[0]))
                {
                    logisticsObjectlist[0].GetComponent<Mineral_Dissolve>().Dissolve();
                    logisticsObjectlist.RemoveAt(0);
                    return;
                }
            }
            else
            {
                logisticsObjectlist[0].GetComponent<Mineral_Dissolve>().Dissolve();
            }

        }
        else if(isOutLeftSide == false)
        {
            // ������ �Һ��� �ٲ�����
            transform.Find("Classifier").transform.Find("Machine_Light_Right").GetComponent<ClassifierLight>().SetTexture(logisticsObjectlist[0].type);

            if (rightSideOutputBelt != null)
            {
                logisticsObjectlist[0].transform.position = rightSideEndPositionObejct.transform.position;

                if (!rightSideOutputBelt.AddLogisticsObject(logisticsObjectlist[0]))
                {
                    logisticsObjectlist[0].GetComponent<Mineral_Dissolve>().Dissolve();
                    logisticsObjectlist.RemoveAt(0);
                    return;
                }
            }
            else 
            {
                logisticsObjectlist[0].GetComponent<Mineral_Dissolve>().Dissolve();
            }

        }

        isOutLeftSide = !isOutLeftSide;

        logisticsObjectlist.RemoveAt(0);
    }

    public LogisticsTransfer GetRightSideObject()
    {
        return rightSideOutputBelt;
    }

    public void RightSideObjectMakeNull()
    {
        rightSideOutputBelt = null;
    }
}
