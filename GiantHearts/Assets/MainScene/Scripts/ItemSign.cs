using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LogisticsObject;

public class ItemSign : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer signRenderer;

    [SerializeField] private LogisticsObjectType itemKind;

    [SerializeField] private Texture2D coalTexture;
    [SerializeField] private Texture2D ironTexture;
    [SerializeField] private Texture2D copperTexture;
    [SerializeField] private Texture2D mythrilTexture;
    [SerializeField] private Texture2D meteorTexture;
    [SerializeField] private Texture2D mythrilAndCoalTexture;

    private void SetItemTexture()
    {                                                                                                                                                                                           
        switch (itemKind)
        {
            case LogisticsObjectType.Coal:
                {
                    signRenderer.material.SetTexture("_MainTex", coalTexture);
                }
                break;
            case LogisticsObjectType.Iron:
                {
                    signRenderer.material.SetTexture("_MainTex", ironTexture);
                }
                break;
            case LogisticsObjectType.Copper:
                {
                    signRenderer.material.SetTexture("_MainTex", copperTexture);
                }
                break;
            case LogisticsObjectType.Mythril:
                {
                    signRenderer.material.SetTexture("_MainTex", mythrilTexture);
                }
                break;
            case LogisticsObjectType.Meteorite:
                {
                    signRenderer.material.SetTexture("_MainTex", meteorTexture);
                }
                break;
            case LogisticsObjectType.MythrilAndCoal:
                {
                    signRenderer.material.SetTexture("_MainTex", mythrilAndCoalTexture);
                }
                break;
        }
    }

    void Awake()
    {
        SetItemTexture();
    }

}
