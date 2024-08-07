using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    private PlaySound playSound;
    public GameObject[] doors;

    public void DoorClose()
    {
        // ScaleUp�Լ����� isOpen�� false�� �ٲ� �ٷ� �ٽ� ũ�Ⱑ �۾�����
        // �� ���� �Ҹ�
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
