using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public UIManager m_UIOjbect;
    public Gridsystem m_Gridsystem;

    // ������Ʈ ��������
    private Rigidbody playerRigidbody;

    public float moveSpeed;
    private Vector3 playerPos;

    // �Ǽ���� ����
    private static bool m_isBuildMode;

    // GameMuen UI ����
    private bool m_isGameMenuUI;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerPos = transform.position;
        
        m_isBuildMode = false;  // �Ǽ����
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (!m_isGameMenuUI) // GameMenuUI�� �����ִٸ�
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                BuildModeOnOff();
            }
        }
    }

    private void Move()
    {
        float hAxis = Input.GetAxisRaw("Horizontal");
        float vAxis = Input.GetAxisRaw("Vertical");

        Vector3 inputDir = new Vector3(hAxis, 0, vAxis).normalized;
        playerRigidbody.velocity = inputDir * moveSpeed;
        transform.LookAt(transform.position + inputDir);

    }

    private void BuildModeOnOff()
    {
        m_isBuildMode = m_isBuildMode == true ? false : true;

        m_UIOjbect.BuildModeUIActiveSelfSetting();
    }
}
