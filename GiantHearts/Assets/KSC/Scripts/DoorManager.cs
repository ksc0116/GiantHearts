using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    private PlaySound playSound;
    public GameObject[] doors;

    public void DoorClose()
    {
        // ScaleUp함수에서 isOpen을 false로 바꿔 바로 다시 크기가 작아지니
        // 문 열림 소리
        playSound.PlaySE(Manager.Instance.manager_SE.ES_hatch_open);

        for (int i = 0; i < doors.Length; ++i)
        {
            doors[i].GetComponent<ScaleDown>().ScaleUp();
        }
    }

    void Start()
    {
        playSound = GetComponent<PlaySound>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
