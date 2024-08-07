using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemReceiver : LogisticsTransfer
{
    [SerializeField]
    private LogisticsObject.LogisticsObjectType CheckType;
    private const float generatorCoolTime = 1.5f;
    private float elapseTime;

    protected override void SetMainInput(LogisticsTransfer mainInput)
    {
        mainInputBelt = mainInput;
    }

    public override void SetMainInputAtRigntOut(Splitter splitter) 
    {
        mainInputBelt = splitter;
    }

    protected override void AlignMainInput(LogisticsTransfer mainInput) { }
    public override void MoveLogisticsTransfer()
    {
        if(logisticsObjectlist.Count < 1)
        {
            return;
        }

        logisticsObjectlist[0].locationOffset -= speed * Time.deltaTime;

        float offset = 0.0f;

        for(int i = 0; i < logisticsObjectlist.Count; ++i)
        {
            offset += logisticsObjectlist[i].locationOffset;
            Vector3 prevPos = logisticsObjectlist[i].transform.position;
            Vector3 nextPos = Vector3.Lerp(endPositionObject.transform.position, startPositionObject.transform.position, offset);
            logisticsObjectlist[i].transform.position = nextPos;

            if(nextPos - prevPos != Vector3.zero)
            {
                logisticsObjectlist[i].transform.rotation = Quaternion.LookRotation(nextPos - prevPos);
            }
        }
    }

    private void SetInOutDir()
    {
        outDirection = Vector3.zero;

        inDirection = endPositionObject.transform.position - startPositionObject.transform.position;
        inDirection.x = Mathf.Clamp(Mathf.Round(inDirection.x), -1, 1);
        inDirection.y = Mathf.Clamp(Mathf.Round(inDirection.y), -1, 1);
        inDirection.z = Mathf.Clamp(Mathf.Round(inDirection.z), -1, 1);
    }

    void Start()
    {
        Init();
        SetInOutDir();
    }

    new void Update()
    {
        if (logisticsObjectlist.Count <= 0)
        {
            return;
        }
        // 오브젝트를 다음 벨트로 이동할 조건이 만족하지 않았으면 리턴.
        if (logisticsObjectlist[0].locationOffset > float.Epsilon)
        {
            return;
        }

        if (logisticsObjectlist[0].gameObject.GetComponent<LogisticsObject>().type 
            == CheckType)
        {
            // 성공시 로직
            transform.Find("EntranceDoor").GetComponent<DoorLight>().Correct();
            Debug.Log("성공");
        }
        else
        {
            // 실패시 로직
            transform.Find("EntranceDoor").GetComponent<DoorLight>().Wrong();
            Debug.Log("실패");
        }

        logisticsObjectlist[0].GetComponent<Mineral_Dissolve>().Dissolve();
        logisticsObjectlist.RemoveAt(0);
    }
}
