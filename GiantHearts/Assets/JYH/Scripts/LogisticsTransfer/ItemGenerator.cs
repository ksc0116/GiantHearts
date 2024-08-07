using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : LogisticsTransfer
{
    [SerializeField]
    private List<GameObject> targetGameObjectList;
    private int currGenerateIndex = 0;
    private const float generatorCoolTime = 4.0f;
    private float elapseTime = 4.0f;
    private bool isGeneratorOn = false;
    private ObjectPoolSystem objectPool;

    protected override void SetMainInput(LogisticsTransfer mainInput) { }
    public override void SetMainInputAtRigntOut(Splitter splitter) { }
    protected override void AlignMainInput(LogisticsTransfer mainInput) { }
    public override void MoveLogisticsTransfer()
    {
        if (logisticsObjectlist.Count < 1)
        {
            return;
        }

        logisticsObjectlist[0].locationOffset -= speed * Time.deltaTime;

        float offset = 0.0f;
        for (int i = 0; i < logisticsObjectlist.Count; ++i)
        {
            offset += logisticsObjectlist[i].locationOffset;

            Vector3 prevPos = logisticsObjectlist[i].transform.position;
            Vector3 nextPos = Vector3.Lerp(endPositionObject.transform.position, startPositionObject.transform.position, offset);
            logisticsObjectlist[i].transform.position = nextPos;
            logisticsObjectlist[i].transform.rotation = Quaternion.LookRotation(nextPos - prevPos);
        }
    }

    public void StartGenerator()
    {
        isGeneratorOn = true;
    }

    public void StopGenerator()
    {
        isGeneratorOn = false;
    }

    private void SetInOutDir()
    {
        outDirection = endPositionObject.transform.position - startPositionObject.transform.position;
        outDirection.x = Mathf.Clamp(Mathf.Round(outDirection.x), -1, 1);
        outDirection.y = Mathf.Clamp(Mathf.Round(outDirection.y), -1, 1);
        outDirection.z = Mathf.Clamp(Mathf.Round(outDirection.z), -1, 1);
        inDirection = Vector3.zero;
    }
    // Start is called before the first frame update
    void Start()
    {
        Init();
        SetInOutDir();

        objectPool = Manager.Instance.objectPool;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if(isGeneratorOn == false)
        {
            return;
        }

        elapseTime += Time.deltaTime;

        if(elapseTime < generatorCoolTime) 
        {
            return;
        }

        if (logisticsObjectlist.Count >= threshold)
        {
            return;
        }

        elapseTime -= generatorCoolTime;

        GameObject item= null;
        if (objectPool == null)
        {
            item = Instantiate(targetGameObjectList[currGenerateIndex], startPositionObject.transform.position, Quaternion.identity);
        }
        else 
        {
            item = objectPool.TakeOutObject(targetGameObjectList[currGenerateIndex].tag);
            item.transform.position = startPositionObject.transform.position;
            item.transform.rotation = Quaternion.identity;
            item.SetActive(true);
        }
        
        currGenerateIndex += 1;
        currGenerateIndex %= targetGameObjectList.Count;

        if(item != null)
            AddLogisticsObject(item.GetComponent<LogisticsObject>());
    }
}
