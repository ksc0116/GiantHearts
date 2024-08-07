using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class LogisticsTransfer : MonoBehaviour
{
    protected float speed = 0.5f;

    protected GameObject startPositionObject;
    protected GameObject endPositionObject;
    protected List<GameObject> guidePoints;
    protected List<LogisticsObject> logisticsObjectlist = new List<LogisticsObject>(3);
    protected int threshold = 3;
    protected LogisticsTransfer mainInputBelt;
    [SerializeField]
    protected LogisticsTransfer outputBelt;
    public Vector3 outDirection;
    public Vector3 inDirection;

    public virtual void ConnectNextBelt(LogisticsTransfer targetBelt)
    {
        targetBelt.SetMainInput(this);
        SetOutput(targetBelt);
    }

    public virtual void AlignNextBelt(LogisticsTransfer targetBelt)
    {
        targetBelt.AlignMainInput(this);
    }

    protected abstract void SetMainInput(LogisticsTransfer mainInput);

    public abstract void SetMainInputAtRigntOut(Splitter splitter);

    protected abstract void AlignMainInput(LogisticsTransfer mainInput);

    public void SetOutput(LogisticsTransfer output)
    {
        this.outputBelt = output;
    }

    public Vector3 GetOutBeltDirection()
    {
        return outDirection;
    }

    public abstract void MoveLogisticsTransfer();

    protected Vector3 CalcBezierCurvesRecursive(List<Vector3> points, float t)
    {
        if(points.Count == 1)
        {
            return points[0];
        }

        for(int i = 1; i < points.Count; ++i)
        {
            points[i - 1] = Vector3.Lerp(points[i - 1], points[i], t);
        }

        points.RemoveAt(points.Count - 1);

        return CalcBezierCurvesRecursive(points, t);
    }
    public bool AddLogisticsObject(LogisticsObject logisticsObject)
    {
        if (logisticsObjectlist.Count >= threshold)
        {
            return false;
        }

        logisticsObjectlist.Insert(logisticsObjectlist.Count, logisticsObject);

        float offset = 0.0f;

        for(int i = 0; i < logisticsObjectlist.Count; ++i)
        {
            offset += logisticsObjectlist[i].locationOffset;
        }

        logisticsObject.locationOffset = 1.0f - offset;
        return true;
    }
    protected virtual void Init()
    {
        startPositionObject = transform.Find("startPosition").gameObject;
        endPositionObject = transform.Find("endPosition").gameObject;
    }

    protected void Update()
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
        if (outputBelt == null)
        {
            logisticsObjectlist[0].GetComponent<Mineral_Dissolve>().Dissolve();
            //Destroy(logisticsObjectlist[0].gameObject);
        }
        else
        {
            logisticsObjectlist[0].transform.position = endPositionObject.transform.position;
            if (!outputBelt.AddLogisticsObject(logisticsObjectlist[0]))
            {
                return;
            }
        }
        logisticsObjectlist.RemoveAt(0);
    }

    protected void LateUpdate()
    {
        MoveLogisticsTransfer();
    }

    protected void OnDisable()
    {
        foreach(var obj in logisticsObjectlist)
        {
            if(obj == null)
            {
                continue;
            }

            obj.GetComponent<Mineral_Dissolve>().Dissolve();
        }

        logisticsObjectlist.Clear();

        if(mainInputBelt != null)
        {
            mainInputBelt.outputBelt = null;
            mainInputBelt = null;
        }

        if(outputBelt != null)
        {
            outputBelt.mainInputBelt = null;
            outputBelt = null;
        }

    }

    public LogisticsTransfer GetInPutObject()
    {
        return mainInputBelt;
    }

    public void OutPutObjectMakeNull()
    {
        outputBelt = null;
    }
}
