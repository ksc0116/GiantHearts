using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart_Arrow : MonoBehaviour
{
    [SerializeField] private float playTime;
    private float accPlayTime;

    private Animator anim;
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if(Manager.Instance.gameManager.curStage >0)
        {
            accPlayTime += Time.deltaTime;
            if (accPlayTime >= playTime)
            {
                accPlayTime = 0.0f;
                anim.SetTrigger("onArrow");
            }
        }
    }
}
