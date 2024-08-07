using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart_Gear : MonoBehaviour
{
    private Animator anim;
    public float animSpeed;
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if(Manager.Instance.gameManager.curStage == 2)
        {
            animSpeed = 0.3f;
        }
        else if(Manager.Instance.gameManager.curStage == 3)
        {
            animSpeed = 0.5f;
        }
        else
        {
            animSpeed = 0.0f;
        }
        anim.SetFloat("AnimSpeed", animSpeed);
    }
}
