using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleDown : MonoBehaviour
{
    [SerializeField] private Transform upDoor;
    [SerializeField] private Transform downDoor;
    [SerializeField] private float openDoorSpeed;

    public bool isOpen = false;

    [SerializeField] private float itemEmitTIme;

    public int openStage;

    public bool isEmitItem;

    private DoorLight doorLight;

    private ItemGenerator itemGenerator;

    void Start()
    {
        if(isEmitItem)
        {
            itemGenerator = transform.parent.GetComponent<ItemGenerator>();
        }
        //playSound = gameObject.GetComponent<PlaySound>();
        doorLight = gameObject.GetComponent<DoorLight>();
    }

    public void ScaleDownStart()
    {
        StartCoroutine(Co_ScaleDownStart());
    }

    private IEnumerator Co_ScaleDownStart()
    {
        float yScale = upDoor.localScale.y;
        while (upDoor.localScale.y > 0.0)
        {
            yScale -= openDoorSpeed * Time.deltaTime;
            upDoor.localScale = new Vector3(upDoor.localScale.x, yScale, upDoor.localScale.z);
            downDoor.localScale = new Vector3(upDoor.localScale.x, yScale, upDoor.localScale.z);
            yield return null;
        }
        upDoor.localScale = new Vector3(upDoor.localScale.x, 0.0f, upDoor.localScale.z);
        upDoor.gameObject.SetActive(false);
        downDoor.localScale = new Vector3(upDoor.localScale.x, 0.0f, upDoor.localScale.z);
        downDoor.gameObject.SetActive(false);
        yield break;
    }

    public void ScaleUp()
    {
        isOpen = false;
        upDoor.gameObject.SetActive(true);
        upDoor.localScale = new Vector3(1, 1, 1);
        downDoor.gameObject.SetActive(true);
        downDoor.localScale = new Vector3(1, 1, 1);
        doorLight.Common();
        if(isEmitItem)
        {
            itemGenerator.StopGenerator();
        }
    }

    private void Update()
    {
        if ((Manager.Instance.gameManager.curStage >= openStage) && (!isOpen)
            && (Manager.Instance.uiManager.inGameUI.activeSelf))
        {
            isOpen = true;
            if(isEmitItem)
            {
                StartCoroutine(doorLight.EmitItem(itemEmitTIme));
            }
            ScaleDownStart();
        }

        if((!Manager.Instance.uiManager.inGameUI.activeSelf))
        {
            ScaleUp();
        }
    }
}
