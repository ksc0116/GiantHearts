using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_SE : MonoBehaviour
{
    public AudioSource seAudio;

    [Header("Clip")]
    // 클립들 넣기
    public AudioClip ES_Buzzer_error;
    public AudioClip ES_Click;
    public AudioClip ES_Delete;
    public AudioClip ES_Footstep;
    public AudioClip ES_hatch_open;
    public AudioClip ES_Hit;

    // StartButton 클릭 시
    public void SE_StartButtonCilck()
    {
        seAudio.PlayOneShot(ES_Click);
    }
}
