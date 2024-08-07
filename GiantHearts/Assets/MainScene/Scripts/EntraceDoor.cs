using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntraceDoor : MonoBehaviour
{
    [SerializeField]
    private Texture2D worngTexture;
    [SerializeField]
    private Texture2D successTexture;

    [SerializeField]
    private float accTime;
    [SerializeField]
    private int twinkelMaxCount;
    [SerializeField]
    private float twinkelSpeed;
    [SerializeField]
    private float twinkelTime;

    [SerializeField]
    private bool isWrong;
    [SerializeField]
    private bool isSuccess;

    [SerializeField]
    private MeshRenderer newRenderer;

    [SerializeField]
    private Transform upDoor;
    [SerializeField]
    private Transform downDoor;
    [SerializeField]
    private float openDoorSpeed;

    [SerializeField]
    private int openStage;

    private void OpenDoor()
    {
        StartCoroutine(Co_OpenDoor());  
    }

    private IEnumerator Co_OpenDoor()
    {
        float yScale = upDoor.localScale.y;
        while (upDoor.localScale.y > 0.0)
        {
            Debug.Log("문 여는 중");
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

    public void Wrong()
    {
        newRenderer.material.SetTexture("_EmissionTex", worngTexture);
        StartCoroutine (Co_Wrong());
    }

    public void Correct()
    {
        newRenderer.material.SetFloat("_EmissionIntensity", 1.0f);
        newRenderer.material.SetTexture("_EmissionTex", successTexture);
    }

    public void Common()
    {
        //newRenderer.material.SetFloat("_EmissionIntensity", 1.0f);
        newRenderer.material.SetTexture("_EmissionTex", null);
    }

    private IEnumerator Co_Wrong()
    {
        Debug.Log("틀림");
        int toggle = 1;
        int curCount = 0;

        while(curCount < twinkelMaxCount * 2)
        {
            accTime += toggle * twinkelSpeed * Time.deltaTime;
            newRenderer.material.SetFloat("_EmissionIntensity", (accTime / twinkelTime));
            if (curCount %2 ==0)
            {
                if (newRenderer.material.GetFloat("_EmissionIntensity") >= 1.0f)
                {
                    curCount++;
                    toggle *= -1;
                }
            }
            else
            {
                if (newRenderer.material.GetFloat("_EmissionIntensity") <= 0.0f)
                {
                    curCount++;
                    toggle *= -1;
                }
            }
            
            yield return null;
        }

        newRenderer.material.SetTexture("_EmissionTex", null);
        yield break;
    }

    void Update()
    {
        
    }
}
