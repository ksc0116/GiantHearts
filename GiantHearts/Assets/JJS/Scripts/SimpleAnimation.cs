using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SimpleAnimation : MonoBehaviour
{
    public Animation anim;
    public AnimationClip clip;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animation>();
        anim.Play("Body");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
