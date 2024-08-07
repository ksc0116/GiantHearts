using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LogisticsObject;

public class ClassifierLight : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer lightRenderer;

    [SerializeField] private LogisticsObjectType itemKind;

    [SerializeField] private Texture2D coalTexture;
    [SerializeField] private Texture2D ironTexture;
    [SerializeField] private Texture2D copperTexture;
    [SerializeField] private Texture2D mythrilTexture;
    [SerializeField] private Texture2D meteorTexture;

    public void SetTexture(LogisticsObjectType p_kind)
    {
        lightRenderer.material.SetFloat("_EmissionIntensity", 1.0f);
        switch (p_kind)
        {
            case LogisticsObjectType.None:
                {
                    lightRenderer.material.SetTexture("_EmissionTex", null);
                }
                break;
            case LogisticsObjectType.Coal:
                {
                    lightRenderer.material.SetTexture("_EmissionTex", coalTexture);
                }
                break;
            case LogisticsObjectType.Copper:
                {
                    lightRenderer.material.SetTexture("_EmissionTex", copperTexture);
                }
                break;
            case LogisticsObjectType.Iron:
                {
                    lightRenderer.material.SetTexture("_EmissionTex", ironTexture);
                }
                break;
            case LogisticsObjectType.Meteorite:
                {
                    lightRenderer.material.SetTexture("_EmissionTex", meteorTexture);
                }
                break;
            case LogisticsObjectType.Mythril:
                {
                    lightRenderer.material.SetTexture("_EmissionTex", mythrilTexture);
                }
                break;
        }
    }
}
