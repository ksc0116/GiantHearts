using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlay : MonoBehaviour
{
    public GameObject UIManager;
    public VideoPlayer creditVideo;

    public void CreditVideoPlay()
    {
        creditVideo.Play();
    }
}
