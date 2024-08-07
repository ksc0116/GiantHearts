using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart_Bulb : MonoBehaviour
{
    private MeshRenderer newrenderer;

    [SerializeField] private float ableTime;
    private float accAbleTime;

    [SerializeField] private float twinkelTime;
    private float accTwinkelTime;

    private bool isTwinkel;

    void Start()
    {
        newrenderer = gameObject.GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if(Manager.Instance.gameManager.curStage != 0)
        {
            newrenderer.materials[1].SetFloat("_EmissionIntensity", 1.0f);
        }

        if((Manager.Instance.gameManager.curStage == 0) && (isTwinkel == false))
        {
            accAbleTime+=Time.deltaTime;

            if(accAbleTime >= ableTime)
            {
                accAbleTime = 0.0f;
                isTwinkel = true;
            }

        }

        if(isTwinkel)
        {
            accTwinkelTime += Time.deltaTime;
            float intensity = Random.Range(0.0f, 1.0f);
            newrenderer.materials[1].SetFloat("_EmissionIntensity", intensity);
            if (accTwinkelTime >= twinkelTime)
            {
                newrenderer.materials[1].SetFloat("_EmissionIntensity", 1.0f);
                accTwinkelTime = 0.0f;
                isTwinkel = false;
            }
        }
    }
}
