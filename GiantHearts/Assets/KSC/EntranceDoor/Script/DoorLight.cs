using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLight : MonoBehaviour
{
    [SerializeField] private Texture2D worngTexture;
    [SerializeField] private Texture2D successTexture;
    [SerializeField] private Texture2D emitTexture;

    [SerializeField] private float twinkelAccTime;
    [SerializeField] private int twinkelMaxCount;
    [SerializeField] private float twinkelSpeed;
    [SerializeField] private float twinkelTime;

    [SerializeField] private MeshRenderer newRenderer;

    [SerializeField] private float commonDelay;
    [SerializeField] private float commonAccTime;

    [SerializeField] private int correctIndex;

    private ScaleDown scaleDown;

    private Coroutine runningCoroutine;

    private PlaySound playSound;

    private ItemGenerator itemGenerator;
    void Start()
    {
        itemGenerator = transform.parent.GetComponent<ItemGenerator>();
        playSound = gameObject.GetComponent<PlaySound>();
        scaleDown = gameObject.GetComponent<ScaleDown>();
    }

    public void Wrong()
    {
        // 버저 에러
        playSound.PlaySE(Manager.Instance.manager_SE.ES_Buzzer_error);
        commonAccTime = 0.0f;
        Manager.Instance.gameManager.isStageClear[scaleDown.openStage][correctIndex] = false;
        newRenderer.material.SetTexture("_EmissionTex", worngTexture);
        newRenderer.material.SetFloat("_EmissionIntensity", 0.0f);
        // 버저 빨간색 중복 방지 확인 필요
        if (runningCoroutine != null)
        {
            StopCoroutine(runningCoroutine);
        }
        runningCoroutine = StartCoroutine(Co_Wrong());
    }

    public void Correct()
    {
        commonAccTime = 0.0f;
        newRenderer.material.SetTexture("_EmissionTex", successTexture);
        newRenderer.material.SetFloat("_EmissionIntensity", 1.0f);
        Manager.Instance.gameManager.isStageClear[scaleDown.openStage][correctIndex] = true;
    }

    public void Common()
    {
        Manager.Instance.gameManager.isStageClear[scaleDown.openStage][correctIndex] = false;
        newRenderer.material.SetTexture("_EmissionTex", null);
        newRenderer.material.SetFloat("_EmissionIntensity", 0.0f);
    }

    public IEnumerator EmitItem(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        newRenderer.material.SetTexture("_EmissionTex", emitTexture);
        newRenderer.material.SetFloat("_EmissionIntensity", 1.0f);
        if(itemGenerator != null)
        {
            itemGenerator.StartGenerator();
        }
    }

    private IEnumerator Co_Wrong()
    {
        Debug.Log("틀림");
        int toggle = 1;
        int curCount = 0;
        float intensity = 0.0f;

        while (curCount < twinkelMaxCount * 2)
        {
            //twinkelAccTime += toggle * twinkelSpeed * Time.deltaTime;
            intensity += toggle * twinkelSpeed * Time.deltaTime;
            //newRenderer.material.SetFloat("_EmissionIntensity", (twinkelAccTime / twinkelTime));
            newRenderer.material.SetFloat("_EmissionIntensity", intensity);
            if (curCount % 2 == 0)
            {
                if (newRenderer.material.GetFloat("_EmissionIntensity") >= 0.35f)
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
        if(Manager.Instance.gameManager.curStage >= scaleDown.openStage)
        {
            if (!scaleDown.isEmitItem)
            {
                commonAccTime += Time.deltaTime;

                if (commonAccTime >= commonDelay)
                {
                    commonAccTime = 0.0f;
                    Common();
                }
            }
        }
    }
}
