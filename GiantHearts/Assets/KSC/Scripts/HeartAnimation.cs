using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartAnimation : MonoBehaviour
{
    private Animator anim;
    public float animSpeed = 1.0f;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if(Manager.Instance.gameManager.curStage == 0)
        {
            animSpeed = 0.0f;
            anim.SetFloat("AnimSpeed", animSpeed);
        }
        else if(Manager.Instance.gameManager.curStage == 1)
        {
            animSpeed = 0.3f;
            anim.SetFloat("AnimSpeed", animSpeed);
        }
        else if (Manager.Instance.gameManager.curStage == 2)
        {
            animSpeed = 0.6f;
            anim.SetFloat("AnimSpeed", animSpeed);
        }
        else
        {
            animSpeed = 1.0f;
            anim.SetFloat("AnimSpeed", animSpeed);
        }
    }
}
