using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mineral_Dissolve : MonoBehaviour
{
    [SerializeField]
    private Material originMaterial;
    [SerializeField]
    private Material dissolveMaterial;
    [SerializeField]
    private float dissolveSpeed;

    private MeshRenderer newrenderer;

    public void Dissolve()
    {
        StartCoroutine(Co_Dissolve());
    }

    private IEnumerator Co_Dissolve()
    {
        float amount = 0;
        newrenderer.material = dissolveMaterial;
        while (newrenderer.material.GetFloat("_Amount")<=1.0f)
        {
            amount += dissolveSpeed * Time.deltaTime;
            newrenderer.material.SetFloat("_Amount", amount);
            yield return null;
        }
        newrenderer.material.SetFloat("_Amount", 0.0f);
        gameObject.SetActive(false);
        yield break;
    }

    void Start()
    {
        newrenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
    }

    private void OnDisable()
    {
        newrenderer.material = originMaterial;
    }
}
