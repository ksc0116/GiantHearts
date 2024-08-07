using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public void PlaySE(AudioClip clip)
    {
        Manager.Instance.manager_SE.seAudio.PlayOneShot(clip);
    }
}
